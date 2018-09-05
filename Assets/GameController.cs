using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject gameover;
	public GameObject victory;

	public void showGameover() {
		gameover.SetActive(true);
	}

	public void showVictory() {
		victory.SetActive(true);
	}
}
