using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Coords = Map.Coords;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

	public enum Type { Horizontal, Vertical, NoDiagonal, Diagonal, AllDirections, Count, Random }
	enum Direction { Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft, Count }

	[Header("Stats")]
	public int health = 1;

	[Header("Movement")]
	public bool canMoveHorizontal = true;
	public bool canMoveVertical = true;
	public bool canMoveDiagonal = true;

	public float moveInterval = 1;
	public float smoothTime = 0.1f;

	[System.NonSerialized] public Coords currentCoords;

	Transform _transform;
	Vector3 _currentPos;
	Vector3 _targetPos;
	Vector3 _velocity;
	float _timer;
	int _damage = 0;

	List<Coords> _validNeighbors = new List<Coords>();

	Vector3 getCurrentPos() {
		return Map.instance.getPosFromCoords(currentCoords.x, currentCoords.y);
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

	void init() {
		Map.instance.addEnemy(this);

		_transform = transform;
		_targetPos = getCurrentPos();
		_currentPos = _targetPos;

		_transform.localPosition = _currentPos;

		_timer = Random.value * moveInterval;
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
				_validNeighbors.Add(coords);
		}
	}

	void moveToRandomNeighbor() {
		updateValidMoves();
		if (_validNeighbors.Count == 0)
			return; // we currently have nowere to move

		int i = (int) (Random.value * _validNeighbors.Count);
		Map.instance.moveEnemy(this, _validNeighbors[i]);
		_targetPos = getCurrentPos();
	}

	void Update() {
		_timer += Time.deltaTime;

		if (_timer >= moveInterval) {
			moveToRandomNeighbor();
			_timer = 0;
		} else {
			_currentPos = Vector3.SmoothDamp(_currentPos, _targetPos, ref _velocity, smoothTime);
			_transform.localPosition = _currentPos;
		}
	}

	public void takeDamage(int damage) {
		_damage += damage;
		if (_damage >= health) {
			Map.instance.removeEnemy(this);
			Map.instance.CreateGold(this.currentCoords);
			Destroy(gameObject);
		}
	}
}
