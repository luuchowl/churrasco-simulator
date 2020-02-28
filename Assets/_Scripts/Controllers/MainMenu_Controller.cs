using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu_Controller : MonoBehaviour {
	public LerpPosition camPos;
	public AudioMixer mixer;
	public GameObject backButton;
	[BoxTitle("Title Screen")]
	public Transform titleViewPosition;
	public GameObject showMenuButton;
	public GameObject pressStart;
	[BoxTitle("Menu Screen")]
	public Transform menuViewPosition;
	public GameObject menuPanel;
	[BoxTitle("MenuOptions")]
	public Slider soundSlider;
	public Slider musicSlider;
	public Slider sfxSlider;
	public Transform gameModesPosition;
	public GameObject gameModesPanel;
	[BoxTitle("Tutorial")]
	public Transform tutorialViewPosition;
	public VideoPlayer videoPlayer;
	public LerpPosition tutorialPhone;
	public Transform showPhonePosition;
	public Transform restPhonePosition;
	[BoxTitle("Credits")]
	public Transform creditsViewPosition;

	private void Start() {
		ShowTitleScreen();

		menuPanel.gameObject.SetActive(false);
		gameModesPanel.gameObject.SetActive(false);

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
		camPos.SetPos(titleViewPosition);

		pressStart.SetActive(true);
		showMenuButton.SetActive(true);
		menuPanel.gameObject.SetActive(false);
		backButton.SetActive(false);
	}

	public void ShowMenu() {
		camPos.SetPos(menuViewPosition);
		tutorialPhone.SetPos(restPhonePosition);

		pressStart.SetActive(false);
		showMenuButton.SetActive(false);
		menuPanel.gameObject.SetActive(true);
		gameModesPanel.gameObject.SetActive(false);

		backButton.SetActive(false);
		videoPlayer.Stop();
	}

	public void ShowGameModes()
	{
		camPos.SetPos(gameModesPosition);

		gameModesPanel.gameObject.SetActive(true);
		menuPanel.gameObject.SetActive(false);

		backButton.SetActive(true);
	}

	public void ShowCredits() {
		camPos.SetPos(creditsViewPosition);

		menuPanel.gameObject.SetActive(false);
		backButton.SetActive(true);
	}

	public void ShowTutorial() {
		camPos.SetPos(tutorialViewPosition);

		backButton.SetActive(true);
		menuPanel.gameObject.SetActive(false);
		tutorialPhone.SetPos(showPhonePosition);
		videoPlayer.Play();
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

	public void StartQuickPlay()
	{
		string mode = "QuickPlay";
		GlobalGame_Manager.Instance.lastModeSelected = mode;
		LoadingScreen_Controller.Instance.ChangeScene(mode);
	}
}
