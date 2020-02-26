using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingScreen_Controller : Singleton<LoadingScreen_Controller> {

	public Image fadeImage;
	public float fadeDuration;
	public UnityEvent fadeInEnded = new UnityEvent();
	public UnityEvent fadeOutEnded = new UnityEvent();

	private Color imgColor;

	private void Start()
	{
		imgColor = fadeImage.color;
		fadeImage.gameObject.SetActive(false);
	}

	public void FadeIn()
	{
		StartCoroutine(Fade_Routine(0, 1, fadeInEnded));
	}

	public void FadeOut()
	{
		fadeOutEnded.AddListener(() => { fadeImage.gameObject.SetActive(false); });
		StartCoroutine(Fade_Routine(1, 0, fadeOutEnded));
	}

	private IEnumerator Fade_Routine(float startAlpha, float endAlpha, UnityEvent finishedEvent)
	{
		fadeImage.gameObject.SetActive(true);
		float timePassed = 0;
		
		while(timePassed < fadeDuration)
		{
			timePassed += Time.deltaTime;

			imgColor.a = Mathf.Lerp(startAlpha, endAlpha, timePassed / fadeDuration);
			fadeImage.color = imgColor;

			yield return null;
		}

		imgColor.a = endAlpha;
		fadeImage.color = imgColor;

		finishedEvent.Invoke();
		finishedEvent.RemoveAllListeners();
	}
}
