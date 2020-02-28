using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick_Content : Cookable {
	public Transform[] ingredientsPivots;
	public OrderHolder currentOrder;
	public List<Food> foods = new List<Food>();
	
	private int counter = 0;
	private Rigidbody rb;
	private Collider col;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
	}

	private void OnEnable() {
		counter = 0;
	}

	public void AddIngredient(Food food) {
		if(counter < ingredientsPivots.Length) {
			food.transform.position = ingredientsPivots[counter].position;
			currentOrder.order.ingredients.Add(food.ingredientImage);
			foods.Add(food.GetComponent<Food>());

			food.stick = this;
			food.GetComponent<Rigidbody>().isKinematic = true;
			Physics.IgnoreCollision(col, food.col, true);
			food.transform.SetParent(ingredientsPivots[counter]);
			food.transform.localRotation = Quaternion.Euler(Random.insideUnitSphere * 360f);
			
			counter++;
		}
	}

	public override void Cook() {
		for (int i = 0; i < foods.Count; i++) {
			if (foods[i] != null) {
				foods[i].Cook();
			}
		}
	}

	public void ReturnFoodsToPool() {
		foreach (Food food in foods) {
			if (food != null) {
				food.GetComponent<Rigidbody>().isKinematic = false;
				Physics.IgnoreCollision(col, food.col, false);
				food.GetComponent<Poolable>().ReturnToPool();
				food.stick = null;
			}
		}

		foods.Clear();
	}

	public void Grab() {
		col.enabled = false;
	}

	public void Release() {
		col.enabled = true;
	}
}
