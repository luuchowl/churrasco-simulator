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

	public float currentTime = 5;

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
		StartCoroutine(WrongAnim_Routine());
	}

	private IEnumerator WrongAnim_Routine()
	{
		float timePassed = 0;
		Vector2 startPos = rt.anchoredPosition;
		Vector2 newPos = Vector2.zero;

		while (timePassed < shakeDuration)
		{
			timePassed += Time.deltaTime;
			newPos.x = Mathf.Sin(timePassed * shakeFrequency) * shakeMagnitude;

			rt.anchoredPosition = startPos + newPos;

			yield return null;
		}

		rt.anchoredPosition = newPos = startPos;
		timePassed = 0;

		while (timePassed < jumpDuration)
		{
			timePassed += Time.deltaTime;
			float progress = timePassed / jumpDuration;

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

	}
}