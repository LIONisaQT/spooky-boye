using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 1.5f;
	private bool facingRight = true;
	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow) && facingRight) {
			FlipX();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && !facingRight) {
			FlipX();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			animator.SetTrigger("Player_Go_Up");
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			animator.SetTrigger("Player_Go_Down");
		}
	}
	void FixedUpdate () {
		var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		transform.position += move * speed * Time.deltaTime;
	}
	void FlipX() {
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}