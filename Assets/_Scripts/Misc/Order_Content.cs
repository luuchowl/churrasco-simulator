using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using System;

[System.Serializable]
public class Order {
	public List<Sprite> ingredients = new List<Sprite>();
}

public class Order_Content : MonoBehaviour {
	public float timeToDisappear = 5;
	public RectTransform ingredientsHolder;
	public Order order;
    public Image timer;
    public Image postIt;
    public Image correctIcon;
    public Image wrongIcon;
	public CanvasGroup group;
	[Header("Wrong Anim")]
	public float shakeDuration;
	public float shakeMagnitude;
	public float shakeFrequency;
	public float jumpDuration;
	public Vector2 jumpFinalPos;
	public AnimationCurve jumpXCurve;
	public AnimationCurve jumpYCurve;
	public float jumpRotation;
	[Header("Wrong Anim")]
	public float correctIconAnimDuration;
	public float flyUpDuration;
	public Vector2 flyUpFinalPos;
	public AnimationCurve flyUpXCurve;
	public AnimationCurve flyUpYCurve;
	public AnimationCurve flyUpScaleXCurve;
	public AnimationCurve flyUpScaleYCurve;


	private float currentTime = 5;
	private RectTransform rt;

	private void Awake()
	{
		rt = GetComponent<RectTransform>();
	}

	private void OnEnable() {
		Invoke("Disable", timeToDisappear);//TODO remove later
		transform.SetAsLastSibling();
        currentTime = timeToDisappear;
		group.alpha = 1;

		correctIcon.gameObject.SetActive(false);
		wrongIcon.gameObject.SetActive(false);
	}

	public void Disable() { //TODO remove later?
		Gameplay_Manager.Instance.orders.currentOrders.Remove(this);
		CancelInvoke();
		RemoveAsWrong();
	}

	private void OnDisable() {
		CancelInvoke();
	}

    private void Update(){
        currentTime -= Time.deltaTime;
        timer.fillAmount = currentTime / timeToDisappear ;
    }

	public void RemoveAsWrong()
	{
		CancelInvoke();
		StartCoroutine(WrongAnim_Routine());
	}

	private IEnumerator WrongAnim_Routine()
	{
		float timePassed = 0;
		float progress = 0;
		Vector2 startPos = rt.anchoredPosition;
		Vector2 newPos = Vector2.zero;
		wrongIcon.gameObject.SetActive(true);

		while (timePassed < shakeDuration)
		{
			timePassed += Time.deltaTime;
			progress = timePassed / shakeDuration;

			newPos.x = Mathf.Sin(timePassed * shakeFrequency) * shakeMagnitude;

			rt.anchoredPosition = startPos + newPos;
			wrongIcon.color = Color.Lerp(Color.clear, Color.red, progress);
			wrongIcon.transform.localScale = Vector3.one * Mathf.Lerp(2, 1, progress);

			yield return null;
		}

		rt.anchoredPosition = newPos = startPos;
		timePassed = 0;

		while (timePassed < jumpDuration)
		{
			timePassed += Time.deltaTime;
			progress = timePassed / jumpDuration;

			newPos.x = jumpXCurve.Evaluate(progress) * jumpFinalPos.x;
			newPos.y = jumpYCurve.Evaluate(progress) * jumpFinalPos.y;

			rt.anchoredPosition = startPos + newPos;
			rt.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, jumpRotation, progress));

			group.alpha = Mathf.Lerp(1, 0, progress);

			yield return null;
		}

		this.gameObject.SetActive(false);
	}

	public void RemoveAsRight()
	{
		CancelInvoke();
		StartCoroutine(CorrectAnim_Routine());
	}

	private IEnumerator CorrectAnim_Routine()
	{
		float timePassed = 0;
		float progress = 0;
		Vector2 startPos = rt.anchoredPosition;
		Vector2 newPos = Vector2.zero;
		correctIcon.gameObject.SetActive(true);

		while (timePassed < correctIconAnimDuration)
		{
			timePassed += Time.deltaTime;
			progress = timePassed / correctIconAnimDuration;

			correctIcon.color = Color.Lerp(Color.clear, Color.red, progress);
			correctIcon.transform.localScale = Vector3.one * Mathf.Lerp(2, 1, progress);

			yield return null;
		}

		timePassed = 0;
		Vector2 newScale = Vector2.zero;

		while (timePassed < flyUpDuration)
		{
			timePassed += Time.deltaTime;
			progress = timePassed / flyUpDuration;

			newPos.x = flyUpXCurve.Evaluate(progress) * flyUpFinalPos.x;
			newPos.y = flyUpYCurve.Evaluate(progress) * flyUpFinalPos.y;
			rt.anchoredPosition = startPos + newPos;

			newScale.x = 1 + flyUpScaleXCurve.Evaluate(progress);
			newScale.y = 1 + flyUpScaleYCurve.Evaluate(progress);
			rt.localScale = newScale;

			group.alpha = Mathf.Lerp(1, 0, progress);

			yield return null;
		}

		this.gameObject.SetActive(false);
	}
}