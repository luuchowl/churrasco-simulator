using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/New Speech Database")]
public class Speech_Database : ScriptableObject {
	public float delayOnSequentialMessages = 1;
	public Conversation[] conversas;
}

[System.Serializable]
public class Conversation {
	public Speech[] falas;
}

[System.Serializable]
public class Speech {
	[TextArea]
	public string texto;
	public Sprite emoji;
}
