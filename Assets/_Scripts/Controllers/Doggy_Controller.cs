using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggy_Controller : MonoBehaviour {
	public Rigidbody doggyObj;

	private Transform doggyTransform;
	private List<Transform> snacksList = new List<Transform>();

	// Use this for initialization
	void Start () {
		doggyTransform = doggyObj.transform;
	}

	private void OnCollisionEnter(Collision collision) {
		if(collision.rigidbody.tag == "Ingredient") {
			snacksList.Add(collision.rigidbody.transform);
		}
	}
}
