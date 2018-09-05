using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour {
    
    void Start() {
    
    }

    void OnCollisionEnter2D(Collision2D col) {
        //Debug.Log("Oh i hit a box");
    }

    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Oh I hear da speaker");
    }
}
