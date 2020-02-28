using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour {
	[BoxTitle("Preferences")]
	public int correctPoints = 10;
	public int wrongPoints = -20;

	private void OnCollisionEnter(Collision other) {
		//Stick with multipple ingredients
		if (other.gameObject.CompareTag("Stick")) {
			Stick_Content stick = other.gameObject.GetComponent<Stick_Content>();

			if (CheckIfRightCombination(stick)) {
				Debug.Log("Correct!");
				GlobalGame_Manager.Instance.AddPoints(correctPoints);
			} else {
				Debug.Log("Incorrect...");
				GlobalGame_Manager.Instance.AddPoints(wrongPoints);
				Gameplay_Manager.Instance.orders.AddWrong();
			}

			//Clear the ingredients
			stick.ReturnFoodsToPool();

			//Return the Stick
			ObjectPool[] pools = Gameplay_Manager.Instance.pools;
			for (int i = 0; i < pools.Length; i++) {
				if (pools[i].ReturnObjectToPool(other.gameObject)) {
					break;
				}
			}

			//Clear the order
			stick.currentOrder.order.ingredients.Clear();
		}
		else if (other.gameObject.CompareTag("Ingredient")) //Single Ingredient
		{
			Order o = new Order();
			Food f = other.collider.attachedRigidbody.GetComponent<Food>();
			o.ingredients.Add(f.ingredientImage);

			bool correct = false;

			if (Gameplay_Manager.Instance.orders.TakeOrder(o))
			{
				if (f.currentStage == Food.stage.WellDone)
				{
					Debug.Log("Correct!");
					GlobalGame_Manager.Instance.AddPoints(correctPoints);
					correct = true;
				}
			}

			if (!correct)
			{
				Debug.Log("Incorrect...");
				GlobalGame_Manager.Instance.AddPoints(wrongPoints);
				Gameplay_Manager.Instance.orders.AddWrong();
			}

			//Return the Ingredient
			ObjectPool[] pools = Gameplay_Manager.Instance.pools;
			for (int i = 0; i < pools.Length; i++)
			{
				if (pools[i].ReturnObjectToPool(other.gameObject))
				{
					break;
				}
			}
		}
	}

	private bool CheckIfRightCombination(Stick_Content stick) {
		//First check if the order is right
		if (Gameplay_Manager.Instance.orders.TakeOrder(stick.currentOrder.order)){
			int points = 0;

			//Then check if the ingredients are not burnt
			foreach (Food food in stick.foods) {
				if (food.currentStage == Food.stage.WellDone) {
					points++;
				}
			}

			if(points == stick.foods.Count) {
				return true;
			}
		}

		return false;
	}
}
