using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovementTest : MonoBehaviour
{
    public Transform model;
    public float speed = 1;
    public int pixelsPerUnit = 100;
    public float animDuration;
    public AnimationCurve curve;
    public float degPerSec = 1;

    private Camera cam;
    private Vector3 startPos;
    private float targetY;
    private float startY;
    private float t;

    // Start is called before the first frame update
    void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		cam = Camera.main;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position;

		if (Input.GetMouseButtonDown(1))
		{
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetY = -hit.distance;
                startY = 0;
                t = 0;
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(targetY != -hit.distance)
				{
                    t = 0;
                    targetY = -hit.distance;
                    startY = model.transform.localPosition.y;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
		{
            targetY = 0;
            startY = model.transform.localPosition.y;
            t = 0;
		}

        float rotDir = Input.GetAxisRaw("Mouse ScrollWheel");

        if (rotDir != 0)
		{
            model.transform.Rotate(Vector3.forward, degPerSec * Time.deltaTime * rotDir);
		}

        Vector3 delta = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

        if (delta.sqrMagnitude > 0)
        {
            newPos += (delta / (float)pixelsPerUnit) * speed;
        }

        t += Time.deltaTime;

		model.transform.localPosition = Vector3.up * Mathf.LerpUnclamped(startY, targetY, curve.Evaluate(t / animDuration));

		transform.position = newPos;
    }


}
