using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealChurras : MonoBehaviour {

    public Transform target;
    public float stealDuration = 1;
    public float returnDuration = 1;

	[HideInInspector] public Vector3 initPos;

	private Grill_Manager grill;
	private Grabber player;
	private Vector3 lastTargetPosition;
	private bool returning;
	private float counter;
	private float step;
	private bool grabbed;
	private Food food;

	private void OnEnable() {
		grill = Game_Manager.Instance.levelController.grill;
		player = Game_Manager.Instance.levelController.player.GetComponentInChildren<Grabber>();

		initPos = transform.position;

		SelectNewTarget();
	}

	void SelectNewTarget() {
		int id = Random.Range(0, grill.objectsOnGrill.Count);
		target = grill.objectsOnGrill[id].transform;

		step = 1 / stealDuration;
		lastTargetPosition = target.position;
		transform.forward = target.position - transform.position;
		food = target.GetComponent<Food>();
	}

	// Update is called once per frame
	void Update() {
		if(target != null) {
			Vector3 handPosition = transform.position;

			if (!returning) {
				if (player.grabObject != null && target == player.grabObject.transform) {
					StartReturning();
					//if (grill.objectsOnGrill.Count > 0) {
					//	SelectNewTarget();
					//} else {
					//	returning = true;
					//}
				} else {
					counter += step * Time.deltaTime;
					handPosition = Vector3.Lerp(initPos, target.position, counter);

					if(counter >= 1) {
						StartReturning();
						target.SetParent(transform);
						grabbed = true;
						target.GetComponent<Rigidbody>().isKinematic = true;
						target.GetComponent<Collider>().enabled = false;
					}
				}
			} else {
				counter -= step * Time.deltaTime;
				handPosition = Vector3.Lerp(initPos, lastTargetPosition, counter);
				
				if (counter <= 0) {
					if (grabbed) {
						target.transform.SetParent(null);
						target.GetComponent<Poolable>().ReturnToPool();
						target.GetComponent<Collider>().enabled = true;
					}
					Destroy(this.gameObject);
				}
			}

			transform.position = Vector3.Lerp(transform.position, handPosition, 0.2f);
		} else {
			Destroy(this.gameObject);
		}
	}

	public void StartReturning() {
		returning = true;
		lastTargetPosition = transform.position;
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
