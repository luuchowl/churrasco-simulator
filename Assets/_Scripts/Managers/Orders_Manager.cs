using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Orders_Manager : MonoBehaviour
{
	[BoxTitle("References")]
	public RectTransform orderParent;
	public ObjectPool orderPool;
	public ObjectPool ingredientPool;
	public Ingredients_Database ingredientsDatabase;
	public TextMeshProUGUI pointsText;

	public GameObject[] wrongSymbols;
	[BoxTitle("Properties")]
	public int maxOrders = 5;
	public float startDelay = 1;
	public float orderDelay = 1;
	public float randomizeDelay = 1;
	public float difficultyStep = 1;
	public float difficultyStepTime = 1;
	public Vector2Int minMaxNumberOfIngredients = new Vector2Int(1, 3);
	public int mistakesPermitted = 3;
	public Color[] postItColors;
	[BoxTitle("Misc")]
	public List<Order_Content> currentOrders = new List<Order_Content>();

	private Animator pointsAnim;
	private int wrongs;
	private Player_Controller player;
	private int lastColorID;

	private void Awake()
	{
		Gameplay_Manager.Instance.gameStartEvent.AddListener(Initialize);
	}

	private void OnEnable()
	{
		GlobalGame_Manager.Instance.addPointsAction += AddPoints;
	}

	private void OnDisable()
	{
		GlobalGame_Manager.Instance.addPointsAction -= AddPoints;
	}

	private void Start()
	{
		pointsText.text = $"{GlobalGame_Manager.Instance.GetPoints()}";
		pointsAnim = pointsText.GetComponent<Animator>();

		pointsText.gameObject.SetActive(false);

		for (int i = 0; i < wrongSymbols.Length; i++)
		{
			wrongSymbols[i].SetActive(false);
		}
	}

	public void Initialize()
	{
		StartCoroutine("Order_Routine");
		pointsText.gameObject.SetActive(true);

		player = Gameplay_Manager.Instance.player;
	}

	private IEnumerator Order_Routine()
	{
		yield return new WaitForSeconds(startDelay);
		int stepCounter = 0;
		float lastStep = Time.time;


		while (true)
		{
			if (currentOrders.Count < maxOrders)
			{
				MakeOrder();
			}

			if (lastStep - Time.time > difficultyStepTime)
			{
				lastStep = Time.time;
				stepCounter++;
			}

			yield return new WaitForSeconds(orderDelay + Random.Range(0, randomizeDelay) - (difficultyStep * stepCounter));
		}
	}

	public void MakeOrder()
	{
		Order_Content order = orderPool.GetPooledObject<Order_Content>(orderParent);

		int colorId = 0;

		do
		{
			colorId = Random.Range(0, postItColors.Length);
		} while (colorId == lastColorID);

		order.postIt.color = postItColors[colorId];

		int childCount = order.ingredientsHolder.childCount;
		for (int i = 0; i < childCount; i++)
		{
			ingredientPool.ReturnObjectToPool(order.ingredientsHolder.GetChild(0).gameObject);
		}

		Order o = new Order();

		//TODO change from random?
		int ingredientsAmount = Random.Range(minMaxNumberOfIngredients.x, minMaxNumberOfIngredients.y + 1);

		for (int i = 0; i < ingredientsAmount; i++)
		{
			Image ingredient = ingredientPool.GetPooledObject<Image>(order.ingredientsHolder);
			int id = Random.Range(0, ingredientsDatabase.ingredientsSprites.Length);
			ingredient.sprite = ingredientsDatabase.ingredientsSprites[id];
			o.ingredients.Add(ingredient.sprite);
		}

		order.order = o;

		currentOrders.Add(order);

		LayoutRebuilder.MarkLayoutForRebuild(orderParent);
	}

	public bool TakeOrder(Order o)
	{
		if (CheckIfOrderIsRight(o))
		{
			return true;
		}
		else
		{
			if (currentOrders.Count > 0)
			{ //Remove the first order if it's wrong
				currentOrders[0].gameObject.SetActive(false);
				currentOrders.Remove(currentOrders[0]);
			}
		}

		return false;
	}

	private void AddPoints(int amount)
	{
		pointsText.text = "" + GlobalGame_Manager.Instance.GetPoints();
		pointsAnim.SetTrigger("PointAdded");
	}

	private bool CheckIfOrderIsRight(Order order)
	{
		for (int i = 0; i < currentOrders.Count; i++)
		{
			int points = 0;

			//Check if the lenght of the ingredients are the same
			//Debug.Log(order == null);
			//Debug.Log(order.ingredients == null);
			//Debug.Log(currentOrders == null);
			//Debug.Log(currentOrders[i] == null);
			//Debug.Log(currentOrders[i].order == null);
			//Debug.Log(currentOrders[i].order.ingredients == null);
			if (order.ingredients.Count != currentOrders[i].order.ingredients.Count)
			{
				continue;
			}

			//If it is, then check if the ingredients are the same
			for (int j = 0; j < order.ingredients.Count; j++)
			{
				//Comment this and uncomment the "if" below if you want the order of the ingredients to matter
				if (currentOrders[i].order.ingredients.Contains(order.ingredients[j]))
				{
					points++;
				}

				//if (order.ingredients[j] == currentOrders[i].order.ingredients[j]) {
				//	points++; //We add a point for each correct ingredient
				//}
			}

			//Now we check the points, if it's the same amount as the ingredients, then the recipe is correct
			if (points == currentOrders[i].order.ingredients.Count)
			{
				currentOrders[i].gameObject.SetActive(false);
				currentOrders.Remove(currentOrders[i]);
				return true;
			}
		}

		return false;
	}

	public void AddWrong()
	{
		wrongs++;

		if (wrongs >= mistakesPermitted)
		{
			player.acting = true;
			Gameplay_Manager.Instance.GameOver();
		}

		for (int i = 0; i < wrongs; i++)
		{
			wrongSymbols[i].SetActive(true);
		}
	}
}
