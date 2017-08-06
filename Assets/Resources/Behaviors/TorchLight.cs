using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour {

	public float fadeTotalTime = 100;

	Transform _transform;
	Material _material;
	float _startTime;
	float _dist0;
	float _dist1;
	int _distance0ID;
	int _distance1ID;

	void Awake() {
		_transform = transform;
		_material = GetComponent<Renderer>().material;

		_distance0ID = Shader.PropertyToID("_Distance0");
		_distance1ID = Shader.PropertyToID("_Distance1");
		_dist0 = _material.GetFloat(_distance0ID);
		_dist1 = _material.GetFloat(_distance1ID);

		GameManager.onGameStart += reset;
	}

	void reset() {
		_startTime = Time.time;
		_material.SetFloat(_distance0ID, _dist0);
		_material.SetFloat(_distance1ID, _dist1);
	}

	void Update () {
		if(Hero.instance == null) {
			return;
		}

		Vector3 position = Hero.instance.transform.position;
		position.z = -50;
		_transform.position = position;

		// fade torch by time
		float param = 1f - Mathf.InverseLerp(_startTime, _startTime + fadeTotalTime, Time.time);
		param = Mathf.Clamp(param, 0.2f, 1);
		_material.SetFloat(_distance0ID, _dist0 * param);
		_material.SetFloat(_distance1ID, _dist1 * param);
	}
}
