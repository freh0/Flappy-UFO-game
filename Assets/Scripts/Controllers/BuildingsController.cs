using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsController : MonoBehaviour {
	[SerializeField] private GameObject tubePrefabR;

	List<GameObject> listU;
	List<GameObject> listD;
	List<bool> isScored;

	public float speed = 3f;
	public float startgameX = 12f;
	public float distbetweenX = 4f;
	public float distbetweenY = 3.4f;

	private float playAreaSizeY = 9.25f;
	private float minsizeY = 1.3f;
	private float maxsizeY = 5.5f;
	private float sizeX = 1;

	private float startYdown = 0.8f;
	private float startYup = 10.05f;

	private bool isGameOver;

	void SetUpDownTubes() {
		listD = new List<GameObject> ();
		listU = new List<GameObject> ();
		isScored = new List<bool> ();

		for (int i = 0; i < 4; i++) {
			isScored.Add (false);

			listD.Add (Instantiate (tubePrefabR));
			listU.Add (Instantiate (tubePrefabR));

			float sizeYdown = Random.Range (minsizeY, maxsizeY);
			float sizeYup = playAreaSizeY - (sizeYdown + distbetweenY);

			int lastU = listU.Count - 1;
			int lastD = listD.Count - 1;

			listU [lastU].transform.Rotate (0, 0, 180);

			listD [lastD].GetComponent<SpriteRenderer> ().size = new Vector2 (sizeX, sizeYdown);
			listU [lastU].GetComponent<SpriteRenderer> ().size = new Vector2 (sizeX, sizeYup);

			listD [lastD].transform.position = new Vector3 (startgameX + i * distbetweenX, startYdown, 10f);
			listU [lastU].transform.position = new Vector3 (startgameX + sizeX + i * distbetweenX, startYup, 10f);
		}
	}

	void Start () {
		Time.timeScale = 1.25f;

		isGameOver = true;

		SetUpDownTubes ();
	}

	void Update () {
		if (!isGameOver) {
			for (int i = 0; i < listD.Count; i++) {
				if (listD [i].transform.position.x < 0.7 && !isScored[i]) {
					isScored [i] = true;

					Messenger.Broadcast (GameEvent.Tube_Passed);
				}

				if (listD [i].transform.position.x < 0 - 2 * sizeX) {
					Vector3 pos = listD [i].transform.position;
					pos.x = startgameX;
					listD [i].transform.position = pos;

					pos = listU [i].transform.position;
					pos.x = startgameX + sizeX;
					listU [i].transform.position = pos;

					float sizeYdown = Random.Range (minsizeY, maxsizeY);
					float sizeYup = playAreaSizeY - (sizeYdown + distbetweenY);

					listD [i].GetComponent<SpriteRenderer> ().size = new Vector2 (sizeX, sizeYdown);
					listU [i].GetComponent<SpriteRenderer> ().size = new Vector2 (sizeX, sizeYup);

					isScored [i] = false;
				}
			}

			for (int i = 0; i < listD.Count; i++) {
				listD [i].transform.position = listD [i].transform.position - new Vector3 (speed * Time.deltaTime, 0);
			}

			for (int i = 0; i < listU.Count; i++) {
				listU [i].transform.position = listU [i].transform.position - new Vector3 (speed * Time.deltaTime, 0);
			}
		}
	}

	void OnGameOver() {
		isGameOver = true;
	}

	void ClearLists() {
		for (int i = listU.Count - 1; i >= 0; i--) {
			Destroy (listU [i]);
			listU.RemoveAt (i);

			Destroy (listD [i]);
			listD.RemoveAt (i);
		}
	}

	void StartGame() {
		isGameOver = false;

		ClearLists ();

		SetUpDownTubes ();
	}

	void Awake() {
		Messenger.AddListener (GameEvent.StartGame, StartGame);
		Messenger.AddListener (GameEvent.Game_Over, OnGameOver);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.StartGame, StartGame);
		Messenger.RemoveListener (GameEvent.Game_Over, OnGameOver);
	}
}