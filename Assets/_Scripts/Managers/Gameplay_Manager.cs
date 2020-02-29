using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Gameplay_Manager : Singleton<Gameplay_Manager> {
	[BoxTitle("Main References")]
	public LevelBuilder randomLevel;
	public Camera mainCamera;
	public Player_Controller player;
	public Orders_Manager orders;
	public Speech_Manager speeches;
	public Grill_Manager grill;
	public ObjectPool[] pools;
	[BoxTitle("Pause")]
	public LerpPosition phone;
	public GameObject pauseMenu;
	public Transform phoneRest;
	public Transform phoneView;
	public UnityEvent gameStartEvent = new UnityEvent();

	private bool paused;
	private bool canPause;

	protected override void Awake()
	{
		base.Awake();

		pauseMenu.SetActive(false);
		canPause = true;
		paused = false;

		Initialize();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && canPause)
		{
			PauseGame(!paused);
		}

		//Debug
		if (Input.GetKeyDown(KeyCode.P))
		{
			GlobalGame_Manager.Instance.AddPoints(5);
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			 orders.AddWrong();
		}
	}

	public void Initialize()
	{
		StopAllCoroutines();
		StartCoroutine(Initialize_Routine());
	}

	private IEnumerator Initialize_Routine() {
		//Randomize the level props
		randomLevel = FindObjectOfType<LevelBuilder>();
		randomLevel.Randomize();

		//Find all main references
		mainCamera = Camera.main;
		player = FindObjectOfType<Player_Controller>();
		orders = FindObjectOfType<Orders_Manager>();
		speeches = FindObjectOfType<Speech_Manager>();
		grill = FindObjectOfType<Grill_Manager>();
		pools = FindObjectsOfType<ObjectPool>();

		//Wait a frame so all objects can have theyr references set and then start the game Start the game
		yield return null;
		gameStartEvent?.Invoke();

		//Game_Manager.Instance.levelController.speeches.StartSpeeches();
		Sound_Manager.Instance.PlayRandomSFX(true, Sound_Manager.Instance.audioHolder.burningCoal.simple);
	}

	public void GameOver() {
		LoadingScreen_Controller.Instance.FadeIn();
		LoadingScreen_Controller.Instance.fadeInEnded.AddListener(() =>
		{
			SceneManager.LoadSceneAsync("GameOver");
		});
	}

	public void PauseGame(bool pause)
	{
		if (!pause)
		{
			Time.timeScale = 1;
			pauseMenu.SetActive(false);
		}
		else
		{
			phone.finishedTransitionEvent.AddListener(() =>
			{
				pauseMenu.SetActive(true);
				Time.timeScale = 0;
			});
		}

		phone.finishedTransitionEvent.AddListener(() => {
			canPause = true;
			phone.finishedTransitionEvent.RemoveAllListeners();
		});

		phone.SetPos(pause ? phoneView : phoneRest);
		canPause = false;
		paused = pause;
	}
}
