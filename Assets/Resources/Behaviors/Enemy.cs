using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

	public struct Coords {
		public int x;
		public int y;

		public Coords (int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static Coords operator + (Coords a, Coords b) {
			return new Coords(a.x + b.x, a.y + b.y);
		}
	}

	public float moveInterval = 1;
	public float scale = 5;
	public float smoothTime = 0.1f;

	const int TEMP_GRID_WIDTH = 10;
	const int TEMP_GRID_HEIGHT = 10;
	const float TEMP_CELL_SIZE = 1;

	Transform _transform;
	Coords _currentCoords;
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

	Vector3 getPosFromCoords(int x, int y) {
		return new Vector3(x * TEMP_CELL_SIZE, y * TEMP_CELL_SIZE);
	}

	Vector3 getCurrentPos() {
		return getPosFromCoords(_currentCoords.x, _currentCoords.y);
	}

	void init() {
		int x = (int)(Random.value * TEMP_GRID_WIDTH);
		int y = (int)(Random.value * TEMP_GRID_HEIGHT);
		_currentCoords = new Coords(x, y);

		_transform = transform;
		_targetPos = getCurrentPos();
		_currentPos = _targetPos;

		float s = scale * TEMP_CELL_SIZE;
		_transform.localPosition = _currentPos;
		_transform.localScale = new Vector3(s, s, s);

		_timer = Random.value * moveInterval;
	}

	bool isValidCoords(Coords coords) {
		if (coords.x < 0 || coords.x >= TEMP_GRID_WIDTH)
			return false;
		if (coords.y < 0 || coords.y >= TEMP_GRID_HEIGHT)
			return false;
		return true;
	}

	void updateValidMoves() {
		_validNeighbors.Clear();
		foreach (Coords move in _possibleMoves) {
			Coords coords = _currentCoords + move;
			if (isValidCoords(coords))
				_validNeighbors.Add(coords);
		}
	}

	void moveToRandomNeighbor() {
		updateValidMoves();
		int i = (int) (Random.value * _validNeighbors.Count);
		_currentCoords = _validNeighbors[i];
		_targetPos = getCurrentPos();
	}

	void Awake() {
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
}
