using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolableObject : MonoBehaviour
{
	public Pool_SO pool { get; protected set; }
	public Action onRequest;
	public Action onReturn;

	public virtual void OnRequest(Pool_SO _pool)
	{
		pool = _pool;
		onRequest?.Invoke();
	}

	public virtual void OnReturn()
	{
		onReturn?.Invoke();
	}

	public void ReturnToPool()
	{
		pool.Return(this);
	}
}
