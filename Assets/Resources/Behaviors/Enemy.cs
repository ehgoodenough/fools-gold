using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Coords = Map.Coords;

public class Enemy : Walker {

	public enum Type { Horizontal, Vertical, NoDiagonal, Diagonal, AllDirections, Count, Random }
	enum Direction { Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft, Count }

	[Header("Stats")]
	public int health = 1;
	public int gold = 1;

	[Header("Movement")]
	public bool canMoveHorizontal = true;
	public bool canMoveVertical = true;
	public bool canMoveDiagonal = true;
	public float moveInterval = 1;
	public int numSteps = 1;
	public float extraStepDelay = 0.1f;

	public float chaseRadius = 0;

	[System.NonSerialized] public Coords currentCoords;

	float _timer;
	int _damage = 0;
	bool _isFlashing = false;
	bool _isFollowing = false;
	Color _defalutColor;
	int _stepCounter = 0;
	SpriteRenderer _eyes;

	bool _isDead = false;
	public bool isAlive { get { return !_isDead; } }

	List<Coords> _validNeighbors = new List<Coords>();

	private const float Z_INDEX = 0.25f;

	Vector3 getCurrentPos() {
		return Map.instance.getPosFromCoords(currentCoords);
	}

	static Coords getMoveFromDir(Direction dir) {
		switch (dir) {
			case Direction.Top:			return new Coords(0, 1);
			case Direction.TopRight:	return new Coords(1, 1);
			case Direction.Right:		return new Coords(1, 0);
			case Direction.BottomRight: return new Coords(1, -1);
			case Direction.Bottom:		return new Coords(0, -1);
			case Direction.BottomLeft:	return new Coords(-1, -1);
			case Direction.Left:		return new Coords(-1, 0);
			case Direction.TopLeft:		return new Coords(-1, 1);
			default:					return new Coords(0, 0);
		}
	}

	public static Enemy getPrototypeFromType(Type type) {
		if (type == Type.Random)
			type = (Type)(Random.value * (float)Type.Count);

		string path = "Prefabs/Enemies/Enemy ";
		switch (type) {
			case Type.Horizontal:		path += "Only Horizontal"; break;
			case Type.Vertical:			path += "Only Vertical"; break;
			case Type.NoDiagonal:		path += "No Diagonal"; break;
			case Type.Diagonal:			path += "Only Diagonal"; break;
			case Type.AllDirections:	path += "All Directions"; break;
			default:					return null;
		}

		GameObject obj = Resources.Load(path) as GameObject;
		return obj.GetComponent<Enemy>();
	}

	public static Enemy create(Coords coords, Type type = Type.Random) {
		if (Map.instance.canMoveTo(coords) == false) {
			Debug.LogError("Unable to place an enemy in coords:" + coords.ToString());
			return null;
		}

		Enemy prototype = getPrototypeFromType(type);
		Enemy newEnemy = Instantiate(prototype, Map.instance.transform);
		newEnemy.currentCoords = coords;
		newEnemy.init();
		return newEnemy;
	}

	void createEyes() {
		GameObject obj = Instantiate(Resources.Load("Prefabs/SkeletonEyes") as GameObject);
		Vector3 scale = obj.transform.localScale;
		obj.transform.SetParent(_transform);
		obj.transform.localPosition = new Vector3(0, 0, -100);
		obj.transform.localScale = scale;
		_eyes = obj.GetComponent<SpriteRenderer>();
	}

	void init() {
		Map.instance.addEnemy(this);
		setPosition(getCurrentPos());
		_timer = Random.value * moveInterval;
		createEyes();
	}

	void updateValidMoves() {
		_validNeighbors.Clear();
		for (int i = 0; i < (int)Direction.Count; i++) {
			Direction dir = (Direction)i;

			if (canMoveHorizontal == false && (dir == Direction.Left || dir == Direction.Right))
				continue;
			if (canMoveVertical == false && (dir == Direction.Bottom || dir == Direction.Top))
				continue;
			if (canMoveDiagonal == false && (dir == Direction.TopLeft || dir == Direction.TopRight))
				continue;
			if (canMoveDiagonal == false && (dir == Direction.BottomLeft || dir == Direction.BottomRight))
				continue;

			Coords move = getMoveFromDir(dir);
			Coords coords = currentCoords + move;

			if (Map.instance.canMoveTo(coords))
			{
				Map.Tile tileType = Map.instance.getTile(new Vector2(coords.x, coords.y));
					if (tileType != Map.Tile.FinalRoomFloor && tileType != Map.Tile.RugEndFloor && tileType != Map.Tile.RugFloor) {
						_validNeighbors.Add(coords);
					}
			}
		}
	}

	bool canDamageHero() {
		for (int i = 0; i < (int)Direction.Count; i++) {
			Direction dir = (Direction)i;
			Coords move = getMoveFromDir(dir);
			Coords coords = currentCoords + move;
			if (Map.instance.isHeroInCoords(coords))
				return true;
		}
		return false;
	}

	void moveToRandomNeighbor() {
		updateValidMoves();
		if (_validNeighbors.Count == 0)
			return; // we currently have nowere to move

		int i = (int) (Random.value * _validNeighbors.Count);
		Map.instance.moveEnemy(this, _validNeighbors[i]);
		targetPos = getCurrentPos();
	}

	void moveToClosestNeighbor(Vector2 heroPos) {
		updateValidMoves();
		if (_validNeighbors.Count == 0)
			return; // we currently have nowere to move

		Coords nextCoords = currentCoords;
		float closestDist = float.MaxValue;
		foreach (Coords coords in _validNeighbors) {
			Vector2 pos = Map.instance.getPosFromCoords(coords);
			float dist = Vector2.Distance(pos, heroPos);
			if (dist < closestDist) {
				closestDist = dist;
				nextCoords = coords;
			}
		}

		Map.instance.moveEnemy(this, nextCoords);
		targetPos = getCurrentPos();
	}

	IEnumerator hitFlashAnimCo() {
		_isFlashing = true;
		const float duration = 0.2f;
		float t = 0;
		while (t < duration) {
			float strength = t / duration;
			strength = 1f - strength * strength;

			_renderer.material.color = Color.Lerp(_defalutColor, Color.white, strength);

			yield return null;
			t += Time.deltaTime;
		}

		_renderer.material.color = _defalutColor;
		_isFlashing = false;
	}

	public void takeDamage(int damage) {
		if (_isFlashing)
			return;
		_isFlashing = true;

		_damage += damage;
		playHitEffect("Bones Splash");

		if (_damage >= health) {
			_isDead = true;
			Hero.instance.audios[3].Play();
			StartCoroutine(deathAnimationCo());
			return;
		}

		_timer = 0; // reset timer for next move
		StartCoroutine(hitFlashAnimCo());
	}

	IEnumerator deathAnimationCo() {
		const float duration = 1;
		float t = 0;
		Vector3 startPos = _transform.position;
		Vector3 startShadowPos = _shadow.position;

		// delay so that particlse can catch up
		yield return new WaitForSeconds(0.2f);

		// replace skelaton sprite with skull
		_renderer.sprite = getSpriteResource("Skull");
		Color color = _renderer.material.color;
		bool goldDropped = false;

		SpriteRenderer shadowSprite = _shadow.GetComponent<SpriteRenderer>();
		Color shadowColor = shadowSprite.material.color;

		while (t < duration) {
			float param = t / duration;
			float param2 = param * param;
			float y = Mathf.Abs(Mathf.Cos(param2 * Mathf.PI * 3)) / (1f + param2 * 5f) - 1;

			Vector3 pos = startPos + new Vector3(0, y, 0);
			color.a = 1f - param2 * param2;
			shadowColor.a = 1f - param;

			_transform.position = pos;
			_shadow.position = startShadowPos;
			_renderer.material.color = color;
			shadowSprite.material.color = shadowColor;

			if (goldDropped == false && param > 0.5f) {
				goldDropped = true;
				Map.instance.CreateGold(currentCoords, gold);
			}

			t += Time.deltaTime;
			yield return null;
		}

		Destroy(gameObject);
	}

	void OnDestroy() {
		Map.instance.removeEnemy(this);
	}

	protected override void Awake() {
		base.Awake();
		Color c = _renderer.material.color;
		_defalutColor = new Color(c.r, c.g, c.b);
		_zIndex = Z_INDEX;
		_attackSprite = getSpriteResource("SkeletonAttack");
	}
	
	void updateEyes() {
		float a = 0;
		if (_isDead == false) {
			a = TorchLight.instance.getLightInPosition(_transform.position);
			a = 1f - a * a;
		}
		_eyes.flipX = _renderer.flipX;
		_eyes.material.color = new Color(1, 1, 1, a);
	}

	void Update() {
		if (_isDead)
			return;

		_timer += Time.deltaTime;

		if(GameManager.instance.IsPaused()) {
			return;
		}

		updateEyes();

		float delay = _stepCounter == 0 ? moveInterval : extraStepDelay;
		if (_timer >= delay) {
			_timer = 0;
			Vector2 pos = targetPos;
			Vector2 heroPos = Hero.instance.targetPos;

			if (canDamageHero()) {
				attack(Hero.instance.targetPos);
				Hero.instance.takeDamage(1);
			} else {
				float distToHero = Vector2.Distance(pos, heroPos);
				_isFollowing = distToHero < chaseRadius;

				_stepCounter = (_stepCounter + 1) % numSteps;
				if (_isFollowing) {
					moveToClosestNeighbor(heroPos);
				} else {
					moveToRandomNeighbor();
				}
			}
		}
	}

	void OnDrawGizmos() {
		if (Application.isPlaying == false)
			return;
		if (chaseRadius == 0)
			return;

		Gizmos.color = _isFollowing ? Color.red : Color.green;
		Gizmos.DrawWireSphere(targetPos, chaseRadius);
	}
}
