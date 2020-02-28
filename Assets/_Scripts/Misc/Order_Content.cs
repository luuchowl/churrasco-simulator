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
    public Image timer;
    public Image postIt;

	public float currentTime = 5;
	
	private void OnEnable() {
		Invoke("Disable", timeToDisappear);//TODO remove later
		transform.SetAsLastSibling();
        currentTime = timeToDisappear;
	}

	public void Disable() { //TODO remove later?
		Gameplay_Manager.Instance.orders.currentOrders.Remove(this);
		//Game_Manager.Instance.levelController.orders.AddWrong();
		CancelInvoke();
		gameObject.SetActive(false);
	}

	private void OnDisable() {
		CancelInvoke();
	}

    private void Update(){
        currentTime -= Time.deltaTime;
        timer.fillAmount = currentTime / timeToDisappear ;
    }
}