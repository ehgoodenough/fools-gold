using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {


	private Object tileObject;
	public static Map instance;

	void Awake() {
		instance = this;
		this.tileObject = Resources.Load("Prefabs/Tile") as Object;
	}

	void Start() {
		this.addTile(new Vector2(-1, -1));
	}

	void addTile(Vector2 position) {
		Transform parent = this.transform;
		Quaternion rotation = Quaternion.identity;
		Object.Instantiate(this.tileObject, position, rotation, parent);
		// TODO: Add more logic here for pulling an image from a tileset.
	}

	public bool canMoveTo() {
		return true;
	}
}
