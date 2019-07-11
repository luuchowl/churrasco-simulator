using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : Singleton<Game_Manager> {
	public Level_Controller levelController;

	// Use this for initialization
	void Start () {
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	protected override void OnDestroy() {
		base.OnDestroy();

		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		
	}

	private void OnSceneUnloaded(Scene scene) {
		Sound_Manager.Instance.StopAll();
		levelController = null;
	}
}
