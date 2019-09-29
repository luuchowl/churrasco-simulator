using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour {
	[BoxTitle("Preferences")]
	public int correctPoints = 10;
	public int wrongPoints = -20;

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Stick")) {
			Stick_Content stick = other.gameObject.GetComponent<Stick_Content>();

			if (CheckIfRightCombination(stick)) {
				Debug.Log("Correct!");
				Game_Manager.Instance.AddPoints(correctPoints);
			} else {
				Debug.Log("Incorrect...");
				Game_Manager.Instance.AddPoints(wrongPoints);
				Game_Manager.Instance.levelController.orders.AddWrong();
			}

			//Clear the ingredients
			stick.ReturnFoodsToPool();

			//Return the Stick
			ObjectPool[] pools = Game_Manager.Instance.levelController.pools;
			for (int i = 0; i < pools.Length; i++) {
				if (pools[i].ReturnObjectToPool(other.gameObject)) {
					break;
				}
			}

			//Clear the order
			stick.currentOrder.order.ingredients.Clear();
		}
	}

	private bool CheckIfRightCombination(Stick_Content stick) {
		//First check if the order is right
		if (Game_Manager.Instance.levelController.orders.TakeOrder(stick.currentOrder.order)){
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
