using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] LoadSceneEvent_SO loadEvent;
    [SerializeField, Scene] private string[] scenesToLoad;
    [SerializeField] private bool showLoadingScreen;
    public void Raise()
	{
		loadEvent.Raise(showLoadingScreen, scenesToLoad);
	}

	private void OnTriggerEnter(Collider other)
	{
		Raise();
	}
}
