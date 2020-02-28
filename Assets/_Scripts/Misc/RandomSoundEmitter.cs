using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundEmitter : MonoBehaviour
{
	public Vector2 minMaxDelay;
	public AudioClip[] clips;

	private void OnEnable()
	{
		StopAllCoroutines();
		StartCoroutine(RandomCanSound_Routine());
	}

	private IEnumerator RandomCanSound_Routine()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minMaxDelay.x, minMaxDelay.y));

			Sound_Manager soundManager = Sound_Manager.Instance;
			soundManager.PlayRandomSFX(clips);
		}
	}
}
