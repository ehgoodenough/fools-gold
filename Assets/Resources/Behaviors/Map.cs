using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public struct Coords {
		public int x;
		public int y;

		public Coords(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static Coords operator +(Coords a, Coords b) {
			return new Coords(a.x + b.x, a.y + b.y);
		}

		public static Coords operator -(Coords a, Coords b) {
			return new Coords(a.x - b.x, a.y - b.y);
		}
	}

	public float tileSize = 1;

	private Object tileObject;
	public static Map instance;

	private Hashtable tiles = new Hashtable();
	private Dictionary<Coords, Enemy> _enemies = new Dictionary<Coords, Enemy>();

	// this is just temporary until we have a real map
	const int TEMP_GRID_WIDTH = 10;
	const int TEMP_GRID_HEIGHT = 10;

	void Awake() {
		instance = this;
		tileObject = Resources.Load("Prefabs/Tile") as Object;
	}

	void Start() {
		GetComponent<MapGenerator> ().CreateRoom (7, 7, new Vector3 (-3, -3, 10));
	}

	public void addTile(Vector3 position) {

		// Check if tile already exists
		if (canMoveTo(position)) {
			// Instantiate in the scene.
			Transform parent = this.transform;
			Quaternion rotation = Quaternion.identity;
			Object tile = Object.Instantiate(this.tileObject, position, rotation, parent);

			// Keep track of it in our hashtable.
			this.tiles.Add(position.x + "-" + position.y, tile);
		}
	}

	public void addEnemy(Enemy enemy) {
		_enemies.Add(enemy.currentCoords, enemy);
	}

	public void moveEnemy(Enemy enemy, Coords nextCoords) {
		_enemies.Remove(enemy.currentCoords);
		enemy.currentCoords = nextCoords;
		_enemies.Add(enemy.currentCoords, enemy);
	}

	public Coords getRandomCoords() {
		int x = (int)(Random.value * TEMP_GRID_WIDTH);
		int y = (int)(Random.value * TEMP_GRID_HEIGHT);
		return new Coords(x, y);
	}

	public Vector3 getPosFromCoords(int x, int y) {
		return new Vector3(x * tileSize, y * tileSize);
	}

	public bool canMoveTo(Vector2 position) {
		return this.tiles[position.x + "-" + position.y] == null;
	}

	public bool canMoveTo(Coords coords) {
		if (coords.x < 0 || coords.x >= TEMP_GRID_WIDTH)
			return false;
		if (coords.y < 0 || coords.y >= TEMP_GRID_HEIGHT)
			return false;
		if (canMoveTo(new Vector2(coords.x, coords.y)) == false)
			return false;
		if (_enemies.ContainsKey(coords))
			return false;
		return true;
	}
}
