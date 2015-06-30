﻿using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public Vector2 speed = new Vector2(50,20);
	
	public float gravity = 2f;
	public float center = -0.15f;

	private Vector2 movement = new Vector2(0,0);
	private bool jumping = false;
//	private bool directionUp;

	// Update is called once per frame
	void Update () {
		if (transform.position.y < center + 1 && transform.position.y > center -1) {
			jumping = false;
		}
		else jumping = true;
		if (!jumping) {
			int inputY = 0;
			if (Input.GetAxis("Vertical") > 0) inputY = 1;
			else if (Input.GetAxis("Vertical") < 0) inputY = -1;
			movement = new Vector2(0 , speed.y * inputY);
			jumping = (inputY != 0);
			//directionUp = (inputY == 1);
			print(inputY);
		}
		// Gravity
		if (transform.position.y > center) movement.y -= gravity - Time.deltaTime;
		else movement.y += gravity - Time.deltaTime;
	}

	void FixedUpdate() {
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}
