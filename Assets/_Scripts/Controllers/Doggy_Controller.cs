using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggy_Controller : MonoBehaviour {
	public GameObject doggyObj;
	public float uprightForce = 1;
	public float launchMultiplier = 1;
	public Vector2 minMaxJumpForce;
	public float flipForce = 1;
	public float walkForce = 1;
	[Range(0, 1)] public float rotationSpeed = .5f;
	public AudioClip[] dogBarks;
	public List<Transform> snacksList = new List<Transform>();
	[Header("Debug")]
	public bool launchOnMouseClick;

	private Transform doggyTransform;
	private Rigidbody rb;
	private bool stayUpright;
	private ObjectPool[] pools;
	private Vector3 walkDir;
	
	// Use this for initialization
	void Start () {
		doggyTransform = doggyObj.transform;
		rb = doggyObj.GetComponent<Rigidbody>();
		pools = FindObjectsOfType<ObjectPool>();
		stayUpright = true;

		InvokeRepeating("ChooseRandomWalkDir", 0, 5);
	}

	private void Update()
	{
		if (snacksList.Count > 0 && stayUpright)
		{
			Launch(snacksList[0].transform.position);
		}

		if (launchOnMouseClick && Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				Launch(hit.point);
			}
		}

		if (stayUpright)
		{
			var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
			rb.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightForce);
			rb.AddForce(walkDir * walkForce);

			doggyTransform.forward = Vector3.Lerp(doggyTransform.forward, walkDir, rotationSpeed);
		}
	}

	public void Launch(Vector3 point)
	{
		Vector3 dir = point - doggyTransform.position;
		dir.y = Random.Range(minMaxJumpForce.x, minMaxJumpForce.y);

		transform.forward = dir;

		rb.AddForce(dir * launchMultiplier, ForceMode.Impulse);
		rb.AddRelativeTorque((Vector3.right + (Vector3.up * Random.Range(-1f, 1f))) * flipForce, ForceMode.Impulse);

		stayUpright = false;
		CancelInvoke("BackToUpright");
		Invoke("BackToUpright", 1);

		Sound_Manager.Instance.PlayRandomSFX(dogBarks);
	}

	private void BackToUpright()
	{
		stayUpright = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Stick"))
		{
			Stick_Content c = collision.collider.GetComponent<Stick_Content>();
			for (int i = 0; i < c.ingredientsPivots.Length; i++)
			{
				for (int j = 0; j < pools.Length; j++)
				{
					if (c.ingredientsPivots[i] != null)
					{
						if (snacksList.Contains(collision.transform))
						{
							snacksList.Remove(collision.transform);
						}
						pools[i].ReturnObjectToPool(c.ingredientsPivots[i].gameObject);
					}
				}
			}
		}
		else
		{
			if (collision.rigidbody != null && snacksList.Contains(collision.rigidbody.transform))
			{
				snacksList.Remove(collision.rigidbody.transform);
			}

			for (int i = 0; i < pools.Length; i++)
			{
				pools[i].ReturnObjectToPool(collision.gameObject);
			}
		}
	}

	private void ChooseRandomWalkDir()
	{
		walkDir = Random.insideUnitSphere;
		walkDir.y = 0;
	}
}
