using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkCamera : MonoBehaviour {

    public Material distortMat;
    private float distortScale;
    public float intensity = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.forward, Mathf.Cos(Time.time) * intensity);


        distortScale = (Mathf.Cos(Time.time) + 1) / 2;
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        distortMat.SetFloat("_Distortion", distortScale);
        Graphics.Blit(source, destination, distortMat);
    }

}
