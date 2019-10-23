using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Player_Controller : MonoBehaviour {
	public LayerMask whatToHit;
	[Range(0, 1)]
	public float smoothness;
	public bool showCursor;
	public Transform hand;
	public Animator anim;
	public int screenTransparentArea;
	[Range(0, 1)]
	public float opacityWhenTransparent = .5f;
	public Renderer[] rendToMakeTransparent;
	public GameObject target;

	[BoxTitle("Debug")]
	[HideInInspector] public bool acting;
	[ReadOnly] public Collider hitObject;

	private Camera cam;
	private Vector3 newPos;
	private Color[] startColor;
	private Color[] transparentColor;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		Cursor.visible = showCursor;

		startColor = new Color[rendToMakeTransparent.Length];
		transparentColor = new Color[startColor.Length];

		for (int i = 0; i < rendToMakeTransparent.Length; i++) {
			startColor[i] = rendToMakeTransparent[i].material.GetColor("_Color");
			transparentColor[i] = startColor[i];
			transparentColor[i].a = opacityWhenTransparent;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!acting) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 10, whatToHit)) {
				newPos = hit.point;

				if (hit.collider.tag != "HandCollider") {
					target.SetActive(true);
					hitObject = hit.collider;
				} else {
					target.SetActive(false);
					hitObject = null;
				}
			}

			transform.position = Vector3.Lerp(transform.position, newPos, smoothness);

			for (int i = 0; i < rendToMakeTransparent.Length; i++) {
				if (Input.mousePosition.x < screenTransparentArea) {
					rendToMakeTransparent[i].material.SetColor("_Color", transparentColor[i]);
				} else {
					rendToMakeTransparent[i].material.SetColor("_Color", startColor[i]);
				}
			}
		}
	}
}
