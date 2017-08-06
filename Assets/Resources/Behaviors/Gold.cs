using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {
    private const float Z_INDEX = 0.75f;

	Animator _animator;
	const int NUM_VARIATIONS = 5;

	public void setAmount(int amount) {
		int f = 0;
		if (amount > 1)
			f++;
		if (amount > 2)
			f++;
		if (amount > 3)
			f++;
		if (amount > 5)
			f++;

		float t = 1f * f / NUM_VARIATIONS;
		_animator.Play("GoldVatiations", 0, t);
	}

	void Awake() {
		_animator = GetComponent<Animator>();
	}

    void Update() {
        // Do the z-indexing off their y position.
        Vector3 position = transform.position;
        position.z = position.y + Z_INDEX;
        transform.position = position;
    }
}
