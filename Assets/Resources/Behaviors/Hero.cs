using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour {
	private Vector3 _targetPosition = new Vector3();
	public Vector2 targetPosition { get { return _targetPosition; } }
	private float speed = 10;

	private float deathspin = 22;
	private double deathtimer = 1.5;

	public int maxhealth = 3;
	public int health = 3;
	public int gold = 100;

	private bool isDead = false;

	public static Hero instance;

	void Awake() {
		instance = this;
	}

	void Update() {
		if(this.isDead) {
			this.transform.Rotate(Vector3.forward * this.deathspin);

			this.deathtimer -= Time.deltaTime;
			if(this.deathtimer <= 0) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
			return;
		}

		// Poll the keyboard.
		if(Input.GetKeyDown("up")) {
			if(this.canMoveTo(this._targetPosition + Vector3.up)) {
				this._targetPosition += Vector3.up;
			}
		}
		if(Input.GetKeyDown("down")) {
			if(this.canMoveTo(this._targetPosition + Vector3.down)) {
				this._targetPosition += Vector3.down;
			}
		}
		if(Input.GetKeyDown("left")) {
			if(this.canMoveTo(this._targetPosition + Vector3.left)) {
				this._targetPosition += Vector3.left;
			}
		}
		if(Input.GetKeyDown("right")) {
			if(this.canMoveTo(this._targetPosition + Vector3.right)) {
				this._targetPosition += Vector3.right;
			}
		}

		if(Map.instance != null) {
			if(Map.instance.HasGold(this._targetPosition)) {
				Object gold = Map.instance.GetGold(this._targetPosition);
				Object.Destroy(gold);
			}
		}

		// Move the position to the target position.
		float step = Vector3.Distance(this.transform.position, this._targetPosition) * this.speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetPosition, step);

		// If we've reached out target position, then stop moving.
		if(Vector3.Distance(this.transform.position, this._targetPosition) < 0.01) {
			this.transform.position = this._targetPosition;
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
		if(this.isDead != true) {
			this.isDead = true;

			// TODO: play death sound.
		}
	}
}
