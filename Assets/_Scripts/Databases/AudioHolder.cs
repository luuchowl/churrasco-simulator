using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Holder", menuName = "Custom/New Audio Holder")]
public class AudioHolder : ScriptableObject {
	public SimpleSound music;
	public SimpleSound ambient;

	public SimpleSound meatDrop;
    public SimpleSound meatSound;
    public SimpleSound openCan;
    public SimpleSound pickingUpOther;

    public SimpleSound slap;
    public SimpleSound burningCoal;
    public SimpleSound grillingMeat;
    public SimpleSound handShowing;
	
	[System.Serializable]
	public class SimpleSound {
		public AudioClip[] simple;
	}
}
