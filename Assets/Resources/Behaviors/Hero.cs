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
	ParticleSystem _blood;

	protected override void Awake() {
		base.Awake();
		instance = this;
		_blood = GetComponentInChildren<ParticleSystem>();
	}

	void Update() {
		if(this.isDead) {
			this.transform.Rotate(Vector3.forward * this.deathspin);
			return;
		}

		// Poll the keyboard.
		if(Input.GetKey("up")) {
			if(canMoveTo(targetPos + Vector3.up)) {
				targetPos += Vector3.up;
			}
		}
		if(Input.GetKey("down")) {
			if(canMoveTo(targetPos + Vector3.down)) {
				targetPos += Vector3.down;
			}
		}
		if(Input.GetKey("left")) {
			if(canMoveTo(targetPos + Vector3.left)) {
				targetPos += Vector3.left;
			}
		}
		if(Input.GetKey("right")) {
			if(canMoveTo(targetPos + Vector3.right)) {
				targetPos += Vector3.right;
			}
		}

		if(Map.instance != null) {
			if(Map.instance.HasGold(targetPos)) {
				this.gold += Map.instance.GetGolds(targetPos).Count;
				Map.instance.RemoveGold(targetPos);
			}
			if(this._targetPosition.x == Map.instance.endPosition.x
			&& this._targetPosition.y == Map.instance.endPosition.y) {
				if(this.isDone != true) {
					this.isDone = true;
					Debug.Log("DONE");
				}
			}
		}

		/*
		// Move the position to the target position.
		float step = Vector3.Distance(this.transform.position, this._targetPosition) * this.speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetPosition, step);

		// If we've reached out target position, then stop moving.
		if(Vector3.Distance(this.transform.position, this._targetPosition) < 0.01) {
			this.transform.position = this._targetPosition;
		}
		*/

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
			enemy.takeDamage(1);

			return false;
		}

		return Map.instance.canMoveTo(position);
	}

	public void takeDamage(int damage) {
		if (health <= 0)
			return;

		health -= damage;
		_blood.Play();

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
