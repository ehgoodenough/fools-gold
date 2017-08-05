using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour {
	Transform _transform;

	void Awake() {
		_transform = transform;
	}

	void Update () {
		if(Hero.instance == null) {
			return;
		}

		Vector3 position = Hero.instance.transform.position;
		position.z = -1;
		_transform.position = position;
	}
}
