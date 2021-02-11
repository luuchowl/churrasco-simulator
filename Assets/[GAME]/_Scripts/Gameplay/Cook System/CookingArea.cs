using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingArea : MonoBehaviour
{
	private List<Cookable> cookables = new List<Cookable>();

	private void OnTriggerEnter(Collider other)
	{
		Cookable obj = other.attachedRigidbody.GetComponent<Cookable>();

		if (obj != null)
		{
			obj.OnStartCooking();
			cookables.Add(obj);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Cookable obj = other.attachedRigidbody.GetComponent<Cookable>();

		if (obj != null && cookables.Contains(obj))
		{
			obj.OnStopCooking();
			cookables.Remove(obj);
		}
	}

	private void Update()
	{
		for (int i = 0; i < cookables.Count; i++)
		{
			cookables[i].Cook();
		}
	}
}
