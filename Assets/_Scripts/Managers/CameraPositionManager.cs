using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour {

    public Transform[] transforms;
    public float lerpFactor;
    private Vector3 targetPos;
    private Quaternion targetRot;

	// Use this for initialization
	void Start () {
        targetPos = transforms[4].position;
        targetRot = transforms[4].rotation;   	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpFactor);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpFactor);

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            targetPos = transforms[0].position;
            targetRot = transforms[0].rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            targetPos = transforms[1].position;
            targetRot = transforms[1].rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            targetPos = transforms[2].position;
            targetRot = transforms[2].rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            targetPos = transforms[3].position;
            targetRot = transforms[3].rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            targetPos = transforms[4].position;
            targetRot = transforms[4].rotation;
        }
	}

    public void LoadPos(int i){
        targetPos = transforms[i].position;
        targetRot = transforms[i].rotation;
    }
}
