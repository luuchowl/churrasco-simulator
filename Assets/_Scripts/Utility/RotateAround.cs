using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {
	public float anglesPerSecond = 100;
	public Vector3 axis;
	public Space space;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(axis, anglesPerSecond * Time.deltaTime, space);
	}
}
