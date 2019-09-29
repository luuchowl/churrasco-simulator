using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/New Props Database")]
public class Props_Database : ScriptableObject {
	public Transform[] ground;
	public Transform[] wall;
	public Transform[] bbq;
	public Transform[] table;
}
