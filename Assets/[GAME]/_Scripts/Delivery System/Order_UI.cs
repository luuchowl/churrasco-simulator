using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order_UI : PoolableObject
{
	public Pool_SO itemImagePool;
	public Transform itemHolder;
	public Image timer;
	public Order currentOrder { get; private set; }

	private float maxTime;
	private List<PoolableObject> itemImages = new List<PoolableObject>();

	public void SetOrder(Order order)
	{
		currentOrder = order;
		maxTime = order.remaingTime;

		for (int i = 0; i < order.itens.Count; i++)
		{
			PoolableObject itemImage = itemImagePool.Request();
			Image img = itemImage.GetComponent<Image>();
			img.sprite = order.itens[i].sprite;
			itemImage.transform.SetParent(itemHolder);

			itemImages.Add(itemImage);
		}

		timer.fillAmount = 1;
	}

	public override void OnReturn()
	{
		for (int i = 0; i < itemImages.Count; i++)
		{
			itemImagePool.Return(itemImages[i]);
		}

		itemImages.Clear();
		currentOrder = null;
	}

	public void UpdateUI()
	{
		timer.fillAmount = currentOrder.remaingTime / maxTime;
	}
}
