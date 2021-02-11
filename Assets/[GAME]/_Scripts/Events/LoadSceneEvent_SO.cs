using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Custom/Load Scene Event")]
public class LoadSceneEvent_SO : ScriptableObject
{
	public event Action<bool, string[]> loadEvent;

	public void Raise(bool showLoadingScreen, params string[] scenesToLoad)
	{
		loadEvent?.Invoke(showLoadingScreen, scenesToLoad);
	}
}
