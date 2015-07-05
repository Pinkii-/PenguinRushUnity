﻿using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float jumptime = 0.4f;
	public float dist = 4.5f;
	public Vector2 center = new Vector2(-7,-0.15f);

	public float rotationForce = 1;

	private Vector2 movement = new Vector2(0,0);
	private float gravity = 0f;

	private enum dir {
		up, down, none
	};
	private dir lastJump = dir.none;

	private float obstacleSpeed=0;

	public void setObstacleSpeed(float s) {
		obstacleSpeed = s;
	}

	void Awake() {
		transform.position = center;
	}

	// Update is called once per frame
	void Update () {
		if (lastJump == dir.none) {
			int inputY = 0;
			if (Input.GetAxis("Vertical") > 0) {
				inputY = 1;
				lastJump = dir.up;
			}
			else if (Input.GetAxis("Vertical") < 0) {
				inputY = -1;
				lastJump = dir.down;
			}
			if (inputY != 0) {
				float factor = 2f/(jumptime * jumptime);
				gravity = - factor * ((dist + center.y) - transform.position.y);
				movement.y = - gravity * jumptime;
				movement.y *= inputY;
			}
		}
	}

	void FixedUpdate() {
		// Gravity
		if (transform.position.y > center.y) movement.y += gravity*Time.deltaTime;
		else movement.y -= gravity*Time.deltaTime;
		// Crossing the middle
		if (transform.position.y > center.y && transform.position.y + movement.y*Time.deltaTime < center.y
		    || transform.position.y < center.y && transform.position.y + movement.y*Time.deltaTime > center.y) {
			movement.y *= 0.5f;
			lastJump = dir.none;
		}
		// Horizontal Speed
		movement.x = center.x - transform.position.x;
		// Speed
		GetComponent<Rigidbody2D>().velocity = movement;

		// Agular
		float angularDirection;
		angularDirection = transform.rotation.z;
		if (transform.rotation.z > 0) angularDirection = Mathf.Max (0.2f, Mathf.Min(0.6f ,angularDirection));
		else angularDirection = Mathf.Min (-0.2f, Mathf.Max( -0.6f, angularDirection));
		angularDirection = - Mathf.Pow(angularDirection*100,2) * Mathf.Sign(angularDirection);
		// jump Rotation
		float jumpRotation = 180 - Mathf.Rad2Deg * Mathf.Atan2(movement.y,(movement.x-obstacleSpeed));
		float auxRotation = transform.rotation.eulerAngles.z;
		//jumpRotation = unityRotation(jumpRotation-auxRotation);
		GetComponent<Rigidbody2D>().angularVelocity = 
			(rotationForce * angularDirection * 0.5f) + 
			GetComponent<Rigidbody2D>().angularVelocity*0.5f;
			//+ (jumpRotation - auxRotation) * 20;
	}

	private float unityRotation(float degrees) {
		if (degrees < 180) return degrees/180;
		else return -(1 - degrees/180);
	}
}