using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour {
	void Update () {
		if(Hero.instance == null) {
			return;
		}

		Vector3 position = Hero.instance.transform.position;
		position.z = -1;
		this.transform.position = position;
	}
}
