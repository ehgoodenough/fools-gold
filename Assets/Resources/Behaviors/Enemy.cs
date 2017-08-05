using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Coords = Map.Coords;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

	public float moveInterval = 1;
	public float scale = 5;
	public float smoothTime = 0.1f;

	[System.NonSerialized] public Coords currentCoords;

	Transform _transform;
	Vector3 _currentPos;
	Vector3 _targetPos;
	Vector3 _velocity;
	float _timer;

	static readonly Coords[] _possibleMoves = new Coords[] {
		new Coords(-1, -1),
		new Coords(-1,  0),
		new Coords(-1,  1),
		new Coords( 0, -1),
		new Coords( 0,  1),
		new Coords( 1, -1),
		new Coords( 1,  0),
		new Coords( 1,  1)
	};
	List<Coords> _validNeighbors = new List<Coords>();

	Vector3 getCurrentPos() {
		return Map.instance.getPosFromCoords(currentCoords.x, currentCoords.y);
	}

	void init() {
		// find a random unoccupied coords for this enemy
		bool foundValidCoords = false;
		while (foundValidCoords == false) {
			currentCoords = Map.instance.getRandomCoords();
			foundValidCoords = Map.instance.canMoveTo(currentCoords);
		}
		Map.instance.addEnemy(this);

		_transform = transform;
		_targetPos = getCurrentPos();
		_currentPos = _targetPos;

		float s = scale * Map.instance.tileSize;
		_transform.localPosition = _currentPos;

		_timer = Random.value * moveInterval;
	}

	void updateValidMoves() {
		_validNeighbors.Clear();
		foreach (Coords move in _possibleMoves) {
			Coords coords = currentCoords + move;

			if (Map.instance.canMoveTo(coords))
				_validNeighbors.Add(coords);
		}
	}

	void moveToRandomNeighbor() {
		updateValidMoves();
		int i = (int) (Random.value * _validNeighbors.Count);
		Map.instance.moveEnemy(this, _validNeighbors[i]);
		_targetPos = getCurrentPos();
	}

	void Start() {
		init();
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
		// TODO: take more than 1 damage.

		Map.instance.removeEnemy(this);
		Object.Destroy(this.gameObject);
	}
}
