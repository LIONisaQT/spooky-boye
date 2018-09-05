using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour {
	private GameObject player;
	private Vector3 playerPosition;
	private float distanceBetween;
	private bool nearby = false;
	private Canvas prompt;
	private bool switchState = false;
	private DeviceController dc;

	private SpriteRenderer sr;

	public GameObject connectedDevice;
	public int triggerDistance;

	void Start() {
		player = GameObject.Find("Player");
		prompt = GetComponentInChildren<Canvas>();
		sr = GetComponent<SpriteRenderer>();
	} 
	// Added comment to force update
	void Update() {
		// Check if player is nearby
		if (player) {
			playerPosition = player.transform.position;
			distanceBetween = Vector3.Distance(playerPosition, transform.position);
			nearby = distanceBetween < triggerDistance;
		} 
		// If so, display 'press x to interact' prompt
		prompt.enabled = nearby;

		// If interacted, do a thing?
		// (just change color for now?)
		if (Input.GetButtonDown("Fire3") && nearby) {
			dc = connectedDevice.GetComponent<DeviceController>();
			// Switches remain in their last set position
			if (this.tag == "Switch") {
				if (switchState) {
					sr.color = Color.red;
					dc.Deactivate();
				} else {
					sr.color = Color.green;
					dc.Activate();
				}
				switchState = !switchState;
			} 
			// Buttons turn off after 0.5(?) seconds
			else if (this.tag == "Button") {
				StartCoroutine(ButtonPress());
			}
		}
	}

	public void switchToOff() {
		if (switchState) {
			sr.color = Color.red;
			dc.Deactivate();
		}
	}

	IEnumerator ButtonPress() {
		sr.color = Color.green;
		dc.Activate();
		yield return new WaitForSeconds(0.5f);
		sr.color = Color.red;
		dc.Deactivate();
	}
}
