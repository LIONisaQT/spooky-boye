using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatrol : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    //public Transform pathPoints;
    public float waitTime;

    private int currentSpot;

    public bool loop;    

    private bool facingRight = true;
    private Animator animator;

    private FieldOfView fovScript;

    // Use this for initialization
    void Start()
    {
        currentSpot = 0;
        //StartCoroutine(FollowPath(pathPoints));
		animator = GetComponent<Animator>();

        fovScript = GetComponent<FieldOfView>();
    }

  
    public void gotoPoint(Vector3 point) {
        StartCoroutine(TurnToFace(point));
        transform.position = point;
    }
   
    // IEnumerator FollowPath(Transform path){
    //     while(true){
    //         Vector2 targetPoint = path.GetChild(currentSpot).position;
    //         transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);



    //         if (Vector2.Distance(transform.position, targetPoint) < 0.4f){
    //             if (loop) {
    //                 currentSpot = (currentSpot + 1) % path.childCount;
    //             } else {
    //                 currentSpot += 1;
    //             }
    //             if (currentSpot >= path.childCount) break;

    //             targetPoint = path.GetChild(currentSpot).position;


    //             yield return new WaitForSeconds(waitTime);
    //             yield return StartCoroutine(TurnToFace(targetPoint));
    //         }
    //         yield return null;
    //     }
    // }

    IEnumerator TurnToFace(Vector3 lookTarget){
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float angle = dirToFloatAngle(dirToLookTarget);
        fovScript.setMagicRotationConstant(angle);
        if (dirToLookTarget.x > 0 && !facingRight && (Mathf.Abs(dirToLookTarget.x) >= Mathf.Abs(dirToLookTarget.y))) {
            animator.SetTrigger("Popo_Right");
            FlipX();
        } else if (dirToLookTarget.x < 0 && facingRight && (Mathf.Abs(dirToLookTarget.x) >= Mathf.Abs(dirToLookTarget.y))) {
            animator.SetTrigger("Popo_Right");
            FlipX();
        } else if (dirToLookTarget.y > 0 ) {
            animator.SetTrigger("Popo_Up");
        } else if (dirToLookTarget.y < 0 ) {
            animator.SetTrigger("Popo_Down");
        }
        yield return null;
    }

    // void OnDrawGizmos()
    // {
    //     Vector2 startPosition = pathPoints.GetChild(0).position;
    //     Vector2 prevPosition = startPosition;

    //     foreach (Transform waypoint in pathPoints){
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawSphere(waypoint.position, .2f);
    //         Gizmos.DrawLine(prevPosition, waypoint.position);
    //         prevPosition = waypoint.position;
    //     }
    //     if (loop) {
    //         Gizmos.DrawLine(prevPosition, startPosition);
    //     }
       
    // }

    float dirToFloatAngle(Vector3 dir) {
        // -90 because Idk where this extra 90deg came from
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    void FlipX() {
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}
