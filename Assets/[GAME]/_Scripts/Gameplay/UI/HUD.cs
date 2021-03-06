﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public Pool_SO order_UIPool;
    public RectTransform ordersParent;
    public Pool_SO mistakes_UIPool;
    public RectTransform mistakesParent;
    public TMP_Text pointsText;

    private OrderManager orderManager;
    private List<Order_UI> orders = new List<Order_UI>();

    private void Awake()
    {
        orderManager = FindObjectOfType<OrderManager>();

        orderManager.orderAdded += OnOrderAdded;
        orderManager.orderRemoved += OnOrderRemoved;
        orderManager.orderUpdated += OnOrderUpdated;
        orderManager.orderDelivered += OnOrderDelivered;

        UpdatePointsText(0);
    }

    private void OnDestroy()
    {
        orderManager.orderAdded -= OnOrderAdded;
        orderManager.orderRemoved -= OnOrderRemoved;
        orderManager.orderUpdated -= OnOrderUpdated;
        orderManager.orderDelivered -= OnOrderDelivered;
    }

    private void OnOrderAdded(Order obj)
    {
        Order_UI order = (Order_UI)order_UIPool.Request();
        order.SetOrder(obj);
        orders.Add(order);
        order.transform.SetParent(ordersParent, false);
    }

    private void OnOrderRemoved(Order obj)
    {
        Order_UI order = orders.FirstOrDefault(x => x.currentOrder == obj);
        order_UIPool.Return(order);
        orders.Remove(order);
    }

    private void OnOrderUpdated(Order obj)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].currentOrder == obj)
            {
                orders[i].UpdateUI();
            }
        }
    }

    private void OnOrderDelivered(bool correct, int points)
    {
        if (!correct)
        {
            Transform mistake = mistakes_UIPool.Request().transform;
            mistake.SetParent(mistakesParent, false);
        }
    }

    public void UpdatePointsText(int points)
    {
        pointsText.text = points.ToString();
    }
}
