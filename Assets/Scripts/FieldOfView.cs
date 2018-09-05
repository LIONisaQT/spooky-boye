using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Pathfinding;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float meshResolution;

    public List<Transform> visibleTargets = new List<Transform>();

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    // Angle offset from transform.right
    // This works because we never rotate the gameobject anymore
    // so use magicRotationConstant to account for sprite moving in different directions
    private float magicRotationConstant;

    private Vector2 v2;

    private AIDestinationSetter aids;

    public GameObject gameController;

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", .1f);
        aids = GetComponent<AIDestinationSetter>();
        gameController = GameObject.FindWithTag("GameController");
    }

    void Update() {
        DrawFieldOfView();
        v2 = transform.position;
    }

    public void setMagicRotationConstant(float angle) {
        magicRotationConstant = angle;
    }

    IEnumerator FindTargetsWithDelay(float delay){
        while (true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    Vector2 CalculateForwardAngle() {
        return DirFromAngle(magicRotationConstant, false);
    }


    void FindVisibleTargets(){
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(v2, viewRadius, targetMask, -10, 10);
        //Debug.Log(targetsInViewRadius.Length);
        for (int i = 0; i < targetsInViewRadius.Length; i++){
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            Vector2 dirFacing = CalculateForwardAngle();
            float dstToTarget = Vector2.Distance(transform.position, target.position);

            // Music is independent of sight cone
            if (!Physics2D.Raycast(v2, dirToTarget, dstToTarget, obstacleMask)) {
                if (target.tag == "Distraction" && target.name == "speaker noise") {
                    DeviceController distractionControlScript = target.transform.parent.gameObject.GetComponent<DeviceController>();
                    Vector2 distractionControlLocation = distractionControlScript.inputDevice.transform.position;
                    Debug.Log("Found Distraction; controller is at: " + distractionControlLocation);
                    aids.noticedDistraction(distractionControlLocation, distractionControlScript);
                }
            }

            if(Vector2.Angle(dirFacing ,dirToTarget) < viewAngle/2){
                if(!Physics2D.Raycast(v2, dirToTarget, dstToTarget, obstacleMask)){
                    if (target.tag == "Robot") {
                        Debug.Log("Found Robot");
                        gameController.GetComponent<GameController>().showGameover();
                    } else if (target.tag == "Distraction" && target.name == "lighted area") {
                        DeviceController distractionControlScript = target.transform.parent.gameObject.GetComponent<DeviceController>();
                        Vector2 distractionControlLocation = distractionControlScript.inputDevice.transform.position;
                        Debug.Log("Found Distraction; controller is at: " + distractionControlLocation);
                        aids.noticedDistraction(distractionControlLocation, distractionControlScript);
                        //distractionControlScript.Deactivate();
                    }
                }

            }
        }
    }

    void DrawFieldOfView() {
        int stepCount =  Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.z + magicRotationConstant - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2)*3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i+1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2) {
                triangles[i*3] = i+2;
                triangles[i*3+1] = i+1;
                triangles[i*3+2] = 0;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal){
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

	ViewCastInfo ViewCast(float globalAngle) {
		Vector3 dir = DirFromAngle (globalAngle, true);
        Vector2 dir2 = dir;
        v2 = transform.position;
		RaycastHit2D[] hit = new RaycastHit2D[1];
        ContactFilter2D cf2d = new ContactFilter2D();
        cf2d.useLayerMask = true;
        cf2d.layerMask = obstacleMask;

        if (Physics2D.Raycast (v2, dir2, cf2d, hit, viewRadius) > 0) {

			return new ViewCastInfo (true, hit[0].point, hit[0].distance, globalAngle);
		} else {
			return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

    public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}
}