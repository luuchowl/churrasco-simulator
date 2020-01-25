using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawOnClick : MonoBehaviour {
	public ObjectPool[] pools;
	public float height;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				GameObject obj = pools[Random.Range(0, pools.Length)].GetPooledObject();
				obj.transform.position = hit.point + Vector3.up * height;
			}
		}
	}
}
