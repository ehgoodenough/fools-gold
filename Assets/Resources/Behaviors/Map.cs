using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {


	private Object tileObject;
	public static Map instance;

	private Hashtable tiles = new Hashtable();

	void Awake() {
		instance = this;
		tileObject = Resources.Load("Prefabs/Tile") as Object;
	}

	void Start() {
		this.addTile(new Vector3(-1, -1, 10));
		this.addTile(new Vector3(-1, 0, 10));
		this.addTile(new Vector3(-1, 1, 10));
		this.addTile(new Vector3(-1, 2, 10));
		this.addTile(new Vector3(0, -1, 10));
		this.addTile(new Vector3(1, -1, 10));
		this.addTile(new Vector3(2, -1, 10));
	}

	void addTile(Vector3 position) {

		// Instantiate in the scene.
		Transform parent = this.transform;
		Quaternion rotation = Quaternion.identity;
		Object tile = Object.Instantiate(this.tileObject, position, rotation, parent);

		// Keep track of it in our hashtable.
		this.tiles.Add(position.x + "-" + position.y, tile);
	}

	public bool canMoveTo(Vector2 position) {
		return this.tiles[position.x + "-" + position.y] == null;
	}
}
