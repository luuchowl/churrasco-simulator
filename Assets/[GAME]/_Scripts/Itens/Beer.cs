using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
public class Beer : MonoBehaviour
{
	public GameObject effect;
	public Tag stickTag;
    public bool isEmpty { get; private set; }

	private Grabbable grabbable;

	private void Awake()
	{
		grabbable = GetComponent<Grabbable>();
		grabbable.onGrab += Grabbable_onGrab;
	}

	private void OnDestroy()
	{
		grabbable.onGrab -= Grabbable_onGrab;
	}

	private void OnEnable()
	{
		isEmpty = false;
		effect.SetActive(false);
	}

	private void Grabbable_onGrab(Grabber obj)
	{
		if (isEmpty)
			return;

		MultiTag objTags = obj.GetComponent<MultiTag>();

		if (objTags == null || !objTags.ContainsTag(stickTag))
			return;

		isEmpty = true;
		effect.SetActive(true);
	}
}
