using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabbableItem : Grabbable
{
    public bool allowGrabbingParent;
    public bool useCustomRotation;
    public Vector3 grabRotation;
    public Tag[] excludeTags;

    private FixedJoint joint;

    public override void OnGrab(Grabber grabber, Transform pivot)
    {
        //Check if we can even grab this object
        if (excludeTags.Length > 0)
        {
            MultiTag objTags = grabber.GetComponent<MultiTag>();

            if (objTags != null)
            {
                foreach (var tag in excludeTags)
                {
                    if (objTags.ContainsTag(tag))
                        return;
                }
            }
        }

        //Is this item is already grabbed, check if we can grab the parent
        if (isGrabbed)
        {
            if (allowGrabbingParent)
            {
                Grabbable parent = grabParent.GetComponent<Grabbable>();

                if (parent != null)
                {
                    parent.OnGrab(grabber, pivot);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        //Attach the object to the grabber
        rigidbody.isKinematic = true;
        transform.position = pivot.position;
        transform.parent = pivot;

        if (useCustomRotation)
            transform.localRotation = Quaternion.Euler(grabRotation);

        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = grabber.rigidbody;

        Helper.ChangeLayerRecursively(transform, LayerMask.NameToLayer(ignoreCollisionlayerName));
        ChangeCollisionDetectionfItem(grabber, true);

        grabParent = grabber;
        grabber.grabbedItens.Add(this);
        RaiseOnGrabEvent();
    }

    public override void OnRelease(Grabber grabber)
    {
        rigidbody.isKinematic = false;
        transform.parent = null;
        Destroy(joint);

        Helper.ChangeLayerRecursively(transform, originalLayer);
        ChangeCollisionDetectionfItem(grabber, false);

        grabber.grabbedItens.Remove(this);
        RaiseOnReleaseEvent();
        grabParent = null;
    }
}
