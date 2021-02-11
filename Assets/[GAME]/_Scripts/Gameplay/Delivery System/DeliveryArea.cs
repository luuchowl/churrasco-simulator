using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryArea : MonoBehaviour
{
    private OrderManager orderManager;

    public event System.Action<List<Item>> orderDelivered;

    private void Awake()
    {
        orderManager = FindObjectOfType<OrderManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        List<Item> itens = new List<Item>();

        //Check if it's a stick
        Stick stick = rb.GetComponent<Stick>();

        if (stick != null)
        {
            for (int i = 0; i < stick.grabbedItens.Count; i++)
            {
                CookableFood food = stick.grabbedItens[i].GetComponent<CookableFood>();

                //The ingredient is only valid if it's not raw nor burnt
                if (food != null && food.state != CookState.WellDone)
                    continue;

                Item stickItem = stick.grabbedItens[i].GetComponent<Item>();
                itens.Add(stickItem);
            }

            stick.GetComponent<PoolableObject>().ReturnToPool();
        }
        else
        {
            //Anything that is not in a stick
            Item item = rb.GetComponent<Item>();

            if (item != null)
            {
                itens.Add(item);
            }
        }

        orderDelivered?.Invoke(itens);
    }
}
