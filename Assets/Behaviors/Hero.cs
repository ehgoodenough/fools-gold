using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	private Vector3 targetPosition = new Vector3();
	private float speed = 10;

	void Update() {

		if(Input.GetKeyDown("up")) {
			this.targetPosition += Vector3.up;
		}
		if(Input.GetKeyDown("down")) {
			this.targetPosition += Vector3.down;
		}
		if(Input.GetKeyDown("left")) {
			this.targetPosition += Vector3.left;
		}
		if(Input.GetKeyDown("right")) {
			this.targetPosition += Vector3.right;
		}

		float step = Vector3.Distance(this.transform.position, this.targetPosition) * this.speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, step);
	}
}
