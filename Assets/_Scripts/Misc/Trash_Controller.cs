using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash_Controller : MonoBehaviour {
	public Transform hand;
	public float radius;

	private Transform myTransform;
	private Animator anim;

	private void Awake() {
		myTransform = transform;
		anim = GetComponent<Animator>();
	}

	private void Update() {
		float distance = Vector2.Distance(new Vector2(myTransform.position.x, myTransform.position.z), new Vector2(hand.position.x, hand.position.z));
		anim.SetBool("Opened", distance < radius);
	}
}
