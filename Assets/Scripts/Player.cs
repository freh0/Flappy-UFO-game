using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	public float forceUP = 2f;

	private Rigidbody2D phys;
	private SpriteRenderer sr;
	private int lastTouches;
	private bool isGameOver;
	private float initGravity;
	private Vector3 initPos;

	void OnCollisionEnter2D(Collision2D collider) {
		Messenger.Broadcast (GameEvent.Game_Over);
		isGameOver = true;
	}

	void StartGame() {
		transform.position = initPos;

		StartCoroutine (NormalizeGravity ());
		phys.velocity = Vector2.zero;
		isGameOver = false;

		StartCoroutine (MakePlayerVisible());
	}

	IEnumerator NormalizeGravity() {
		for (int i = 1; i <= 20; i++) {
			phys.gravityScale = Mathf.Lerp (initGravity, 0, 1f / i);

			yield return new WaitForSeconds (0.3f);
		}
	}

	IEnumerator MakePlayerVisible() {
		for (int i = 20; i >= 1; i--) {
			Color color = sr.color;
			color.a = 1f / i;
			sr.color = color;

			yield return new WaitForSeconds (0.02f);
		}
	}

	void Start () {
		initPos = transform.position;

		sr = GetComponent<SpriteRenderer> ();
		phys = GetComponent<Rigidbody2D> ();
		initGravity = phys.gravityScale;

		lastTouches = 0;
		isGameOver = true;

		Color color = sr.color;
		color.a = 0;
		sr.color = color;
	}
		
	void Update () {
		
		if (!isGameOver) {
			int touches = Input.touchCount;
		
			if (Input.GetMouseButtonDown (0) || touches > lastTouches) {
				phys.velocity = Vector2.zero;
				phys.AddForce (Vector2.up * forceUP, ForceMode2D.Impulse);
			}

			lastTouches = touches;
		} else {
			//phys.gravityScale = 0;
			//phys.velocity = Vector2.zero;
		}
	}

	void Awake() {
		Messenger.AddListener (GameEvent.StartGame, StartGame);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.StartGame, StartGame);
	}
}