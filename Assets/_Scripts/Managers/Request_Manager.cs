using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Request_Manager : MonoBehaviour {
	public string url;
	public int timeout;

	public delegate void requestCallback(string response);
	public event Action requestMadeAction;
	public event Action requestDoneAction;

	private IEnumerator Get_Routine(UriBuilder uri, requestCallback callback, WWWForm form = null) {
		yield return null;
	}
}
