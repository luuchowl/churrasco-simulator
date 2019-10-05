using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Request_Manager : Singleton<Request_Manager> {
	public delegate void CallbackDelegate(string response);
	public UnityEvent requestMade = new UnityEvent();
	public UnityEvent requestDone = new UnityEvent();

	public void AddQueryParameter(ref UriBuilder uri, string name, object value) {
		if (uri.Query != null && uri.Query.Length > 1)
			uri.Query = string.Format("{0}&{1}={2}", uri.Query.Substring(1), name, value); //We use Substring(1) because the "Query" parameter of the builder includes the "?" character at the start of the string
		else
			uri.Query = string.Format("{0}={1}", name, value);
	}

	public IEnumerator GET_Routine(UriBuilder uri, CallbackDelegate callback) {
		requestMade.Invoke();

		Debug.Log(string.Format("GET Request URL: {0}", uri.Uri));
		UnityWebRequest request = UnityWebRequest.Get(uri.Uri);

		using (request) {
			request.timeout = 10;

			yield return request.SendWebRequest();

			if (!request.isHttpError || !request.isNetworkError) {
				callback(request.downloadHandler.text);
			} else {
				Debug.LogError("ERROR:" + request.error);
				callback(null);
			}
		}

		requestDone.Invoke();
	}

	public IEnumerator POST_Routine(UriBuilder uri, WWWForm form, CallbackDelegate callback) {
		requestMade.Invoke();

		Debug.Log(string.Format("POST Request URL: {0}", uri.Uri));

		WWW request = new WWW(uri.Uri.ToString(), form);

		yield return request;

		if (string.IsNullOrEmpty(request.error)) {
			callback(request.text);
		} else {
			Debug.LogError("ERROR:" + request.error);
			callback(null);
		}

		requestDone.Invoke();
	}
}
