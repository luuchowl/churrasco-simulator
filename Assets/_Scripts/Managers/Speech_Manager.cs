using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speech_Manager : MonoBehaviour{
	[BoxTitle("References")]
	public RectTransform speechParent;
	public ObjectPool speechPool;
	public Speech_Database speechDatabase;
	[BoxTitle("Properties")]
	public float startDelay = 1;
	public float orderDelay = 1;
	public float randomizeDelay = 1;
	public float onScreenDelay = 1;

	private bool currentlySpeaking;
	private int lastSpeech;
	private ContentSizeFitter fitter;

	public void StartSpeeches() {
		fitter = speechParent.GetComponent<ContentSizeFitter>();

		StartCoroutine("ShowSpeech_Routine");
	}

	private IEnumerator ShowSpeech_Routine() {
		yield return new WaitForSeconds(startDelay);

		while (true) {
			if (!currentlySpeaking) yield return StartCoroutine("ChooseRandomSpeech");

			yield return new WaitForSeconds(orderDelay + Random.Range(0, randomizeDelay));
		}
	}

	public IEnumerator ChooseRandomSpeech() {
		currentlySpeaking = true;

		int id = 0;
		do {
			id = Random.Range(0, speechDatabase.conversas.Length);
		} while (id == lastSpeech);

		lastSpeech = id;
		Conversation c = speechDatabase.conversas[id];

		for (int i = 0; i < c.falas.Length; i++) {
			CreateSpeech(c.falas[i].texto, c.falas[i].emoji);

			yield return new WaitForSeconds(speechDatabase.delayOnSequentialMessages);
		}

		currentlySpeaking = false;
	}

	public void CreateSpeech(string text, Sprite emoji) {
		SpeechBubble_Content bubble = speechPool.GetPooledObject<SpeechBubble_Content>(speechParent);
		bubble.ownerPool = speechPool;

		bubble.text.gameObject.SetActive(text != "");
		bubble.image.gameObject.SetActive(emoji != null);

		bubble.text.text = text;
		bubble.image.sprite = emoji;

		bubble.transform.SetAsLastSibling();

		StartCoroutine(WaitAndHideMessage_Routine(bubble));
	}

	private IEnumerator WaitAndHideMessage_Routine(SpeechBubble_Content bubble) {
		fitter.enabled = false;
		yield return null;
		fitter.enabled = true;

		bubble.Play(onScreenDelay);
	}
}
