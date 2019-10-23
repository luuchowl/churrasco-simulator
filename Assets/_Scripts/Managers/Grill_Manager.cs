using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill_Manager : MonoBehaviour {
	public AudioSource grillSoundSource;
	public List<Cookable> objectsOnGrill = new List<Cookable>();

	private void Start() {
		grillSoundSource.loop = true;
		grillSoundSource.clip = Sound_Manager.Instance.audioHolder.grillingMeat.simple[0];
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Ingredient") || other.gameObject.CompareTag("Stick")) {
			AddToGrill(other.gameObject.GetComponent<Cookable>());
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag("Ingredient") || other.gameObject.CompareTag("Stick")) {
			RemoveFromGrill(other.gameObject.GetComponent<Cookable>());
		}
	}

	private void Update() {
		if (objectsOnGrill.Count > 0) {
			foreach (Cookable item in objectsOnGrill) {
				item.Cook();
			}
		}
	}

	public void AddToGrill(Cookable obj) {
		objectsOnGrill.Add(obj);

		grillSoundSource.Play();

		if (objectsOnGrill.Count == 0) {
			grillSoundSource.Stop();
		}

		Food food = obj.GetComponent<Food>();
		if (food != null) {
			Sound_Manager.Instance.PlayRandomSFX(food.grabClips);
		}
	}

	public void RemoveFromGrill(Cookable obj) {
		objectsOnGrill.Remove(obj);

		if (objectsOnGrill.Count == 0) {
			grillSoundSource.Stop();
		}

		Food food = null;
		if (food = obj.GetComponent<Food>()) {
			Sound_Manager.Instance.PlayRandomSFX(food.grabClips);
		}
	}
}
