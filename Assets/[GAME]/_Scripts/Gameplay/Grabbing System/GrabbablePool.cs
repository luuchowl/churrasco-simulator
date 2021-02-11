using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbablePool : Grabbable
{
    public Pool_SO pool;

	public override void OnGrab(Grabber grabber, Transform pivot)
	{
		var obj = pool.Request();
		obj.GetComponent<Grabbable>().OnGrab(grabber, pivot);
	}
}
