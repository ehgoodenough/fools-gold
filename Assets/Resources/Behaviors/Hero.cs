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

	public AudioSource[] audios;

	protected override void Awake() {
		base.Awake();
		instance = this;
		_zIndex = Z_INDEX;
		_attackSprite = getSpriteResource("Hero_Attack");
		audios = GetComponents<AudioSource>();
	}

	public float playCoinsEffect(int amount) {
		string path = "Prefabs/FX/Coins FX";
		GameObject prefab = Resources.Load(path) as GameObject;
		ParticleSystem fx = Instantiate(prefab).GetComponent<ParticleSystem>();
		fx.transform.position = targetPos;
		fx.Emit(amount);
		Destroy(fx.gameObject, fx.main.duration);
		return fx.main.duration;
	}

	void Update() {
		if(isDead) {
			//_transform.Rotate(Vector3.forward * this.deathspin);
			return;
		}

		if(GameManager.instance.IsPaused()) {
			return;
		}

		// Poll the keyboard.
		if (_isStepping == false) {
			Vector3 stepDir = new Vector3();
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
				stepDir += Vector3.up;
			}
			if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
				stepDir += Vector3.down;
			}
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
				stepDir += Vector3.left;
			}
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
				stepDir += Vector3.right;
			}

			if (canMoveTo(targetPos + stepDir)) {
				targetPos += stepDir;
				if(stepDir != Vector3.zero) {
					audios[5].Play();
				}
			}
		}

		if (Map.instance != null) {
			if(Map.instance.HasGold(targetPos)) {
				int amount = Map.instance.GetGolds(targetPos).Count;
				this.gold += amount;
				Map.instance.RemoveGold(targetPos, 0.1f);
				playCoinsEffect(amount);
				audios[2].Play();
			}
		}

		if(this.health == 1) {
			if(!audios[6].isPlaying) {
				audios[6].Play();
			}
		} else {
			audios[6].Stop();
		}
	}

	private bool canMoveTo(Vector3 position) {
		if(Map.instance == null) {
			// Debug.Log ("Map.instance == null");
			return true;
		}

		Enemy enemy = Map.instance.getEnemy(position);
		if (enemy != null && enemy.isAlive) {
			attack(enemy.targetPos);
			enemy.takeDamage(1);
			// audios[1].pitch = Random.Range(0.5f, 1.0f);
			audios[1].Play();
			return false;
		}

		// The "end position" is now the scammer's position.
		if(position.x == Map.instance.endPosition.x
		&& position.y == Map.instance.endPosition.y) {
			attack(position);
			Debug.Log("Talking to the scammer!");
            int REQUIRED_GOLD = GameManager.instance.GoldNeeded();
            if (this.gold >= REQUIRED_GOLD) {
				this.gold -= REQUIRED_GOLD;
				this.isDone = true;
			} else {
                GameManager.instance.ShowNotEnoughGoldDialogue();
			}
		}

		return Map.instance.canMoveTo(position);
	}

	public void takeDamage(int damage) {
		if (health <= 0)
			return;

		health -= damage;
		playHitEffect("Blood Splash", false);
		audios[0].Play();
		RedScreen.instance.flash();

		if (health <= 0) {
			health = 0;
			die();
		}
	}

	IEnumerator deathAnimationCo() {
		yield return new WaitForSeconds(0.2f);

		playHitEffect("Blood Splash", false);
		yield return new WaitForSeconds(0.2f);

		playHitEffect("Blood Splash", false);
		yield return new WaitForSeconds(0.2f);

		playHitEffect("Blood Splash", false);
		_renderer.enabled = false;
		_shadow.gameObject.SetActive(false);
		Enemy enemy = Enemy.create(new Map.Coords(targetPos), forced: true);
		enemy.setSpriteFlipX(getSpriteFlipX());
		yield return new WaitForSeconds(0.2f);

		playHitEffect("Blood Splash", false);
	}

	private void die() {
		if(isDead != true) {
			isDead = true;
			StartCoroutine(deathAnimationCo());
			audios[4].Play();
		}
	}
}
