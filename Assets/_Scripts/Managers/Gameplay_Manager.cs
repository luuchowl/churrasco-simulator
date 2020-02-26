using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameplay_Manager : Singleton<Gameplay_Manager> {
	[BoxTitle("References")]
	public RandomLevel_Manager randomLevel;
	public Camera mainCamera;
	public Player_Controller player;
	public Orders_Manager orders;
	public Speech_Manager speeches;
	public Grill_Manager grill;
	public ObjectPool[] pools;

	protected override void Awake()
	{
		base.Awake();
		Game_Manager.Instance.ganeplayManager = this;
		GetReferences();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Game_Manager.Instance.AddPoints(5);
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			 orders.AddWrong();
		}
	}

	public void GetReferences() {
		mainCamera = Camera.main;
		randomLevel = FindObjectOfType<RandomLevel_Manager>();
		player = FindObjectOfType<Player_Controller>();
		orders = FindObjectOfType<Orders_Manager>();
		speeches = FindObjectOfType<Speech_Manager>();
		grill = FindObjectOfType<Grill_Manager>();

		pools = FindObjectsOfType<ObjectPool>();
		randomLevel.Randomize();

		//Disable some references
		player.gameObject.SetActive(false);
	}

	public void GameOver() {
		LoadingScreen_Controller.Instance.FadeIn();
		LoadingScreen_Controller.Instance.fadeInEnded.AddListener(() =>
		{
			SceneManager.LoadScene("Game Over");
		});
	}
}
