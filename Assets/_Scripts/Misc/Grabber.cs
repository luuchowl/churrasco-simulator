using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Grabber : MonoBehaviour
{
	public Transform handPivot;
	public Transform normalGrabPivot;
	public Transform closeHandGrabPivot;
	public LayerMask grabMask;
	[BoxTitle("Normal Grab")]
	public AnimationCurve grabCurveDown;
	public AnimationCurve grabCurveUp;
	public float grabDuration;
	[BoxTitle("Stick Grab")]
	public AnimationCurve stickGrabCurveDown;
	public AnimationCurve stickGrabCurveUp;
	public AnimationCurve stickGrabRotationCurveDown;
	public AnimationCurve stickGrabRotationCurveUp;
	public float stickGrabDuration;

	[BoxTitle("Debug")]
	[ReadOnly] public GameObject grabObject;

	private Grill_Manager grill;
	private Player_Controller controller;
	private bool ready;
	private bool isGrabbing = false;
	private Vector3 initPos;
	private Quaternion initRot;
	private bool withStick;
	private Stick_Content stick;
	private bool finishedGrabbing = true;

	void Awake()
	{
		initPos = handPivot.localPosition;
		initRot = handPivot.localRotation;
		Gameplay_Manager.Instance.gameStartEvent.AddListener(Initialize);
	}

	public void Initialize()
	{
		grill = Gameplay_Manager.Instance.grill;
		controller = Gameplay_Manager.Instance.player;
		ready = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (ready)
		{
			if (Input.GetMouseButtonDown(0) && !controller.acting)
			{
				if (controller.hitObject != null && Helper.LayerMaskContains(grabMask, controller.hitObject.gameObject))
				{
					GameObject mainObj = controller.hitObject.attachedRigidbody != null ? controller.hitObject.attachedRigidbody.gameObject : controller.hitObject.gameObject;

					if (!isGrabbing)
					{
						FreeHandActions(mainObj);
					}
					else
					{
						if (withStick && mainObj.tag != "Stick")
						{
							Food f = mainObj.GetComponent<Food>();

							if (f == null || f.stick == null)
							{
								StartCoroutine("GrabObjectWithStick", mainObj);
							}
							else
							{
								DropObject();
							}
						}
						else
						{
							DropObject();
						}
					}
				}
				else if (isGrabbing)
				{
					DropObject();
				}
			}

			//Anim
			if (isGrabbing)
			{
				if (withStick)
				{
					controller.anim.SetBool("HoldClose", true);
					controller.anim.SetBool("HoldOpen", false);
				}
				else
				{
					controller.anim.SetBool("HoldOpen", true);
					controller.anim.SetBool("HoldClose", false);
				}
			}
			else
			{
				controller.anim.SetBool("HoldOpen", false);
				controller.anim.SetBool("HoldClose", false);
			}
		}
	}

	private void FreeHandActions(GameObject obj)
	{
		if (obj.tag == "Stick")
		{
			StartCoroutine("GrabObject", obj);
		}
		else if (obj.tag == "Ingredient")
		{
			Food food = obj.GetComponent<Food>();
			if (food != null && food.stick != null)
			{
				StartCoroutine("GrabObject", food.stick.gameObject);
			}
			else
			{
				StartCoroutine("GrabObject", obj);
			}
		}
	}

	private IEnumerator GrabObject(GameObject objectToGrab)
	{
		controller.acting = true;
		float timePassed = 0;
		Vector3 startPos = initPos;
		Vector3 endPos = Vector3.up * .05f;
		endPos.y = 0;
		float duration = grabDuration / 2;

		//Down
		while (timePassed < duration)
		{
			timePassed += Time.deltaTime;

			float value = grabCurveDown.Evaluate(timePassed / duration);
			handPivot.localPosition = Vector3.LerpUnclamped(startPos, endPos, value);

			yield return null;
		}

		//Grab
		Transform pivot = GetCorrectPivot(objectToGrab);

		ObjectPool pool = objectToGrab.GetComponent<ObjectPool>();
		if (pool != null)
		{
			objectToGrab = pool.GetPooledObject(false);
			objectToGrab.GetComponent<Poolable>().pool = pool;
		}

		stick = objectToGrab.GetComponent<Stick_Content>();
		if (stick != null)
		{
			stick.Grab();
			objectToGrab.transform.localRotation = pivot.rotation;
			Sound_Manager.Instance.PlayRandomSFX(Sound_Manager.Instance.audioHolder.pickingUpOther.simple);
		}
		else
		{
			objectToGrab.transform.localRotation = Quaternion.Euler(Random.insideUnitSphere * 360f);
			Sound_Manager.Instance.PlayRandomSFX(objectToGrab.GetComponent<Food>().grabClips);
		}

		grabObject = objectToGrab;
		Rigidbody objectRB = grabObject.GetComponent<Rigidbody>();
		objectRB.isKinematic = true;
		objectRB.transform.position = pivot.position;
		objectRB.transform.SetParent(pivot);
		isGrabbing = true;
		grill.RemoveFromGrill(objectRB.GetComponent<Cookable>());

		//Up
		timePassed = 0;
		startPos = endPos;
		endPos = initPos;

		while (timePassed < duration)
		{
			timePassed += Time.deltaTime;

			float value = grabCurveUp.Evaluate(timePassed / duration);
			handPivot.localPosition = Vector3.LerpUnclamped(startPos, endPos, value);

			yield return null;
		}

		handPivot.localPosition = endPos;
		controller.acting = false;
		finishedGrabbing = true;
	}

	private IEnumerator GrabObjectWithStick(GameObject objectToStick)
	{
		if (stick.foods.Count >= stick.ingredientsPivots.Length)
		{
			yield break;
		}

		controller.acting = true;
		float timePassed = 0;
		Vector3 startPos = initPos;
		Vector3 endPos = Vector3.up * (stick.foods.Count + 1);
		Quaternion startRot = initRot;
		Quaternion endRot = initRot * Quaternion.AngleAxis(90, transform.forward);
		endPos.y = 0;
		float duration = stickGrabDuration / 2;

		//Disable collisions
		foreach (Food item in stick.foods)
		{
			item.col.enabled = false;
		}

		//Down
		while (timePassed < duration)
		{
			timePassed += Time.deltaTime;

			float value = stickGrabCurveDown.Evaluate(timePassed / duration);
			handPivot.localPosition = Vector3.LerpUnclamped(startPos, endPos, value);
			value = stickGrabRotationCurveDown.Evaluate(timePassed / duration);
			handPivot.localRotation = Quaternion.SlerpUnclamped(startRot, endRot, value);

			yield return null;
		}

		//Stick ingredient
		if (objectToStick.tag != "Stick")
		{
			ObjectPool pool = objectToStick.GetComponent<ObjectPool>();
			if (pool != null)
			{
				objectToStick = pool.GetPooledObject();
				objectToStick.GetComponent<Poolable>().pool = pool;
			}

			Food food = objectToStick.GetComponent<Food>();
			food.col.enabled = false;
			stick.AddIngredient(food);
			Sound_Manager.Instance.PlayRandomSFX(food.grabClips);
		}

		grill.RemoveFromGrill(objectToStick.GetComponent<Cookable>());

		//Up
		timePassed = 0;
		startPos = endPos;
		endPos = initPos;
		startRot = endRot;
		endRot = initRot;

		while (timePassed < duration)
		{
			timePassed += Time.deltaTime;

			float value = stickGrabCurveUp.Evaluate(timePassed / duration);
			handPivot.localPosition = Vector3.LerpUnclamped(startPos, endPos, value);
			value = stickGrabRotationCurveUp.Evaluate(timePassed / duration);
			handPivot.localRotation = Quaternion.SlerpUnclamped(startRot, endRot, value);

			yield return null;
		}

		handPivot.localPosition = initPos;
		handPivot.localRotation = initRot;
		controller.acting = false;
		finishedGrabbing = true;

		//Enable Collisions
		foreach (Food item in stick.foods)
		{
			item.col.enabled = true;
		}
	}

	private void DropObject()
	{
		Rigidbody objRB = grabObject.GetComponent<Rigidbody>();
		objRB.isKinematic = false;
		objRB.transform.SetParent(objRB.GetComponent<Poolable>().pool.transform);
		isGrabbing = false;
		withStick = false;
		grabObject = null;

		if (stick != null)
		{
			stick.Release();
			stick = null;
		}
	}

	private Transform GetCorrectPivot(GameObject c)
	{
		if (c.CompareTag("Stick"))
		{
			withStick = true;
			return closeHandGrabPivot;
		}

		return normalGrabPivot;
	}
}
