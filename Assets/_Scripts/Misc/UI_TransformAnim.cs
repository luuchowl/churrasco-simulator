using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class UI_TransformAnim : MonoBehaviour {
	[Header("Basics")]
	public bool playOnEnable = true;
	public float duration = 1;
	[Tooltip("If true, then the start position is the END of the animation, otherwise, it'll be the START.")]
	public bool useStartPositionAsDestination = true;
	[Tooltip("-1 means infinite, 0 means no Loop (the animation will play only once) and Any other positive number will be the amount of loops the animation will play once started.")]
	public int loop = 0;
	[Header("Delay")]
	public float startDelay;
	public float randomDelayOffset;
	public float loopDelay;
	[Header("Anim Parameters")]
	public PositionInfo position;
	public RotationInfo rotation;
	public ScaleInfo scale;
	public ColorInfo color;
	public Events events;

	[HideInInspector] public bool playing;

	private RectTransform myTransform;
	private Graphic graphic; //Graphic is the base class for all of Unity's UI graphic elements (Image, Text, Raw Image, even Text Mesh Pro texts)

	#region Structs
	[System.Serializable]
	public struct PositionInfo {
		public bool animatePosition;
		public AnimationCurve curve;
		public Vector2 offset;
		[HideInInspector] public Vector2 startPos;
	}

	[System.Serializable]
	public struct RotationInfo {
		public bool animateRotation;
		public AnimationCurve curve;
		public float offset;
		[HideInInspector] public Vector3 startRot;
	}

	[System.Serializable]
	public struct ScaleInfo {
		public bool animateScale;
		public AnimationCurve curve;
		public Vector2 offset;
		[HideInInspector] public Vector3 startScale;
	}

	[System.Serializable]
	public struct ColorInfo {
		public bool animateColor;
		public AnimationCurve curve;
		public Color offset;
		[HideInInspector] public Color startColor;
	}

	[System.Serializable]
	public struct Events {
		public UnityEvent beforeLoopAnimStart;
		public UnityEvent animStart;
		/// <summary>
		/// Is not called if delay is 0.
		/// </summary>
		public UnityEvent animStartAfterDelay;
		public UnityEvent animEnd;
		/// <summary>
		/// Is not called if loops is infinite.
		/// </summary>
		public UnityEvent afterLoopAnimEnd;
	}
	#endregion

	// Use this for initialization
	void Awake() {
		myTransform = GetComponent<RectTransform>();
		position.startPos = myTransform.anchoredPosition;
		rotation.startRot = myTransform.rotation.eulerAngles;
		scale.startScale = myTransform.localScale;

		if (graphic = GetComponent<Graphic>()) {
			color.startColor = graphic.color;
		}
	}

	private void OnEnable() {
		if (playOnEnable) {
			PlayAnim();
		}
	}

	private void OnValidate() {
		if (loop < -1)
			loop = -1;

		if (duration < 0)
			duration = 0.001f;
	}

	public void PlayAnim() {
		StopCoroutine("AnimRoutine");
		ResetAll();
		StartCoroutine("AnimRoutine");
	}

	public void ResetAll() {

		if(position.animatePosition) myTransform.anchoredPosition = position.startPos;
		if (rotation.animateRotation) myTransform.rotation = Quaternion.Euler(rotation.startRot);
		if (scale.animateScale) myTransform.localScale = scale.startScale;

		if (color.animateColor && graphic != null) {
			graphic.color = color.startColor;
		}
	}

	private IEnumerator AnimRoutine() {
		float timePassed = 0;
		float value = 0;
		int loopCount = 0;
		bool ignoreDelay = false;

		playing = true;
		events.beforeLoopAnimStart.Invoke();

		do {
			events.animStart.Invoke();
			timePassed = 0;

			while (timePassed < duration) {
				//Position
				if (position.animatePosition) {
					value = position.curve.Evaluate(timePassed / duration);
					ChangePosition(value);
				}

				//Rotation
				if (rotation.animateRotation) {
					value = rotation.curve.Evaluate(timePassed / duration);
					ChangeRotation(value);
				}

				//Scale
				if (scale.animateScale) {
					value = scale.curve.Evaluate(timePassed / duration);
					ChangeScale(value);
				}

				//Color
				if (color.animateColor && graphic != null) {
					value = color.curve.Evaluate(timePassed / duration);
					ChangeColor(value);
				}

				//If this is the first loop, we check if there's a start delay. We do this here so we can change all parameters to theirs "start values" before starting the animation
				if (startDelay > 0 && timePassed == 0 && !ignoreDelay) {
					yield return new WaitForSeconds(startDelay + (Random.Range(0, randomDelayOffset)));
					events.animStartAfterDelay.Invoke();
					ignoreDelay = true;
				}

				timePassed += Time.deltaTime;
				yield return null;
			}

			if (loop != -1) {
				loopCount++;
			}

			events.animEnd.Invoke();

			yield return new WaitForSeconds(loopDelay);
			yield return null;
		} while (loop == -1 || (loop > 0 && loopCount <= loop));

		//Because deltaTime is variable, we put the object in the desired values manually after the routine ends
		ResetAll();

		events.afterLoopAnimEnd.Invoke();
		playing = false;
	}

	private void ChangePosition(float value) {
		if (useStartPositionAsDestination) {
			myTransform.anchoredPosition = Vector2.LerpUnclamped(position.startPos + position.offset, position.startPos, value);
		} else {
			myTransform.anchoredPosition = Vector2.LerpUnclamped(position.startPos, position.startPos + position.offset, value);
		}
	}

	private void ChangeRotation(float value) {
		if (useStartPositionAsDestination) {
			myTransform.rotation = Quaternion.Euler(Vector3.LerpUnclamped(rotation.startRot + (Vector3.forward * rotation.offset), rotation.startRot, value));
		} else {
			myTransform.rotation = Quaternion.Euler(Vector3.LerpUnclamped(rotation.startRot, rotation.startRot + (Vector3.forward * rotation.offset), value));
		}
	}

	private void ChangeScale(float value) {
		if (useStartPositionAsDestination) {
			myTransform.localScale = Vector3.LerpUnclamped(scale.startScale + (Vector3)scale.offset, scale.startScale, value);
		} else {
			myTransform.localScale = Vector3.LerpUnclamped(scale.startScale, scale.startScale + (Vector3)scale.offset, value);
		}
	}

	private void ChangeColor(float value) {
		Color c;

		if (useStartPositionAsDestination) {
			c = Color.LerpUnclamped(color.offset, color.startColor, value);
		} else {
			c = Color.LerpUnclamped(color.startColor, color.offset, value);
		}

		graphic.color = c;
	}
}
