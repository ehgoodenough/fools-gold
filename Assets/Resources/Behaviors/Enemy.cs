using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour {

	public float moveInterval = 1;
	public float scale = 5;
	public float smoothTime = 0.1f;

	const int TEMP_GRID_WIDTH = 10;
	const int TEMP_GRID_HEIGHT = 10;
	const float TEMP_CELL_SIZE = 1;

	Transform _transform;
	int _coordX;
	int _coordY;
	Vector3 _currentPos;
	Vector3 _targetPos;
	Vector3 _velocity;
	float _timer;

	Vector3 getPosFromCoords(int x, int y) {
		return new Vector3(_coordX * TEMP_CELL_SIZE, _coordY * TEMP_CELL_SIZE);
	}

	Vector3 getCurrentPos() {
		return getPosFromCoords(_coordX, _coordY);
	}

	void init() {
		_coordX = (int)(Random.value * TEMP_GRID_WIDTH);
		_coordY = (int)(Random.value * TEMP_GRID_HEIGHT);

		_transform = transform;
		_targetPos = getCurrentPos();
		_currentPos = _targetPos;

		float s = scale * TEMP_CELL_SIZE;
		_transform.localPosition = _currentPos;
		_transform.localScale = new Vector3(s, s, s);

		_timer = Random.value * moveInterval;
	}

	void moveToRandomNeighbor() {
		int i = (int) (Random.value * 6);
		switch (i) {
			case 0:
				_coordX--;
				_coordY--;
				break;
			case 1:
				_coordX--;
				break;
			case 2:
				_coordY--;
				break;
			case 3:
				_coordX++;
				_coordY++;
				break;
			case 4:
				_coordX++;
				break;
			case 5:
				_coordY++;
				break;
		}

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
