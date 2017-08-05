using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour {

	[Header("Walker")]
	public float moveDuration = 0.2f;
	public float smoothTime = 0.1f;
	public float jumpDist = 0.2f;

	Transform _transform;
	Vector3 _currentPos;
	Vector3 _velocity;

	Vector3 _targetPos;
	protected Vector3 targetPos {
		get { return _targetPos; }

		set {
			_targetPos = value;
			StartCoroutine(stepCo());
		}
	}

	protected void setPosition(Vector3 pos) {
		_targetPos = pos;
		_currentPos = pos;
		_transform.localPosition = pos;
	}

	protected virtual void Awake() {
		_transform = transform;
	}

	IEnumerator stepCo() {
		float t = 0;
		while (t < moveDuration) {
			float param = t / moveDuration;
			float jumpParam = Mathf.Sin(param * Mathf.PI);

			Vector3 pos = Vector3.Lerp(_currentPos, _targetPos, param);
			pos += new Vector3(0, jumpDist * jumpParam, 0);
			_transform.localPosition = pos;

			yield return null;
			t += Time.deltaTime;
		}

		_transform.localPosition = _targetPos;
		_currentPos = _targetPos;
	}
}
