using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] LoadSceneEvent_SO eventToListen;
	[SerializeField, Scene] private string[] initialScenes;
	[SerializeField] private GameObject loadingPanel;
	[SerializeField] private Image loadingbar;

	private List<string> activeScenes = new List<string>();

	private void Awake()
	{
		eventToListen.loadEvent += LoadScenes;
	}

	private void Start()
	{
		LoadScenes(true, initialScenes);
	}

	private void OnDestroy()
	{
		eventToListen.loadEvent -= LoadScenes;
	}

	public void LoadScenes(bool showLoadingScreen, params string[] sceneNames)
	{
		StartCoroutine(LoadScenesRoutine(showLoadingScreen, sceneNames));
	}

	private IEnumerator LoadScenesRoutine(bool showLoadingScreen = true, params string[] sceneNames)
	{
		loadingPanel.SetActive(showLoadingScreen);
		float totalProgress = 0;

		//Unload all active Scenes
		if (activeScenes.Count > 0)
		{
			List<AsyncOperation> scenesToUnload = new List<AsyncOperation>();

			for (int i = 0; i < activeScenes.Count; i++)
			{
				scenesToUnload.Add(SceneManager.UnloadSceneAsync(activeScenes[i]));
			}

			//When a scene reaches 0.9 progress, it's already loaded
			while(totalProgress < 0.9f)
			{
				totalProgress = 0;

				for (int i = 0; i < scenesToUnload.Count; i++)
				{
					totalProgress += scenesToUnload[i].progress;
				}

				loadingbar.fillAmount = (totalProgress / scenesToUnload.Count) * 0.5f;

				yield return null;
			}
		}

		activeScenes.Clear();

		//Load the scenes
		totalProgress = 0;

		if (sceneNames.Length> 0)
		{
			List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

			for (int i = 0; i < sceneNames.Length; i++)
			{
				scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive));
				activeScenes.Add(sceneNames[i]);
			}

			//When a scene reaches 0.9 progress, it's already loaded
			while (totalProgress < 0.9f)
			{
				totalProgress = 0;

				for (int i = 0; i < scenesToLoad.Count; i++)
				{
					totalProgress += scenesToLoad[i].progress;
				}

				loadingbar.fillAmount = 0.5f + ((totalProgress / scenesToLoad.Count) * 0.5f);

				yield return null;
			}
		}

		loadingPanel.SetActive(false);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
