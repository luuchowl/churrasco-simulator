using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenu_Controller : MonoBehaviour {
	[BoxTitle("Title Screen")]
	public GameObject showMenuButton;
	public GameObject pressStart;
	[BoxTitle("Menu Screen")]
	public GameObject menuPanel;
	public Slider soundSlider;
	public Slider musicSlider;
	public Slider sfxSlider;
	public AudioMixer mixer;
	[BoxTitle("Game Start")]
	public UnityEvent gameStarted = new UnityEvent();

	private LerpPosition camPos;

	private void Start() {
		camPos = Game_Manager.Instance.levelController.mainCamera.GetComponent<LerpPosition>();
		ShowTitleScreen();

		menuPanel.gameObject.SetActive(false);

		Sound_Manager.Instance.SetMusic(Sound_Manager.Instance.audioHolder.music.simple[0]);
		Sound_Manager.Instance.PlaySingle(Sound_Manager.Instance.audioHolder.ambient.simple[0], true);

		float value = 1;

		mixer.GetFloat("MasterVolume", out value);
		soundSlider.value = Mathf.InverseLerp(-80, 20, value);

		mixer.GetFloat("MusicVolume", out value);
		musicSlider.value = Mathf.InverseLerp(-80, 20, value);

		mixer.GetFloat("SFXVolume", out value);
		sfxSlider.value = Mathf.InverseLerp(-80, 20, value);
	}

	public void ShowTitleScreen() {
		camPos.SetCameraPos(0);

		pressStart.SetActive(true);
		showMenuButton.SetActive(true);
		menuPanel.gameObject.SetActive(false);
	}

	public void ShowMenu() {
		camPos.SetCameraPos(1);

		pressStart.SetActive(false);
		showMenuButton.SetActive(false);
		menuPanel.gameObject.SetActive(true);
	}

	public void StartGame() {
		Game_Manager.Instance.levelController.orders.Iniciar();
		Game_Manager.Instance.levelController.speeches.StartSpeeches();
		Sound_Manager.Instance.PlayRandomSFX(true, Sound_Manager.Instance.audioHolder.burningCoal.simple);

		gameStarted.Invoke();

		menuPanel.gameObject.SetActive(false);
	}

	public void ShowCredits() {
		menuPanel.gameObject.SetActive(false);
	}

	public void ShowTutorial() {
		menuPanel.gameObject.SetActive(false);
	}

	public void Mute(bool muteSound) {
		if (muteSound) {
			Sound_Manager.Instance.Mute();
		} else {
			Sound_Manager.Instance.Unmute();
		}
	}

	public void SetVolume(float volume) {
		Sound_Manager.Instance.SetGlobalVolume(volume);
	}

	public void SetMusicVolume(float volume) {
		Sound_Manager.Instance.SetMusicVolume(volume);
	}

	public void SetSFXVolume(float volume) {
		Sound_Manager.Instance.SetSfxVolume(volume);
	}
}
