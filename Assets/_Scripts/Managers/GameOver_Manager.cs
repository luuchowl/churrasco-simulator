using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver_Manager : MonoBehaviour {
	public TextMeshProUGUI gameOverText;
	public string sceneToReturn;
	[BoxTitle("Game Over Texts")]
	[TextArea]
	public string[] gameOverTexts;

	private void OnEnable() {
		gameOverText.text = gameOverTexts[Random.Range(0, gameOverTexts.Length)];
	}

	public void ResetGame() {
		SceneManager.LoadScene(sceneToReturn);
	}
}
