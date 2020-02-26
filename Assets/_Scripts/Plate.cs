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
				Game_Manager.Instance.AddPoints(correctPoints);
			} else {
				Debug.Log("Incorrect...");
				Game_Manager.Instance.AddPoints(wrongPoints);
				Game_Manager.Instance.ganeplayManager.orders.AddWrong();
			}

			//Clear the ingredients
			stick.ReturnFoodsToPool();

			//Return the Stick
			ObjectPool[] pools = Game_Manager.Instance.ganeplayManager.pools;
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

			if (Game_Manager.Instance.ganeplayManager.orders.TakeOrder(o))
			{
				if (f.currentStage == Food.stage.WellDone)
				{
					Debug.Log("Correct!");
					Game_Manager.Instance.AddPoints(correctPoints);
					correct = true;
				}
			}

			if (!correct)
			{
				Debug.Log("Incorrect...");
				Game_Manager.Instance.AddPoints(wrongPoints);
				Game_Manager.Instance.ganeplayManager.orders.AddWrong();
			}

			//Return the Ingredient
			ObjectPool[] pools = Game_Manager.Instance.ganeplayManager.pools;
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
		if (Game_Manager.Instance.ganeplayManager.orders.TakeOrder(stick.currentOrder.order)){
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
