using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public static Map instance;

	void Awake() {
		instance = this;
	}

	private Texture2D texture;

	void Start() {
		texture = Resources.Load("Images/square") as Texture2D;
		Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
	}

	public bool canMoveTo() {
		return true;
	}
}
