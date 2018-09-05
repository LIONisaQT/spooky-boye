using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour {
	public GameObject inputDevice;
	public Animator animator;
	private bool active = false;
	private AudioSource audioSource;
	private AudioSource background;

	public void Start() {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		background = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<AudioSource>();
	}
	// Called when the speaker is turned on
	public void Activate() {
		if (active) return;
		active = true;
		if (this.tag == "Speaker") {
			animator.enabled = true;
			audioSource.mute = false;
			background.mute = true;
		} else if (this.tag == "Cone Light") {
			Debug.Log("Light ON");
		} else if (this.tag == "Radial Light") {
			Debug.Log("Radial Light ON");
		}
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}

	public bool isActive() {
		return active;
	}

	public void Deactivate() {
		if (!active) return;
		active = false;
		if (this.tag == "Speaker") {
			animator.enabled = false;
			audioSource.mute = true;
			background.mute = false;
		} else if (this.tag == "Cone Light") {
			Debug.Log("Light OFF");
		} else if (this.tag == "Radial Light") {
			Debug.Log("Radial Light OFF");
		}
		inputDevice.GetComponent<InputHandler>().switchToOff();
		gameObject.transform.GetChild(0).gameObject.SetActive(false);
	}

	public Vector2 getInputDeviceLocation() {
		return inputDevice.transform.position;
	}
}
