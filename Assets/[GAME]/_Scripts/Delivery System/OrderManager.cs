using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using System;

using Random = UnityEngine.Random; //Solves the ambiguity between UnityEngine.Random and System.Random

public class OrderManager : MonoBehaviour
{
    public Database_SO[] databases;
    public OrderModel_SO[] possibleOrders;
    public List<Order> currentOrders = new List<Order>();
    public bool ignoreArrangement = true;
    public DeliveryArea deliveryArea;

    public event Action<Order> orderAdded;
    public event Action<Order> orderRemoved;
    public event Action<Order> orderUpdated;
    public event Action<bool> orderDelivered;

    private void Awake()
    {
        deliveryArea.orderDelivered += CheckOrder;
    }

    private void OnDestroy()
    {
        deliveryArea.orderDelivered -= CheckOrder;
    }

    public Order GenerateOrder()
    {
        OrderModel_SO orderModel_SO = possibleOrders[Random.Range(0, possibleOrders.Length)];

        Order order = new Order();
        order.remaingTime = orderModel_SO.baseTime;

        foreach (var orderTag in orderModel_SO.orderTags)
        {
            foreach (var database in databases)
            {
                if (database.categoryTag == orderTag)
                {
                    ItemInfo ingredient = database.objects[Random.Range(0, database.objects.Length)];
                    order.itens.Add(ingredient);
                }
            }
        }

        currentOrders.Add(order);
        orderAdded?.Invoke(order);

        return order;
    }

    public void RemoveOrder(Order order)
    {
        currentOrders.Remove(order);
        orderRemoved?.Invoke(order);
    }

    public void ClearOrders()
    {
        while (currentOrders.Count > 0)
        {
            RemoveOrder(currentOrders[0]);
        }
    }

    public void UpdateCurrentOrdersTime(float timeToRemove)
    {
        for (int i = 0; i < currentOrders.Count; i++)
        {
            Order order = currentOrders[i];

            order.remaingTime -= timeToRemove;
            orderUpdated?.Invoke(order);

            if (order.remaingTime <= 0)
            {
                RemoveOrder(order);
                i--;
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            Order order = GenerateOrder();
            Debug.Log($"Order generated: {string.Join(" - ", order.itens.Select(x => x.itemTag.name))}");
        }

        //TODO remove this later
        UpdateCurrentOrdersTime(Time.deltaTime);
    }

    private void CheckOrder(List<Item> itens)
    {
        // We can compare two lists to check if all their elements are the same by using "SequenceEqual".
        // But SequeceEqual expects that the arrangement of the sequence to be equal too, so, if we don't care
        // about the arrangement of the itens, we have to manually arrange them.
        ItemInfo[] plate = itens.Select(x => x.info).ToArray();

        if (ignoreArrangement)
        {
            plate = plate.OrderBy(x => x.itemTag.name).ToArray();
        }

        foreach (var orderWanted in currentOrders)
        {
            ItemInfo[] wanted = orderWanted.itens.ToArray();

            if (ignoreArrangement)
            {
                wanted = wanted.OrderBy(x => x.itemTag.name).ToArray();
            }

            if (wanted.SequenceEqual(plate))
            {
                OrderIsRight(itens);
                RemoveOrder(orderWanted);
                return;
            }
        }

        OrderIsWrong(itens);
    }

    private void OrderIsRight(List<Item> itens)
    {
        List<Grabbable> itensToGrab = new List<Grabbable>();

        for (int i = 0; i < itens.Count; i++)
        {
            Grabbable item = itens[i].GetComponent<Grabbable>();

            if (item.isGrabbed)
            {
                Grabbable parent = item.grabParent.GetComponent<Grabbable>();

                if (!itensToGrab.Contains(parent))
                    itensToGrab.Add(parent);
            }
            else
            {
                if (!itensToGrab.Contains(item))
                    itensToGrab.Add(item);
            }
        }

        //Return the objects
        ReturnObjectsToPool(itens);

        Debug.Log($"Order correct! The delivery guy will take these itens [{string.Join(" - ", itensToGrab.Select(x => x.name))}]");
        orderDelivered?.Invoke(true);
    }

    private void OrderIsWrong(List<Item> itens)
    {
        ReturnObjectsToPool(itens);

        Debug.Log("Order is wrong...");
        orderDelivered?.Invoke(false);
    }

    private void ReturnObjectsToPool(List<Item> itens)
    {
        for (int i = 0; i < itens.Count; i++)
        {
            PoolableObject p = itens[i].GetComponent<PoolableObject>();
            p.ReturnToPool();
        }
    }
}
