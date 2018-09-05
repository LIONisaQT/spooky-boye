using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoPath : MonoBehaviour {
    public Transform pathPoints;
    public float speed;
    public float turnSpeed;
    //public Transform pathPoints;
    public float waitTime;

    private int currentSpot = 0;

    public bool loop;    

    private bool facingRight = true;
    private Animator animator;

    private FieldOfView fovScript;
	// Use this for initialization
	void Start () {
		StartCoroutine(FollowPath(pathPoints));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FollowPath(Transform path){
        while(true){
            Vector2 targetPoint = path.GetChild(currentSpot).position;
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);



            if (Vector2.Distance(transform.position, targetPoint) < 0.4f){
                if (loop) {
                    currentSpot = (currentSpot + 1) % path.childCount;
                } else {
                    currentSpot += 1;
                }
                if (currentSpot >= path.childCount) break;

                targetPoint = path.GetChild(currentSpot).position;


                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

	void OnDrawGizmos()
    {
        Vector2 startPosition = pathPoints.GetChild(0).position;
        Vector2 prevPosition = startPosition;

        foreach (Transform waypoint in pathPoints){
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(waypoint.position, .2f);
            Gizmos.DrawLine(prevPosition, waypoint.position);
            prevPosition = waypoint.position;
        }
        if (loop) {
            Gizmos.DrawLine(prevPosition, startPosition);
        }
       
    }

    float dirToFloatAngle(Vector3 dir) {
        // -90 because Idk where this extra 90deg came from
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
