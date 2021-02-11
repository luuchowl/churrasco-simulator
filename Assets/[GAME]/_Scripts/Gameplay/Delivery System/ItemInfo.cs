using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Item Info")]
public class ItemInfo : ScriptableObject
{
    public Tag itemTag;
    public Sprite sprite;
    public GameObject prefab;
}
