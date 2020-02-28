using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour {
	public bool goOnEnable;
	public string sceneName;
	public int sceneBuildIndex;
	public bool useLoadingScreen = true;

	private void OnEnable() {
		if (goOnEnable) {
			if(sceneName != string.Empty) {
				GoToSceneByName(sceneName);
			} else {
				GoToSceneByID(sceneBuildIndex);
			}
		}
	}

	public void GoToSceneByName(string _sceneName) {
		if (useLoadingScreen)
		{
			LoadingScreen_Controller.Instance.ChangeScene(_sceneName);
		}
		else
		{
			SceneManager.LoadScene(_sceneName);
		}

	}

	public void GoToSceneByID(int buildIndex) {
		if (useLoadingScreen)
		{
			LoadingScreen_Controller.Instance.ChangeScene(SceneManager.GetSceneByBuildIndex(buildIndex).name);
		}
		else
		{
			SceneManager.LoadScene(buildIndex);
		}
	}
}
