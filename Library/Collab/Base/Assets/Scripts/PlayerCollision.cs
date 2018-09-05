using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
    void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.name == "Player") {
            Debug.Log("Do something here");
        }

    }
}
