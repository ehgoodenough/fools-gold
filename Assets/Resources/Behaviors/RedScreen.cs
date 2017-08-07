using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedScreen : MonoBehaviour {

	public static RedScreen instance;

	Renderer _renderer;
	Material _material;
	Color _color;

	public void flash() {
		StopAllCoroutines();
		StartCoroutine(flashCo());
	}

	IEnumerator flashCo() {
		_renderer.enabled = true;
		const float duration = 0.3f;
		float t = 0;

		while (t < duration) {
			float param = t / duration;
			float param2 = 1f - param * param;

			Color c = new Color(_color.r, _color.g, _color.b, param2);
			_material.color = c;

			t += Time.deltaTime;
			yield return null;
		}
		_renderer.enabled = false;
	}

	void Awake() {
		instance = this;
		_renderer = GetComponent<Renderer>();
		_material = _renderer.material;
		_color = _material.color;
		_renderer.enabled = false;
	}
}
