using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver_Manager : MonoBehaviour {
	public TextMeshProUGUI gameOverText;
	public string sceneToReturn;
	public GameObject leaderboardPanel;
	public TMP_Text topPlayers;
	public TMP_Text playerPoints;
	[BoxTitle("Game Over Texts")]
	[TextArea]
	public string[] gameOverTexts;

	private void OnEnable() {
		leaderboardPanel.SetActive(false);
		gameOverText.text = gameOverTexts[Random.Range(0, gameOverTexts.Length)];
		StartCoroutine(ShowLeaderboards());
	}

	private IEnumerator ShowLeaderboards() {
		yield return new WaitForSeconds(2);

		leaderboardPanel.SetActive(true);

		//TODO Fazer requests

		topPlayers.text = "Não foi possível acessar o servidor";
		playerPoints.text = "Sua pontuação: " + Game_Manager.Instance.GetPoints();
	}

	public void ResetGame() {
		Game_Manager.Instance.ResetPoints();
		SceneManager.LoadScene(sceneToReturn);
	}
}
