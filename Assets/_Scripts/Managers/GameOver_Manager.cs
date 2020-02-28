using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.IO;

public class GameOver_Manager : MonoBehaviour {
	public TextMeshProUGUI gameOverText;
	public string sceneToReturn;
	public bool useLocalLeaderboard;
	public int topRankAmount = 10;
	[BoxTitle("Leaderboard")]
	public string baseURL;
	public GameObject leaderboardPanel;
	public TMP_Text topPlayers;
	public TMP_Text playerPoints;
	public GameObject namePanel;
	public TMP_InputField nameInput;
	[BoxTitle("Game Over Texts")]
	[TextArea] public string[] gameOverTexts;

	private bool nameReady;
	private string[] ranks;
	private int[] topPointsList;
	private string[] topNameList;

	private void OnEnable() {
		leaderboardPanel.SetActive(false);
		gameOverText.text = gameOverTexts[UnityEngine.Random.Range(0, gameOverTexts.Length)];
		namePanel.SetActive(false);
		nameReady = false;

		StartCoroutine(ShowLeaderboards());
	}

	private IEnumerator ShowLeaderboards() {
		ranks = null;

		yield return new WaitForSeconds(2);

		playerPoints.text = "Sua pontuação: " + GlobalGame_Manager.Instance.GetPoints();
		topPlayers.text = "Carregando...";

		leaderboardPanel.SetActive(true);

		if (useLocalLeaderboard)
		{
			string filePath = Application.persistentDataPath + "/ranking.txt";
			if (File.Exists(filePath))
			{
				ranks = File.ReadAllLines(filePath);
			}
			else
			{
				File.Create(filePath);
				ranks = new string[0];
			}

			topPointsList = new int[ranks.Length < topRankAmount ? ranks.Length : topRankAmount];
			topNameList = new string[topPointsList.Length];
		}
		else
		{
			UriBuilder uri = new UriBuilder(baseURL + "/getRank.php");
			yield return StartCoroutine(Request_Manager.Instance.GET_Routine(uri, GetLeaderBoard_Callback));
		}

		if(ranks == null) {
			Debug.Log("Falhou");
			topPlayers.text = "Não foi possível acessar o servidor";
			yield break;
		}

		string[] values = new string[] {
			ranks.Length.ToString(),
			topPointsList.Length.ToString()
		};
		Debug.Log(string.Join(";", values));

		topPlayers.text = string.Empty;
		for (int i = 0; i < ranks.Length; i++) {

			if(i < topRankAmount)
			{
				string[] words = ranks[i].Split(',');
				Debug.Log(string.Join(";", words));
				topPlayers.text += string.Format("{0}. {1}\t{2}\n", i + 1, words[0], words[1]);

				topNameList[i] = words[0];
				topPointsList[i] = int.Parse(words[1]);

				yield return new WaitForSeconds(0.5f);
			}
			else
			{
				break;
			}
		}

		//Check if the player is on the topRank
		if (topPointsList.Length == 0 || topPointsList.Length < topRankAmount ||topPointsList.Any(x => GlobalGame_Manager.Instance.GetPoints() > x))
		{
			//Wait for the user to type their name
			namePanel.SetActive(true);
			while (!nameReady)
			{
				yield return null;
			}

			//Resave the file
			if (useLocalLeaderboard)
			{
				List<string> newRank = new List<string>();

				for (int i = 0; i < topNameList.Length; i++)
				{
					newRank.Add(string.Format("{0},{1}", topNameList[i], topPointsList[i]));
				}

				if (newRank.Count == 0)
				{
					newRank.Add(string.Format("{0},{1}", nameInput.text, GlobalGame_Manager.Instance.GetPoints()));
				}
				else
				{
					for (int i = 0; i < newRank.Count; i++)
					{
						string[] words = newRank[i].Split(',');
						if (GlobalGame_Manager.Instance.GetPoints() > int.Parse(words[1]))
						{
							newRank.Insert(i, string.Format("{0},{1}", nameInput.text, GlobalGame_Manager.Instance.GetPoints()));
							break;
						}
					}
				}

				File.WriteAllLines(Application.persistentDataPath + "/ranking.txt", newRank.ToArray());
			}
		}
	}

	public void ResetGame() {
		GlobalGame_Manager.Instance.ResetPoints();
		LoadingScreen_Controller.Instance.ChangeScene(GlobalGame_Manager.Instance.lastModeSelected);
	}

	private void GetLeaderBoard_Callback(string response) {
		Debug.Log(response);

		if (string.IsNullOrEmpty(response) || response.Contains("Erro") || response.Contains("erro")) {
			Debug.LogError("Error getting ranks");
			topPlayers.text = "Não foi possível acessar o servidor";
			return;
		}

		ranks = response.Split('\n');
	}

	public void SubmitName()
	{
		if (!string.IsNullOrEmpty(nameInput.text))
		{
			nameReady = true;
			namePanel.SetActive(false);
		}
	}
}
