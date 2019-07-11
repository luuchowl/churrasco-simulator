using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour {
	public ObjectPool pool;

	public void ReturnToPool() {
		pool.ReturnObjectToPool(this.gameObject);
	}
}
