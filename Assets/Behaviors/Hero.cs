using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	private Vector3 targetPosition = new Vector3();
	private float speed = 10;

	void Update() {

		// Poll the keyboard.
		if(Input.GetKeyDown("up")) {
			if(this.canMoveTo(this.targetPosition + Vector3.up)) {
				this.targetPosition += Vector3.up;
			}
		}
		if(Input.GetKeyDown("down")) {
			if(this.canMoveTo(this.targetPosition + Vector3.down)) {
				this.targetPosition += Vector3.down;
			}
		}
		if(Input.GetKeyDown("left")) {
			if(this.canMoveTo(this.targetPosition + Vector3.left)) {
				this.targetPosition += Vector3.left;
			}
		}
		if(Input.GetKeyDown("right")) {
			if(this.canMoveTo(this.targetPosition + Vector3.right)) {
				this.targetPosition += Vector3.right;
			}
		}

		// Move the position to the target position.
		float step = Vector3.Distance(this.transform.position, this.targetPosition) * this.speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, step);

		// If we've reached out target position, then stop moving.
		if(Vector3.Distance(this.transform.position, this.targetPosition) < 0.01) {
			this.transform.position = this.targetPosition;
		}
	}

	private bool canMoveTo(Vector3 position) {
		if(position.x < -3) {
			return false;
		} else {
			return true;
		}
	}
}
