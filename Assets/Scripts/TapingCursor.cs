using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapingCursor : MonoBehaviour {
	[SerializeField] private GameObject cursorTap;
	[SerializeField] private GameObject cursorUntap;

	Coroutine active_cor;

	void Start() {
		
	}

	void Update () {
		if (active_cor == null) {
			active_cor = StartCoroutine (Tap ());
		}
	}

	IEnumerator Tap() {
		cursorTap.SetActive (false);
		cursorUntap.SetActive (true);

		yield return new WaitForSeconds(0.515f);

		cursorTap.SetActive (true);
		cursorUntap.SetActive (false);

		yield return new WaitForSeconds (0.1f);

		active_cor = null;
	}
}
