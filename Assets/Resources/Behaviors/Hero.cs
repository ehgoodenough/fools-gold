using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : Walker {
	//private Vector3 _targetPosition = new Vector3();
	//public Vector2 targetPosition { get { return _targetPosition; } }
	//private float speed = 10;

	private float deathspin = 22;

	public int maxhealth = 3;
	public int health = 3;
	public int gold = 0;

	public bool isDead = false;
	public bool isDone = false;

	public static Hero instance;

	private const float Z_INDEX = 0.5f;

	protected override void Awake() {
		base.Awake();
		instance = this;
	}

	void Update() {
		if(this.isDead) {
			this.transform.Rotate(Vector3.forward * this.deathspin);
			return;
		}

		if(this.isDone) {
			return;
		}

		// Poll the keyboard.
		if (_isStepping == false) {
			Vector3 stepDir = new Vector3();
			if (Input.GetKey("up")) {
				stepDir += Vector3.up;
			}
			if (Input.GetKey("down")) {
				stepDir += Vector3.down;
			}
			if (Input.GetKey("left")) {
				stepDir += Vector3.left;
			}
			if (Input.GetKey("right")) {
				stepDir += Vector3.right;
			}

			if (canMoveTo(targetPos + stepDir)) {
				targetPos += stepDir;
			}
		}

		if (Map.instance != null) {
			if(Map.instance.HasGold(targetPos)) {
				this.gold += Map.instance.GetGolds(targetPos).Count;
				Map.instance.RemoveGold(targetPos);
			}
			if(this.targetPos.x == Map.instance.endPosition.x
			&& this.targetPos.y == Map.instance.endPosition.y) {
				if(this.isDone != true) {
					this.isDone = true;
					Debug.Log("DONE");
				}
			}
		}

		// Do the z-indexing off their y position.
		Vector3 position = transform.position;
		position.z = position.y + Z_INDEX;
		transform.position = position;
	}

	private bool canMoveTo(Vector3 position) {
		if(Map.instance == null) {
			return true;
		}

		if(Map.instance.getEnemy(position) != null) {
			Enemy enemy = Map.instance.getEnemy(position);
			halfStep(enemy.targetPos);
			enemy.takeDamage(1);

			return false;
		}

		return Map.instance.canMoveTo(position);
	}

	public void takeDamage(int damage) {
		if (health <= 0)
			return;

		health -= damage;
		playHitEffect("Blood Splash", false);

		if (health <= 0) {
			health = 0;
			die();
		}
	}

	private void die() {
		if(this.isDead != true) {
			this.isDead = true;

			// TODO: play death sound.
		}
	}
}
