using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealChurras : MonoBehaviour {

    public Transform target;
	public Transform pivot;
	public float stealDuration = 1;
    public float returnDuration = 1;
	public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

	[HideInInspector] public Vector3 initPos;

	private Grill_Manager grill;
	private Grabber player;
	private Vector3 lastTargetPosition;
	private bool returning;
	private float counter;
	private float step;
	private bool grabbed;
	private Food food;
	private Stick_Content stick;

	private void OnEnable() {
		grill = Gameplay_Manager.Instance.grill;
		player = Gameplay_Manager.Instance.player.GetComponentInChildren<Grabber>();

		initPos = transform.position;

		SelectNewTarget();
	}

	void SelectNewTarget() {
		food = null;
		stick = null;

		int id = Random.Range(0, grill.objectsOnGrill.Count);
		target = grill.objectsOnGrill[id].transform;

		step = 1 / stealDuration;
		lastTargetPosition = target.position;
		transform.forward = target.position - transform.position;
		food = target.GetComponent<Food>();

		if(food == null) {
			stick = target.GetComponent<Stick_Content>();
		}
	}

	// Update is called once per frame
	void Update() {
		if(target != null) {
			Vector3 handPosition = transform.position;
			counter += step * Time.deltaTime;

			if (!returning) {
				float value = curve.Evaluate(counter);
				handPosition = Vector3.Lerp(initPos, lastTargetPosition, value);

				if (player.grabObject != null && (target == player.grabObject.transform || (food != null && food.stick != null && food.stick.gameObject == player.grabObject))) {
					StartReturning();
					lastTargetPosition = handPosition;
				} else if(value >= 1) {
					Debug.Log("Foi");
					StartReturning();
					target.SetParent(pivot.transform);
					grabbed = true;
					target.GetComponent<Rigidbody>().isKinematic = true;
					target.GetComponent<Collider>().enabled = false;
					counter = 0;
					lastTargetPosition = handPosition;
				}
			} else {
				float value = curve.Evaluate(counter);
				handPosition = Vector3.Lerp(lastTargetPosition, initPos, counter);
				
				if (value >= 1) {
					if (grabbed) {
						target.transform.SetParent(null);
						target.GetComponent<Poolable>().ReturnToPool();
						target.GetComponent<Collider>().enabled = true;
					}

					Destroy(this.gameObject);
				}
			}

			pivot.transform.position = handPosition;
		} else {
			Destroy(this.gameObject);
		}
	}

	public void StartReturning() {
		returning = true;
		step = 1 / returnDuration;
	}

	void Hit() {
		//Change Animation
		returning = true;
		step = returnDuration / Vector3.Distance(transform.position, target.position);
	}

	private void OnDrawGizmos() {
		if(target != null) {
			Gizmos.DrawWireSphere(target.transform.position, 0.1f);
			Gizmos.DrawWireSphere(lastTargetPosition, 0.1f);
		}
	}
}
