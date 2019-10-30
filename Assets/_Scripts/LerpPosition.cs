using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LerpPosition : MonoBehaviour {
	public float transtionDuration = .2f;
	public AnimationCurve curve;
	public Transform startPos;
	public Transform[] positions;
	public UnityEvent startedTransitionEvent = new UnityEvent();
	public UnityEvent finishedTransitionEvent = new UnityEvent();

	private Transform target;

	private void Start() {
		SetPos(startPos);
	}

	public void SetPos(Transform newPos) {
		target = newPos;
		StartCoroutine(MoveToPosition());
	}

	public void SetPos(int newPosID) {
		target = positions[newPosID];
		StartCoroutine(MoveToPosition());
	}

	private IEnumerator MoveToPosition() {
		startedTransitionEvent.Invoke();

		Vector3 initPos = transform.position;
		Quaternion initRot = transform.rotation;
		float timePassed = 0;

		while(timePassed < transtionDuration) {
			timePassed += Time.deltaTime;

			float value = curve.Evaluate(timePassed / transtionDuration);
			transform.position = Vector3.Lerp(initPos, target.position, value);
			transform.rotation = Quaternion.Slerp(initRot, target.rotation, value);

			yield return null;
		}

		transform.position = target.position;
		transform.rotation = target.rotation;

		finishedTransitionEvent.Invoke();
	}
}
