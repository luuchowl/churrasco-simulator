using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggy_Detector : MonoBehaviour {
	public Doggy_Controller doggy;

	private void OnTriggerEnter(Collider other)
	{
		Rigidbody rb = other.attachedRigidbody;
		if (rb != null && rb.CompareTag("Ingredient"))
		{
			Transform t = rb.transform;
			if (!doggy.snacksList.Contains(t))
			{
				doggy.snacksList.Add(t);
			}
		}
	}
}
