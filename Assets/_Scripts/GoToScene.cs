using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour {
	public bool goOnEnable;
	public string sceneName;
	public int sceneBuildIndex;

	private void OnEnable() {
		if (goOnEnable) {
			if(sceneName != string.Empty) {
				SceneManager.LoadScene(sceneName);
			} else {
				SceneManager.LoadScene(sceneBuildIndex);
			}
		}
	}

	public void GoToSceneByName(string _sceneName) {
		SceneManager.LoadScene(_sceneName);
	}

	public void GoToSceneByID(int buildIndex) {
		SceneManager.LoadScene(buildIndex);
	}
}
