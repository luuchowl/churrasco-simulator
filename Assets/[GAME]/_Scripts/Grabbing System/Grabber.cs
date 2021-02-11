using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabber : MonoBehaviour
{
	public List<Grabbable> grabbedItens = new List<Grabbable>();
	[HideInInspector] public new Rigidbody rigidbody { get; private set; }
	[HideInInspector] public Collider[] colliders;

	protected virtual void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		colliders = GetComponentsInChildren<Collider>();
	}

	public void Grab(Transform pivot, Grabbable item)
	{
		item.OnGrab(this, pivot);
	}

	public void Release(Grabbable item)
	{
		if (!grabbedItens.Contains(item))
			return;

		item.OnRelease(this);
	}

	public Grabbable[] GetGrabbedItens()
	{
		return grabbedItens.ToArray();
	}
}
