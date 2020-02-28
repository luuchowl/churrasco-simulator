using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Food : Cookable {
	public enum stage { Raw, WellDone, Burnt }

	[BoxTitle("Preferences")]
	public Sprite ingredientImage;
	public AudioClip[] grabClips;
	public float timeToBurn = 5;
	public Vector2 minMaxCooking = new Vector2(0.1f, 0.8f);
	[BoxTitle("References")]
	public GameObject model;
	public GameObject readyEffect;
	public GameObject burnSmoke;
	[BoxTitle("Misc")]
	[ReadOnly] public stage currentStage;

	[HideInInspector] public Collider col;
	[HideInInspector] public Rigidbody rb;
	[ReadOnly] public Stick_Content stick;
	
	private Renderer rend;
	private MaterialPropertyBlock propBlock;
	[SerializeField] private float burnCounter;

	// Use this for initialization
	void Awake () {
		rend = GetComponentInChildren<Renderer>();
		rb = GetComponent<Rigidbody>();
		col = GetComponentInChildren<Collider>();

		propBlock = new MaterialPropertyBlock();
	}

	private void OnEnable() {
		burnCounter = 0;
		SetFactor(burnCounter);
		currentStage = stage.Raw;
		if (readyEffect != null) readyEffect.SetActive(false);
		if (burnSmoke != null) burnSmoke.SetActive(false);
	}

	public override void Cook() {
		float percentage = burnCounter / timeToBurn;
		
		if (percentage < 1) {
			burnCounter += Time.deltaTime;
			SetFactor(Helper.Remap(percentage, 0, 1, 0, 3)); //3 is the max amount the "_Factor" property of the shader can go

			if(percentage > minMaxCooking.x && currentStage == stage.Raw) {
				currentStage = stage.WellDone;
				if (readyEffect != null) readyEffect.SetActive(true);
			} else if (percentage > minMaxCooking.y && currentStage == stage.WellDone) {
				currentStage = stage.Burnt;
				if(burnSmoke != null) burnSmoke.SetActive(true);
			}
		}
	}

	private void SetFactor(float value) {
		rend.GetPropertyBlock(propBlock);

		rend.material.SetFloat("_Factor", value);

		rend.SetPropertyBlock(propBlock);
	}
}
