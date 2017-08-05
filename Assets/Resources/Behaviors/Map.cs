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

		public string toKey() {
			return "" + x + "-" + y;
		}
	}

	public enum Tile
	{
		Floor=0,
		Wall=1
	}

	public float tileSize = 1;

	private Object wallObject;
	private Object floorObject;
	private Object enemyObject;
	public static Map instance;

	private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
	private Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();

	// this is just temporary until we have a real map
	const int TEMP_GRID_WIDTH = 10;
	const int TEMP_GRID_HEIGHT = 10;

	void Awake() {
		instance = this;
		wallObject = Resources.Load("Prefabs/Tile") as Object;
		enemyObject = Resources.Load("Prefabs/Enemy") as Object;
	}

	void Start() {
		GetComponent<MapGenerator> ().CreateRoom (7, 7, new Vector3 (-3, -3, 10));

		Object.Instantiate(this.enemyObject, transform);
	}

	public void addTile(Vector3 position, Tile tileType) {

		// Check if tile already exists
		if (canMoveTo(position)) {
			// Instantiate in the scene.
			Transform parent = this.transform;
			Quaternion rotation = Quaternion.identity;

			switch (tileType)
			{
			case Tile.Wall:
				Object.Instantiate (this.wallObject, position, rotation, parent);
				break;
			case Tile.Floor:
				Object.Instantiate (this.floorObject, position, rotation, parent);
				break;
			default:
				break;
			}

			// Keep track of it in our hashtable.
			this.tiles.Add(position.x + "-" + position.y, tileType);
		}
	}

	public void addEnemy(Enemy enemy) {
		_enemies.Add(enemy.currentCoords.toKey(), enemy);
	}

	public void moveEnemy(Enemy enemy, Coords nextCoords) {
		_enemies.Remove(enemy.currentCoords.toKey());
		enemy.currentCoords = nextCoords;
		_enemies.Add(enemy.currentCoords.toKey(), enemy);
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
		return !tiles.ContainsKey( position.x + "-" + position.y) || tiles[position.x + "-" + position.y] != Tile.Wall;
	}

	public bool canMoveTo(Coords coords) {
		if (coords.x < 0 || coords.x >= TEMP_GRID_WIDTH)
			return false;
		if (coords.y < 0 || coords.y >= TEMP_GRID_HEIGHT)
			return false;
		if (canMoveTo(new Vector2(coords.x, coords.y)) == false)
			return false;
		if (_enemies.ContainsKey(coords.toKey()))
			return false;
		return true;
	}

	public Enemy getEnemy(Vector2 position) {
		string key = position.x + "-" + position.y;
		if(_enemies.ContainsKey(key)) {
			return _enemies[key];
		} else {
			return null;
		}
	}
}
