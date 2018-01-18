using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	[SerializeField] private Text scoreText;
	[SerializeField] private Text highestScore;
	[SerializeField] private CanvasRenderer[] UI;
	[SerializeField] private GUIController guic;

	private int score;
	private bool isGameOver;

	public void Restart() {
		score = 0;
	}

	void OnTubePassed() {
		score++;
		scoreText.text = score.ToString ();
	}

	void Start () {
		score = 0;
		isGameOver = true;
	}

	void Update () {
		
	}

	public void HideScoreAndOpenMenu() {
		StartCoroutine (HideShowHighestScore (true));
	}

	void OnGameOver() {
		if (!isGameOver) {
			isGameOver = true;

			if (PlayerPrefs.GetInt ("HighestScore", 0) < score)
				PlayerPrefs.SetInt ("HighestScore", score);

			highestScore.text = PlayerPrefs.GetInt ("HighestScore", 0).ToString ();

			StartCoroutine (HideShowHighestScore (false));
		}
	}

	IEnumerator HideShowHighestScore(bool needHide) {
		int n = 20;

		if (needHide) {
			for (int j = 1; j <= n; j++) {
				for (int i = 0; i < UI.Length; i++) {
					UI [i].SetAlpha (Mathf.Lerp (1, 0, (float)j / n));
				}

				yield return new WaitForSeconds (0.02f);
			}

			for (int i = 0; i < UI.Length; i++)
				UI [i].gameObject.SetActive (false);

			guic.ShowUI ();
		} else {
			for (int i = 0; i < UI.Length; i++)
				UI [i].gameObject.SetActive (true);

			for (int j = 20; j >= 1; j--) {
				for (int i = 0; i < UI.Length; i++) {
					UI [i].SetAlpha (Mathf.Lerp (1, 0, (float)j / n));
				}

				yield return new WaitForSeconds (0.02f);
			}

		}
	}

	void StartGame() {
		score = 0;
		scoreText.text = score.ToString ();

		isGameOver = false;
	}

	void Awake() {
		Messenger.AddListener (GameEvent.Tube_Passed, OnTubePassed);
		Messenger.AddListener (GameEvent.Game_Over, OnGameOver);
		Messenger.AddListener (GameEvent.StartGame, StartGame);
	}

	void OnDestroy() {
		Messenger.RemoveListener (GameEvent.Tube_Passed, OnTubePassed);
		Messenger.RemoveListener (GameEvent.Game_Over, OnGameOver);
		Messenger.RemoveListener (GameEvent.StartGame, StartGame);
	}
}