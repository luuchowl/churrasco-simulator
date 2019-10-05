using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameOver_Manager : MonoBehaviour {
	public TextMeshProUGUI gameOverText;
	public string sceneToReturn;
	[BoxTitle("Leaderboard")]
	public string baseURL;
	public GameObject leaderboardPanel;
	public TMP_Text topPlayers;
	public TMP_Text playerPoints;
	[BoxTitle("Game Over Texts")]
	[TextArea]
	public string[] gameOverTexts;

	private string[] ranks;

	private void OnEnable() {
		leaderboardPanel.SetActive(false);
		gameOverText.text = gameOverTexts[UnityEngine.Random.Range(0, gameOverTexts.Length)];
		StartCoroutine(ShowLeaderboards());
	}

	private IEnumerator ShowLeaderboards() {
		ranks = null;

		yield return new WaitForSeconds(2);

		playerPoints.text = "Sua pontuação: " + Game_Manager.Instance.GetPoints();
		topPlayers.text = "Carregando...";

		leaderboardPanel.SetActive(true);

		UriBuilder uri = new UriBuilder(baseURL + "/getRank.php");
		yield return StartCoroutine(Request_Manager.Instance.GET_Routine(uri, getLeaderBoard_Callback));

		if(ranks == null) {
			yield break;
		}

		topPlayers.text = string.Empty;
		for (int i = 0; i < ranks.Length; i++) {
			yield return new WaitForSeconds(0.5f);
			topPlayers.text += ranks[i] + "\n";
		}
	}

	public void ResetGame() {
		Game_Manager.Instance.ResetPoints();
		SceneManager.LoadScene(sceneToReturn);
	}

	private void getLeaderBoard_Callback(string response) {
		Debug.Log(response);

		if (string.IsNullOrEmpty(response)) {
			Debug.LogError("Error getting ranks");
			topPlayers.text = "Não foi possível acessar o servidor";
			return;
		}

		ranks = response.Split('\n');
	}
}
