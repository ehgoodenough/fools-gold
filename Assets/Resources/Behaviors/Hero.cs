using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	private Vector3 targetPosition = new Vector3();
	private float speed = 10;
	private float deathspin = 22;

	public int maxhealth = 3;
	public int health = 3;
	public int gold = 100;

	private bool isDead = false;

	public static Hero instance;

	void Awake() {
		instance = this;
	}

	void Update() {
		this.isDead = true;
		if(this.isDead) {
			this.transform.Rotate(Vector3.forward * this.deathspin);
			return;
		}

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

		if(Map.instance != null) {
			if(Map.instance.HasGold(this.targetPosition)) {
				Object gold = Map.instance.GetGold(this.targetPosition);
				Object.Destroy(gold);
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
		if(Map.instance == null) {
			return true;
		}

		if(Map.instance.getEnemy(position) != null) {
			Enemy enemy = Map.instance.getEnemy(position);
			enemy.takeDamage(1);

			return false;
		}

		return Map.instance.canMoveTo(position);
	}

	public void takeDamage(int damage) {
		this.health -= damage;
		if(this.health <= 0) {
			this.health = 0;

			this.die();
		}
	}

	private void die() {
		if(this.isDead != false) {
			this.isDead = true;

			// TODO: play death sound.
		}
	}
}
