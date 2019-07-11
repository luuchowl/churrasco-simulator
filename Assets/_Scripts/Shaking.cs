using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaking : MonoBehaviour {

	public float magnitude = 1;
	public float frequency = 1;

	// Update is called once per frame
	void Update() {
		float x = Mathf.PerlinNoise(Time.time * frequency, 0f) * magnitude;
		float y = Mathf.PerlinNoise(Time.time * frequency, 0.5f) * magnitude;
		float z = Mathf.PerlinNoise(Time.time * frequency, 1f) * magnitude;
		transform.localPosition = new Vector3(x, y, z);
	}
}
