using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public abstract class Grabbable : MonoBehaviour
{
	[HideInInspector] public new Rigidbody rigidbody { get; private set; }
	[HideInInspector] public Collider[] colliders;

	public event Action<Grabber> onGrab;
	public event Action<Grabber> onRelease;

	public Grabber grabParent { get; protected set; }
	public bool isGrabbed { get { return grabParent != null;  } }

	protected int originalLayer;
	protected const string ignoreCollisionlayerName = "IgnoreGrabbed";

	protected virtual void Awake()
	{
		originalLayer = gameObject.layer;
		rigidbody = GetComponent<Rigidbody>();
		colliders = GetComponentsInChildren<Collider>();
	}

	public virtual void OnGrab(Grabber grabber, Transform pivot)
	{

	}

	public virtual void OnRelease(Grabber grabber)
	{

	}

	protected void RaiseOnGrabEvent()
	{
		onGrab?.Invoke(grabParent);
	}

	protected void RaiseOnReleaseEvent()
	{
		onRelease?.Invoke(grabParent);
	}


	public void ChangeCollisionDetectionfItem(Grabber item, bool enable)
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			for (int j = 0; j < item.colliders.Length; j++)
			{
				Physics.IgnoreCollision(colliders[i], item.colliders[j], enable);
			}
		}
	}
}
