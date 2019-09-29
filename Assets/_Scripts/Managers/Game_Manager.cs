using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Game_Manager : Singleton<Game_Manager> {
	public Level_Controller levelController;

	public event Action<int> addPointsAction;

	private int points;

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

	public void AddPoints(int amount) {
		points += amount;
		points = points < 0 ? 0 : points;

		if(addPointsAction != null) {
			addPointsAction(amount);
		}
	}

	public void ResetPoints() {
		points = 0;
	}

	public int GetPoints() {
		return points;
	}
}
