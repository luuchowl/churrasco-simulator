using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpeechBubble_Content : MonoBehaviour {
	public TextMeshProUGUI text;
	public Image image;
	public Animator anim;
	[HideInInspector] public ObjectPool ownerPool;

	public void Play(float duration) {
		StartCoroutine(PlayAndReturnToPool_Routine(duration));
	}

	private IEnumerator PlayAndReturnToPool_Routine(float duration) {
		anim.Play("FadeIn");
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

		yield return new WaitForSeconds(duration);

		anim.Play("FadeOut");
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

		ownerPool.ReturnObjectToPool(this.gameObject);
	}
}
