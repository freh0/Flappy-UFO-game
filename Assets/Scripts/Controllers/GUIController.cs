using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {

	[SerializeField] private CanvasRenderer[] UI;
	[SerializeField] private SpriteRenderer[] Sprites;

	private float[] initUIalphas;
	private float[] initSpritesA;

	enum State {
		StartMenu,
		Play,
		Fall
	}

	private State currentState;
	private int lastTouches;

	void Init() {
		initUIalphas = new float[UI.Length];
		initSpritesA = new float[Sprites.Length];

		for (int i = 0; i < initUIalphas.Length; i++)
			initUIalphas [i] = UI [i].GetAlpha ();

		for (int i = 0; i < initSpritesA.Length; i++)
			initSpritesA [i] = Sprites [i].color.a;
	}

	void Start () {
		int touches = Input.touchCount;

		currentState = State.StartMenu;

		Init ();
	}

	public void StartGame() {
		StartCoroutine (HideShowUI (true));
		StartCoroutine (HideShowSprites (true));
	}

	public void ShowUI() {
		StartCoroutine(HideShowUI (false));
		StartCoroutine(HideShowSprites (false));
	}

	IEnumerator HideShowUI(bool needHide) {
		int n = 20;

		if (needHide) {
			for (int j = 1; j <= n; j++) {
				for (int i = 0; i < UI.Length; i++) {
					UI [i].SetAlpha (Mathf.Lerp (initUIalphas [i], 0, (float)j / n));
				}

				yield return new WaitForSeconds (0.015f);
			}

			for (int i = 0; i < UI.Length; i++)
				UI [i].gameObject.SetActive (false);

			Messenger.Broadcast (GameEvent.StartGame);
		} else {
			for (int i = 0; i < UI.Length; i++)
				UI [i].gameObject.SetActive (true);

			for (int j = 1; j <= n; j++) {
				for (int i = 0; i < UI.Length; i++) {
					UI [i].SetAlpha (Mathf.Lerp (0, initUIalphas [i], (float)j / n));
				}

				yield return new WaitForSeconds (0.015f);
			}
		}
	}

	IEnumerator HideShowSprites(bool needHide) {
		int n = 20;

		if (needHide) {
			for (int j = 1; j <= n; j++) {
				for (int i = 0; i < Sprites.Length; i++) {
					Color color = Sprites [i].color;

					float alpha = Mathf.Lerp (initSpritesA [i], 0, (float)j / n);
					color.a = alpha;

					Sprites [i].color = color;
				}

				yield return new WaitForSeconds (0.015f);
			}

			for (int i = 0; i < UI.Length; i++)
				UI [i].gameObject.SetActive (false);
		} else {
			for (int i = 0; i < UI.Length; i++)
				UI [i].gameObject.SetActive (true);

			for (int j = 1; j <= n; j++) {
				for (int i = 0; i < Sprites.Length; i++) {
					Color color = Sprites [i].color;

					float alpha = Mathf.Lerp (0, initSpritesA [i], (float)j / n);
					color.a = alpha;

					Sprites [i].color = color;
				}

				yield return new WaitForSeconds (0.015f);
			}
		}
	}

	void Update () {
		switch(currentState) {
		case State.StartMenu:
			int touches = Input.touchCount;

			if (Input.GetMouseButtonDown (0) || touches > lastTouches) {
				currentState = State.Play;

			}

			lastTouches = touches;
			break;
		}
	}

	void OnGameOver() {
		//PlayerPrefs.SetInt("HighestScore", 
	}

	void Awake() {
		Messenger.AddListener (GameEvent.Game_Over, OnGameOver);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.Game_Over, OnGameOver);
	}
}
