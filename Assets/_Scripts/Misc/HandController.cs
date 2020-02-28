using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    public GameObject handPivot;
    public LayerMask collisionMask;
    public float handDistance = 0.5f;

    private Camera cameraMain;

    private bool animating = false;
    private bool isHolding = false;

    public AnimationCurve grabCurve;
    public float timeToGrab = 0.5f;
    public AnimationCurve dropObjectCurve;
    public float timeToDropObject = 0.5f;




	// Use this for initialization
	void Start () {
        cameraMain = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask))
        {

			Debug.DrawRay(cameraMain.transform.position,(hit.point -cameraMain.transform.position) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            if(!animating){
                handPivot.transform.position = cameraMain.transform.position + (hit.point - cameraMain.transform.position) + (hit.point - cameraMain.transform.position).normalized * handDistance ;
            }
            if (Input.GetMouseButtonDown(0) && !animating)
            {
                if(!isHolding){
                    StartCoroutine(grabAnimationRoutine(hit.point));
                    print("Animando");
                }
                else{
                    StartCoroutine(dropObjectAnimationRoutine(hit.point));

                }
            }
        }
        else
        {
            Debug.DrawRay(cameraMain.transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
	}
   
    IEnumerator grabAnimationRoutine(Vector3 hitPos){
        animating = true;
        float index = 0;
        while(index<timeToGrab){
            handPivot.transform.position = cameraMain.transform.position + (hitPos - cameraMain.transform.position) + (hitPos - cameraMain.transform.position).normalized * handDistance * grabCurve.Evaluate(index * 1/timeToGrab);;
            index += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        print("Acabei");
        isHolding = true;
        animating = false;
        yield return null;
    }

    IEnumerator dropObjectAnimationRoutine(Vector3 hitPos){
        animating = true;
        float index = 0;
        while (index < timeToDropObject)
        {
            /*
            Vector3 hitPosTransform = (hitPos - cameraMain.transform.position).normalized * handDistance * grabCurve.Evaluate(index * 1 / timeToDropObject);
            handPivot.transform.position = cameraMain.transform.position + (hitPos - cameraMain.transform.position) + 
                new Vector3 (hitPosTransform.x, ((hitPos - cameraMain.transform.position).normalized * handDistance).y, hitPosTransform.z);
            */
            index += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        print("Acabei");
        isHolding = false;
        animating = false;
        yield return null;
    }

}
