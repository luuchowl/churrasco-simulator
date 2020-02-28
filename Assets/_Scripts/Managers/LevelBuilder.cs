using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelBuilder : MonoBehaviour {
	public Props_Database database;
	public Transform[] ground;
	public Transform[] wall;
	public Transform[] bbq;
	public Transform[] table;

	public void Randomize() {
		for (int i = 0; i < ground.Length; i++) {
			Transform t = Instantiate(database.ground[Random.Range(0, database.ground.Length)]);
			ReplaceObject(ground[i], t);
			ground[i] = t;
		}

		for (int i = 0; i < wall.Length; i++) {
			Transform t = Instantiate(database.wall[Random.Range(0, database.wall.Length)]);
			ReplaceObject(wall[i], t);
			wall[i] = t;
		}

		for (int i = 0; i < bbq.Length; i++) {
			Transform t = Instantiate(database.bbq[Random.Range(0, database.bbq.Length)]);
			ReplaceObject(bbq[i], t);
			bbq[i] = t;
		}

		for (int i = 0; i < table.Length; i++) {
			Transform t = Instantiate(database.table[Random.Range(0, database.table.Length)]);
			ReplaceObject(table[i], t);
			table[i] = t;
		}
	}

	private void ReplaceObject(Transform src, Transform dst) {
		dst.position = src.position;
		dst.rotation = src.rotation;
		dst.localScale = src.localScale;

		dst.SetParent(src.parent);

		Destroy(src.gameObject);
	}
}
