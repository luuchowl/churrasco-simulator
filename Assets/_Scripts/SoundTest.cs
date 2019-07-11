using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Z)){
            Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.ambient.simple);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.meatDrop.simple);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.meatSound.simple);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.openCan.simple);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.pickingUpOther.simple);
        }
	}
}
