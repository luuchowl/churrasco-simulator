using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Interaction_Controller : Grabber
{
	public Transform grabPivot;
	public float grabRange = 0.1f;
	public LayerMask grabMask;
	[SerializeField] private Player_Movement movement;

	private GameObject lastHoveredObj;

	public void GrabTarget()
	{
		Collider[] colliders = Physics.OverlapSphere(grabPivot.position, grabRange, grabMask);

		for (int i = 0; i < colliders.Length; i++)
		{
			if(colliders[i].gameObject == movement.GetHoveredObject())
			{
				Rigidbody objRb = colliders[i].attachedRigidbody;

				if(objRb != null)
				{
					Grabbable obj = objRb.GetComponent<Grabbable>();
					Grab(grabPivot, obj);
				}
			}
		}
	}

	public void ReleaseHeldObject()
	{
		if(grabbedItens.Count > 0)
		{
			Release(grabbedItens[0]);
		}
	}

	private void Update()
	{
		GameObject hoveredObj = movement.GetHoveredObject();
		
		if (hoveredObj != lastHoveredObj)
		{
			HoverOutline outline;
			if (lastHoveredObj != null)
			{
				outline = lastHoveredObj.GetComponentInParent<HoverOutline>();
			
				if(outline != null)
				{
					outline.OnHoverExit();
				}
			}

			if(hoveredObj != null)
			{
				outline = hoveredObj.GetComponentInParent<HoverOutline>();

				if (outline != null)
				{
					outline.OnHoverEnter();
				}

				lastHoveredObj = hoveredObj;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if(grabPivot != null)
		{
			Gizmos.DrawWireSphere(grabPivot.position, grabRange);
		}
	}
}
