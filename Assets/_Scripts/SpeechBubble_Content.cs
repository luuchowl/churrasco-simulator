using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeechBubble_Content : MonoBehaviour {
	public float timeToDisappear = 5;
	public TextMeshProUGUI text;
	public Image image;

	private void OnEnable() {
		Invoke("Disable", timeToDisappear);
	}

	public void Disable() {
		CancelInvoke();
		gameObject.SetActive(false);
	}
}
