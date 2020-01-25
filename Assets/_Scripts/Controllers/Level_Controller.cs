using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Controller : MonoBehaviour {
	[BoxTitle("References")]
	public RandomLevel_Manager randomLevel;
	public Camera mainCamera;
	public Player_Controller player;
	public MainMenu_Controller menu;
	public Orders_Manager orders;
	public Speech_Manager speeches;
	public Grill_Manager grill;
	public ObjectPool[] pools;
	[BoxTitle("GameOver")]
	public Image fadeImg;
	public float fadeDuration = 2;

	private void Awake() {
		Game_Manager.Instance.levelController = this;
		randomLevel = FindObjectOfType<RandomLevel_Manager>();
		fadeImg.color = Color.clear;

		randomLevel.Randomize();
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
		player = FindObjectOfType<Player_Controller>();
		menu = FindObjectOfType<MainMenu_Controller>();
		orders = FindObjectOfType<Orders_Manager>();
		speeches = FindObjectOfType<Speech_Manager>();
		grill = FindObjectOfType<Grill_Manager>();

		pools = FindObjectsOfType<ObjectPool>();

		//Disable some references
		player.gameObject.SetActive(false);
	}

	public void GameOver() {
		StartCoroutine(GameOver_Routine());
	}

	private IEnumerator GameOver_Routine() {
		float timePassed = 0;

		while (timePassed < fadeDuration) {
			timePassed += Time.deltaTime;

			fadeImg.color = Color.Lerp(Color.clear, Color.black, timePassed / fadeDuration);

			yield return null;
		}

		SceneManager.LoadScene(1);
	}
}
