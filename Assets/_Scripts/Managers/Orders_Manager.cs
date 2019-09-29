using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Orders_Manager : MonoBehaviour {
	[BoxTitle("References")]
	public RectTransform orderParent;
	public ObjectPool orderPool;
	public ObjectPool ingredientPool;
	public Ingredients_Database ingredientsDatabase;
	public TextMeshProUGUI pointsText;
	public Image fadeImg;
	public float fadeDuration = 2;
	public GameObject[] wrongSymbols;
	[BoxTitle("Properties")]
	public int maxOrders = 5;
	public float startDelay = 1;
	public float orderDelay = 1;
	public float randomizeDelay = 1;
	public Vector2Int minMaxNumberOfIngredients = new Vector2Int(1, 3);
	public int mistakesPermitted = 3;
	[BoxTitle("Misc")]
	public List<Order_Content> currentOrders = new List<Order_Content>();
	public UnityEvent startGame = new UnityEvent();
	
	private UI_TransformAnim pointsAnim;
	private int wrongs;
	private Player_Controller player;

	private void OnEnable() {
		Game_Manager.Instance.addPointsAction += AddPoints;
	}

	private void OnDisable() {
		Game_Manager.Instance.addPointsAction -= AddPoints;
	}

	private void Start() {
		pointsText.text = "" + Game_Manager.Instance.GetPoints();
		pointsAnim = pointsText.GetComponent<UI_TransformAnim>();
		
		fadeImg.color = Color.clear;
		pointsText.gameObject.SetActive(false);

		for (int i = 0; i < wrongSymbols.Length; i++) {
			wrongSymbols[i].SetActive(false);
		}
	}

	[ContextMenu("Play game")]
	public void Iniciar() {
		startGame.Invoke();

		StartCoroutine("Order_Routine");
		StartCoroutine("RandomCanSound_Routine");
		pointsText.gameObject.SetActive(true);

		player = FindObjectOfType<Player_Controller>();
	}

	private IEnumerator Order_Routine() {
		yield return new WaitForSeconds(startDelay);

		while (true) {
			if (currentOrders.Count < maxOrders) {
				MakeOrder();
			}

			yield return new WaitForSeconds(orderDelay + Random.Range(0, randomizeDelay));
		}
	}

	public void MakeOrder() {
		Order_Content order = orderPool.GetPooledObject<Order_Content>(orderParent);

		int childCount = order.ingredientsHolder.childCount;
		for (int i = 0; i < childCount; i++) {
			ingredientPool.ReturnObjectToPool(order.ingredientsHolder.GetChild(0).gameObject);
		}

		Order o = new Order();

		//TODO change from random?
		int ingredientsAmount = Random.Range(minMaxNumberOfIngredients.x, minMaxNumberOfIngredients.y + 1);
		
		for (int i = 0; i < ingredientsAmount; i++) {
			Image ingredient = ingredientPool.GetPooledObject<Image>(order.ingredientsHolder);
			int id = Random.Range(0, ingredientsDatabase.ingredientsSprites.Length);
			ingredient.sprite = ingredientsDatabase.ingredientsSprites[id];
			o.ingredients.Add(ingredient.sprite);
		}

		order.order = o;

		currentOrders.Add(order);

		LayoutRebuilder.MarkLayoutForRebuild(orderParent);
	}

	public bool TakeOrder(Order o) {
		if (CheckIfOrderIsRight(o)) {
			return true;
		} else {
			if(currentOrders.Count > 0) { //Remove the first order if it's wrong
				currentOrders[0].gameObject.SetActive(false);
				currentOrders.Remove(currentOrders[0]);
			}
		}

		return false;
	}

	private void AddPoints(int amount) {
		pointsText.text = "" + Game_Manager.Instance.GetPoints();
		pointsAnim.PlayAnim();
	}

	private bool CheckIfOrderIsRight(Order order) {
		for (int i = 0; i < currentOrders.Count; i++) {
			int points = 0;

			//Check if the lenght of the ingredients are the same
			if (order.ingredients.Count != currentOrders[i].order.ingredients.Count) {
				continue;
			}
			
			//If it is, then check if the ingredients are the same
			for (int j = 0; j < order.ingredients.Count; j++) {
				//Comment this and uncomment the "if" below if you want the order of the ingredients to matter
				if (currentOrders[i].order.ingredients.Contains(order.ingredients[j])) {
					points++;
				}
				
				//if (order.ingredients[j] == currentOrders[i].order.ingredients[j]) {
				//	points++; //We add a point for each correct ingredient
				//}
			}

			//Now we check the points, if it's the same amount as the ingredients, then the recipe is correct
			if (points == currentOrders[i].order.ingredients.Count) {
				currentOrders[i].gameObject.SetActive(false);
				currentOrders.Remove(currentOrders[i]);
				return true;
			}
		}
		
		return false;
	}

	public void AddWrong() {
		wrongs++;
		
		if(wrongs >= mistakesPermitted) {
			player.acting = true;
			StartCoroutine("GameOver_Routine");
		}

		for (int i = 0; i < wrongs; i++) {
			wrongSymbols[i].SetActive(true);
		}
	}

	private IEnumerator GameOver_Routine() {
		float timePassed = 0;
		
		while(timePassed < fadeDuration) {
			timePassed += Time.deltaTime;

			fadeImg.color = Color.Lerp(Color.clear, Color.black, timePassed / fadeDuration);

			yield return null;
		}

		SceneManager.LoadScene(1);
	}

	private IEnumerator RandomCanSound_Routine() {
		while (true) {
			yield return new WaitForSeconds(Random.Range(5f, 15f));
			Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.openCan.simple);
		}
	}
}
