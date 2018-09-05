using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWalls : MonoBehaviour {
 	public GameObject prefab;

	// Use this for initialization
	void Start () {
		for (int x = -80; x < 80; x += 20) {
 			GameObject wall = (GameObject)Instantiate(prefab);
            wall.transform.position = new Vector3(x, 50, 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
