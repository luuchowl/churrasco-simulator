using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKidHand : MonoBehaviour {
	public float initialDelay = 1;
	public Vector2 randomDelay;
	public float minDelay = 1;
	public float challengeDecreaseOffset;

    public Transform[] spawnTransformers;
    public GameObject handPrefab;
	[Range(0, 100)] public float dogChanceSpawn;
	public GameObject dogHandPrefab;

	private void Awake() {

	}

	private void Update() {
		if (Input.GetKeyDown("a")) {
			SpawnHand();
		}
	}

	public void StartSpawners() {
		StartCoroutine("SpawnHands");
	}

	private IEnumerator SpawnHands() {
		yield return new WaitForSeconds(initialDelay);
		float offset = 0;

		while (true) {
			float delay = Mathf.Max(minDelay, Random.Range(randomDelay.x, randomDelay.y) - offset);

			yield return new WaitForSeconds(delay);

			SpawnHand();

			//50% chance of decreasing time that hands spawn
			if(Random.Range(0, 100) > 50) {
				offset += challengeDecreaseOffset;
			}
		}
	}

	public void SpawnHand() {
		Debug.Log("Trying to spawn kid hand...");
		if (Gameplay_Manager.Instance.grill.objectsOnGrill.Count > 0) {
			Transform spawn = spawnTransformers[(int)Mathf.Round(Random.Range(0f, 1f))];

			StealChurras hand;
			Vector3 pos = spawnTransformers[(int)Mathf.Round(Random.Range(0f, 1f))].position;

			//Chance of Spawning a dog hand
			if (Random.Range(0, 100) < dogChanceSpawn) {
				hand = Instantiate(dogHandPrefab, pos, Quaternion.identity).GetComponent<StealChurras>();
			} else {
				hand = Instantiate(handPrefab, pos, Quaternion.identity).GetComponent<StealChurras>();
			}
		} else {
			Debug.LogWarning("Nothing to steal!");

		}
	}
}
