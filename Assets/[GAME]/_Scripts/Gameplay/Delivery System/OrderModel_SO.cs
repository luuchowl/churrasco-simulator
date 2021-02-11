using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Order Model")]
public class OrderModel_SO : ScriptableObject
{
    public float baseTime;
    public Tag[] orderTags;
}
