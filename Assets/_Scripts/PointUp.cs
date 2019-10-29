using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUp : MonoBehaviour {
	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		//rb.AddRelativeTorque()
	}
}
