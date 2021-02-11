using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Pool")]
public class Pool_SO : ScriptableObject
{
	public PoolableObject prefab;
	public int initialSize = 1;
	protected readonly Stack<PoolableObject> available = new Stack<PoolableObject>();

	private GameObject poolSceneObj;

	private void InitializePool()
	{
		//Create a scene object to function as our holder for the objects created
		poolSceneObj = new GameObject($"{name}_Scene");
		DontDestroyOnLoad(poolSceneObj); //Since this pool is a Scriptable object, we can't destroy all created objects when the scene changes

		for (int i = 0; i < initialSize; i++)
		{
			available.Push(Create());
		}
	}

	protected virtual PoolableObject Create()
	{
		PoolableObject obj = Instantiate(prefab);
		return obj;
	}

	public virtual PoolableObject Request()
	{
		if(poolSceneObj == null)
		{
			InitializePool();
		}
		
		//Either get an available object or create one if there's none available.
		PoolableObject obj = available.Count > 0 ? available.Pop() : Create();
		Transform t = obj.transform;
		t.SetParent(poolSceneObj.transform);
		t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		t.localScale = Vector3.one;
		obj.gameObject.SetActive(true);
		obj.OnRequest(this);
		return obj;
	}

	public virtual void Return(PoolableObject obj)
	{
		if (poolSceneObj == null)
		{
			InitializePool();
		}

		//Return the object to the available stack
		obj.OnReturn();
		available.Push(obj);
		obj.gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		available.Clear();
#if UNITY_EDITOR
		DestroyImmediate(poolSceneObj);
#else
		Destroy(poolSceneObj);
#endif
	}
}
