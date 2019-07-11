using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/New Ingredients Database")]
public class Ingredients_Database : ScriptableObject {
	[PreviewSprite]
	public Sprite[] ingredientsSprites;
}
