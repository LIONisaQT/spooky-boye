using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeLightController : MonoBehaviour {
	[Range(0.0f, 360.0f)]
	public float rotation;

	void Start () {
		transform.Rotate(new Vector3(0, 0, rotation));
	}
}
