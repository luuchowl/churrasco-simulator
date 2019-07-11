using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Order {
	public List<Sprite> ingredients = new List<Sprite>();
}

public class Order_Content : MonoBehaviour {
	public float timeToDisappear = 5;
	public RectTransform ingredientsHolder;
	public Order order;
	
	private void OnEnable() {
		Invoke("Disable", timeToDisappear);//TODO remove later
		transform.SetAsLastSibling();
	}

	public void Disable() { //TODO remove later?
		Game_Manager.Instance.levelController.orders.currentOrders.Remove(this);
		CancelInvoke();
		gameObject.SetActive(false);
	}

	private void OnDisable() {
		CancelInvoke();
	}
}
