using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

[RequireComponent(typeof(Grabbable), typeof(PoolableObject))]
public class Stick : Grabber
{
	[SerializeField] private Transform[] pivots;
	[SerializeField] private Collider grabPoint;
	public float disableGrabPointDelay;

	[SerializeField] private Tag[] stickableTags;
	private Grabbable grabbable;
	private PoolableObject poolable;

	protected override void Awake()
	{
		base.Awake();
		grabbable = GetComponent<Grabbable>();
		poolable = GetComponent<PoolableObject>();

		grabbable.onGrab += OnGrab;
		grabbable.onRelease += OnRelease;
		poolable.onReturn += OnReturn;
	}

	private void OnDestroy()
	{
		grabbable.onGrab -= OnGrab;
		grabbable.onRelease -= OnRelease;
		poolable.onReturn -= OnReturn;
	}

	private void OnEnable()
	{
		EnableGrabPoint(false);
	}

	private void OnGrab(Grabber grabber)
	{
		EnableGrabPoint(true);
	}

	private void OnRelease(Grabber grabber)
	{
		EnableGrabPoint(false);

		for (int i = 0; i < grabbedItens.Count; i++)
		{
			grabbedItens[i].rigidbody.isKinematic = false;
		}
	}

	private void OnReturn()
	{
		while (grabbedItens.Count > 0)
		{
			Grabbable item = grabbedItens[0];
			Release(item);

			PoolableObject poolableItem = item.GetComponent<PoolableObject>();
			poolableItem.ReturnToPool();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (grabbedItens.Count >= pivots.Length)
			return;

		Rigidbody colRb = other.attachedRigidbody;

		if (colRb == null)
			return;

		//Check if we can grab this object by comparing its tags
		//MultiTag objTags = colRb.GetComponent<MultiTag>();

		//if (objTags != null && stickableTags.Any(x => objTags.ContainsTag(x)))
		//{

		//}

		Grabbable obj = colRb.GetComponent<Grabbable>();

		if (grabbedItens.Contains(obj))
			return;

		Grab(GetFreePivot(), obj);
		grabPoint.enabled = false;
	}

	public void EnableGrabPoint(bool enable)
	{
		grabPoint.enabled = enable;
	}

	public Transform GetFreePivot()
	{
		if (grabbedItens.Count >= pivots.Length)
			return null;

		return pivots[grabbedItens.Count];
	}
}
