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

		public Coords(Vector2 pos) {
			this.x = (int)pos.x;
			this.y = (int)pos.y;
		}

		public static Coords operator +(Coords a, Coords b) {
			return new Coords(a.x + b.x, a.y + b.y);
		}

		public static Coords operator -(Coords a, Coords b) {
			return new Coords(a.x - b.x, a.y - b.y);
		}

		public override string ToString() {
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
	private Object goldObject;

	public static Map instance;

	private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
	private Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();
	private Dictionary<string, Object> gold = new Dictionary<string, Object>();

	// Temp
	void createSomeRandomEnemies() {
		Enemy.create(new Coords(1, 1));
		Enemy.create(new Coords(2, 2));
		Enemy.create(new Coords(3, 3));
		Enemy.create(new Coords(4, 4));
	}

	void Awake() {
		instance = this;
		wallObject = Resources.Load("Prefabs/Tile") as Object;
		goldObject = Resources.Load("Prefabs/Gold") as Object;
	}

	void Start() {
		GetComponent<MapGenerator> ().CreateRoom (17, 11, new Vector3 (-8, -5, 10));
		createSomeRandomEnemies();
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
		_enemies.Add(enemy.currentCoords.ToString(), enemy);
	}

	public void moveEnemy(Enemy enemy, Coords nextCoords) {
		_enemies.Remove(enemy.currentCoords.ToString());
		enemy.currentCoords = nextCoords;
		_enemies.Add(enemy.currentCoords.ToString(), enemy);
	}

	public void removeEnemy(Enemy enemy) {
		_enemies.Remove(enemy.currentCoords.ToString());
	}

	public Coords getRandomCoords() {
		int x = (int)(Random.value * 15) - 8;
		int y = (int)(Random.value * 9) - 5;
		return new Coords(x, y);
	}

	public Vector3 getPosFromCoords(int x, int y) {
		return new Vector3(x * tileSize, y * tileSize);
	}

	string getKeyFromPosition(Vector2 position) {
		return position.x + "-" + position.y;
	}

	public bool canMoveTo(Vector2 position) {
		string key = getKeyFromPosition(position);
		return !tiles.ContainsKey(key) || tiles[key] != Tile.Wall;
	}

	public bool isHeroInCoords(Coords coords) {
		if (Hero.instance == false)
			return false;
		Coords heroCoords = new Coords(Hero.instance.targetPosition);
		return heroCoords.Equals(coords);
	}

	public bool canMoveTo(Coords coords) {
		if (canMoveTo(new Vector2(coords.x, coords.y)) == false)
			return false;
		if (_enemies.ContainsKey(coords.ToString()))
			return false;
		if (isHeroInCoords(coords)) 
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


	public void CreateGold(Vector2 position) {
		Transform parent = this.transform;
		Quaternion rotation = Quaternion.identity;
		Object g = Object.Instantiate(goldObject, position, rotation, parent);

		gold.Add(position.x + "-" + position.y, g);
	}

	public void CreateGold(Coords position) {
		CreateGold(new Vector2(position.x, position.y));
	}

	public bool HasGold(Vector2 position) {
		return gold.ContainsKey(position.x + "-" + position.y);
	}

	public Object GetGold(Vector2 position) {
		return gold[position.x + "-" + position.y];
	}
}
