using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDetection : MonoBehaviour {
    private Animator shockAnimator;
	private SpriteRenderer shockSpriteAnimator;

	private GameObject shock;
	// Use this for initialization
	void Start () {
		shock = this.gameObject.transform.GetChild(1).gameObject;
		shockAnimator = shock.GetComponent<Animator>();
		shockSpriteAnimator = shock.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<DeviceController>() != null && other.GetComponent<DeviceController>().isActive()) {
			shockAnimator.enabled = true;		
			shockSpriteAnimator.enabled = true;	
		}
    }

	void OnTriggerExit2D(Collider2D other) {
        shockAnimator.enabled = false;
		shockSpriteAnimator.enabled = false;
    }
}
