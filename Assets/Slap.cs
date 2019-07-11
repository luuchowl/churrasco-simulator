using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap : MonoBehaviour {

    private Player_Controller hc;
    public float TimeToSlap = 1;
    public AnimationCurve AnimCurve;
    public LayerMask layerMask;

	// Use this for initialization
	void Start () {
        hc = GetComponent<Player_Controller>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0) && !hc.acting){
            RaycastHit hit;
            Ray ray = new Ray(this.transform.position, Vector3.down);
            Debug.DrawRay(this.transform.position, Vector3.down);
            if(Physics.Raycast(this.transform.position, Vector3.down, out hit, 1f, layerMask)){
				ToSlap(hit.transform.gameObject);
            }
        }
	}

    public void ToSlap(GameObject hit){
        StartCoroutine(SlapRoutine(hit));
    }

    IEnumerator SlapRoutine(GameObject hit){
        yield return null;
        hc.acting = true;
        hc.anim.SetBool("Slap", true);

        float index = 0;
        Vector3 positionOrigin = transform.position;
        float yHand = hit.transform.position.y;
        //Vector3 handOrigin;
        while(index<1){

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(positionOrigin.y, yHand,AnimCurve.Evaluate(index)), transform.position.z);
            index += Time.deltaTime/TimeToSlap;
            yield return new WaitForEndOfFrame();
        }


        hc.anim.SetBool("Slap", false);
        hc.acting = false;

    }
}
