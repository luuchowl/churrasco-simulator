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
                Item stickItem = stick.grabbedItens[i].GetComponent<Item>();
                itens.Add(stickItem);
            }
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
