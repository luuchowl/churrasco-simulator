using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PoolableObject obj = other.attachedRigidbody.GetComponent<PoolableObject>();

		if(obj != null)
		{
			obj.ReturnToPool();
		}
	}
}
