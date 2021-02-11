using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(menuName = "Custom/Database")]
public class Database_SO : ScriptableObject
{
    public Tag categoryTag;
    public ItemInfo[] objects;
}
