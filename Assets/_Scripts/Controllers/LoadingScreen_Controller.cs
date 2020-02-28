using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen_Controller : Singleton<LoadingScreen_Controller> {

	public float fadeDuration;
	public Image fadeImage;
	public GameObject loadingIcon;
	public UnityEvent fadeInEnded = new UnityEvent();
	public UnityEvent fadeOutEnded = new UnityEvent();

	private Color imgColor;

	private void Start()
	{
		imgColor = fadeImage.color;
		fadeImage.gameObject.SetActive(false);
		loadingIcon?.SetActive(false);
	}

	public void FadeIn()
	{
		fadeInEnded.AddListener(() => {
			loadingIcon?.SetActive(true);
		});

		StartCoroutine(Fade_Routine(0, 1, fadeInEnded));
	}

	public void FadeOut()
	{
		fadeOutEnded.AddListener(() => { 
			fadeImage.gameObject.SetActive(false); 
		});

		loadingIcon?.SetActive(false);
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

	public void ChangeScene(string sceneName)
	{
		FadeIn();
		fadeInEnded.AddListener(() =>
		{
			SceneManager.LoadSceneAsync(sceneName);
		});
	}
}
