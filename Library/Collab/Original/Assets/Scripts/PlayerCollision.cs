using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
    void OnCollisionEnter (Collision collision) {
        Debug.Log(collision.gameObject.name);
		if (collision.gameObject.name == "Player") {
            Debug.Log("Do something here");
        }

    }
}
