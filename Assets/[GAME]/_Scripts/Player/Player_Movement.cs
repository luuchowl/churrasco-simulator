using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
	public Transform visual;
    public Transform crosshair;
	[Header("Collision Detection")]
	public LayerMask collisionMask;
	public float sphereCastRadius = 0.1f;
	[Header("Movement")]
	public float speed = 1;
	[Tooltip("How many pixels does the mouse needs to move for the object to move 1 unit in the world?")]
	public int pixelsPerUnit = 100;
	[Range(0, 1)] public float smoothing = .5f;
	public Vector3 movementBounds;
	[Header("Vertical Movement")]
	public float animDuration;
	public AnimationCurve animCurve;
	public float heightOffset;
	[Header("Rotation")]
	public float anglesPerSecond;

	private Rigidbody rb;
	private Vector3 targetPos;
	private float dst;

	[Header("Debug")]
	[SerializeField, ReadOnly] private Collider hoveredObject;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Start()
	{
		targetPos = transform.position;
	}

	void Update()
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		
		if (Physics.SphereCast(ray, sphereCastRadius, out hit, 10f, collisionMask))
		{
			crosshair.position = hit.point + Vector3.up * 0.01f;
			hoveredObject = hit.collider;
			dst = hit.distance;

			Debug.DrawLine(ray.origin, hit.point);
		}
		else
		{
			hoveredObject = null;
		}
	}

	private void FixedUpdate()
	{
		Vector3 pos = targetPos;
		pos.x = Mathf.Clamp(pos.x, -movementBounds.x / 2, movementBounds.x / 2);
		pos.y = Mathf.Clamp(pos.y, -movementBounds.y / 2, movementBounds.y / 2);
		pos.z = Mathf.Clamp(pos.z, -movementBounds.z / 2, movementBounds.z / 2);
		targetPos = pos;

		rb.MovePosition(Vector3.Lerp(transform.position, targetPos, smoothing));
	}

	public void Move(Vector3 dir)
	{
		targetPos += (dir / pixelsPerUnit) * speed;
	}

	public void Rotate(float dir)
	{
		visual.Rotate(Vector3.forward, dir * anglesPerSecond * Time.deltaTime);
	}

	public void MoveToTarget()
	{
		StopAllCoroutines();
		StartCoroutine(MoveToTarget_Routine());
	}

	public void ReturnToRestPosition()
	{
		StopAllCoroutines();
		StartCoroutine(MoveToTarget_Routine(true));
	}

	private IEnumerator MoveToTarget_Routine(bool returning = false)
	{
		float t = 0;
		Vector3 startPos = visual.localPosition;

		while(t < animDuration)
		{
			t += Time.deltaTime;

			Vector3 endPos = returning ? Vector3.zero : crosshair.localPosition + Vector3.up * heightOffset;
			visual.localPosition = Vector3.LerpUnclamped(startPos, endPos, animCurve.Evaluate(t / animDuration));

			yield return null;
		}

		do
		{
			visual.localPosition = returning ? Vector3.zero : crosshair.localPosition + Vector3.up * heightOffset;
			yield return null;
		}
		while (!returning);
	}

	public GameObject GetHoveredObject()
	{
		return hoveredObject == null ? null : hoveredObject.gameObject;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position + Vector3.down * dst, sphereCastRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.zero, movementBounds);
	}
}
