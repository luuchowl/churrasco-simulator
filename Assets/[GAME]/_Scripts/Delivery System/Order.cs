using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Order 
{
    public float remaingTime;
    public List<ItemInfo> itens = new List<ItemInfo>();
}
