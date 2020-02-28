using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolOnCollision : MonoBehaviour {
	private ObjectPool[] pools;

	private void Start() {
		pools = FindObjectsOfType<ObjectPool>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Stick")) {
			Stick_Content c = other.GetComponent<Stick_Content>();
			for (int i = 0; i < c.ingredientsPivots.Length; i++) {
				for (int j = 0; j < pools.Length; j++) {
					if (c.ingredientsPivots[i] != null) {
						pools[i].ReturnObjectToPool(c.ingredientsPivots[i].gameObject);
					}
				}
			}
		} else {
			for (int i = 0; i < pools.Length; i++) {
				pools[i].ReturnObjectToPool(other.gameObject);
			}
		}
	}
}
