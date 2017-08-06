using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Walker : MonoBehaviour {

	[Header("Walker")]
	public float moveDuration = 0.2f;
	public float smoothTime = 0.1f;
	public float jumpDist = 0.2f;
	public bool flipped = true;

	protected SpriteRenderer _renderer;
	Transform _transform;
	Vector3 _currentPos;
	Vector3 _velocity;

	protected float _zIndex = 0;
	Transform _shadow;
	Vector3 _shadowOffset;

	protected bool _isStepping = false;
	Vector3 _targetPos;
	public Vector3 targetPos {
		get { return _targetPos; }

		protected set {
			if (_isStepping || _targetPos == value)
				return;
			_targetPos = value;
			StartCoroutine(stepCo());
		}
	}

	public void setPosition(Vector3 pos) {
		_targetPos = pos;
		_currentPos = pos;
		_transform.localPosition = pos;
	}

	protected virtual void Awake() {
		_transform = transform;
		_renderer = GetComponent<SpriteRenderer>();
		_shadow = _transform.Find("Shadow");
		_shadowOffset = _shadow.position - _transform.position;
	}

	public float playHitEffect(string prefabName, bool useColor = true, float moveUp = 0.5f) {
		string path = "Prefabs/FX/" + prefabName;
		GameObject prefab = Resources.Load(path) as GameObject;
		ParticleSystem fx = Instantiate(prefab).GetComponent<ParticleSystem>();
		fx.transform.position = _transform.position + Vector3.up * moveUp;
		Destroy(fx.gameObject, fx.main.duration);

		if (useColor) {
			fx.GetComponent<ParticleSystemRenderer>().material.color = _renderer.material.color;
		}

		return fx.main.duration;
	}

	protected void halfStep(Vector3 toPos) {
		_targetPos = toPos;
		StartCoroutine(stepCo(true));
	}

	IEnumerator stepCo(bool halfStep = false) {
		if (_targetPos.x != _currentPos.x) {
			_renderer.flipX = _targetPos.x < _currentPos.x;
			if (flipped)
				_renderer.flipX = !_renderer.flipX;
		}

		_isStepping = true;
		float t = 0;
		while (t < moveDuration) {
			float param = t / moveDuration;
			if (halfStep && param > 0.5f)
				param = 1f - param;

			float jumpParam = Mathf.Sin(param * Mathf.PI);

			Vector3 pos = Vector3.Lerp(_currentPos, _targetPos, param);
			_shadow.position = pos + _shadowOffset;

			pos += new Vector3(0, jumpDist * jumpParam, 0);
			_transform.position = pos;

			yield return null;
			t += Time.deltaTime;
		}

		if (halfStep) {
			_shadow.position = _currentPos + _shadowOffset;
			_transform.position = _currentPos;
			_targetPos = _currentPos;
		} else {
			_shadow.position = _targetPos + _shadowOffset;
			_transform.position = _targetPos;
			_currentPos = _targetPos;
		}
		
		_isStepping = false;
	}

	void modifyZ(Transform t, float z) {
		Vector3 pos = t.position;
		pos.z = z;
		t.position = pos;
	}

	void LateUpdate() {
		// Do the z-indexing off their y position.
		float z = _transform.position.y + _zIndex;
		modifyZ(_transform, z);
		modifyZ(_shadow, z);
	}
}
