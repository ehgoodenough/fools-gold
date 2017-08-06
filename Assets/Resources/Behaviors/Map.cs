﻿using System.Collections;
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

	private const float TILE_Z_INDEX = 0.9f;
	public float tileSize = 1;

	private Object wallObject;
	private Object floorObject;
	private Object goldObject;

	public static Map instance;

	public Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
	private Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();
	private Dictionary<string, List<Object>> gold = new Dictionary<string, List<Object>>();

	public Vector2 endPosition;

	// Temp
	void createSomeRandomEnemies() {
		Enemy.create(new Coords(1, 1));
		Enemy.create(new Coords(2, 2));
		Enemy.create(new Coords(3, 3));
		Enemy.create(new Coords(4, 4));
	}

	void Awake() {
		instance = this;
		wallObject = Resources.Load("Prefabs/Wall") as Object;
		floorObject = Resources.Load("Prefabs/Floor") as Object;
		goldObject = Resources.Load("Prefabs/Gold") as Object;
	}

	void Start() {
		// Debug.Log ("Map.Start()");
		// Debug.Log ("Tiles: " + tiles.Count);
		MapGenerator mapGen = GetComponent<MapGenerator> ();
		Room hubRoom = mapGen.CreateRoom (17, 11, new Vector3 (-8, -5, 10));
		Room neRoom = mapGen.CreateRoom (6, 6, new Vector3 (4, 6, 10));
		Debug.Log ("Room 2 is " + mapGen.GetRelativePosition (hubRoom, neRoom, 5) + " relative to Room 1.");
		mapGen.CreateCorridor (hubRoom, neRoom, 4);
		Room nwRoom = mapGen.CreateRoom (11, 10, new Vector3 (-14, 6, 10));
		mapGen.CreateCorridor (hubRoom, nwRoom, 5);
		Room swRoom = mapGen.CreateRoom (8, 6, new Vector3 (-10, -11, 10));
		mapGen.CreateCorridor (swRoom, hubRoom, 4);
		Room seRoom = mapGen.CreateRoom (10, 4, new Vector3 (-1, -10, 10));
		mapGen.CreateCorridor (seRoom, hubRoom, 10);
		Room nCorridor = mapGen.CreateRoom (5, 12, new Vector3 (-2, 6, 10));
		mapGen.CreateCorridor (hubRoom, nCorridor, 5);
		Room finalRoom = mapGen.CreateRoom (9, 8, new Vector3 (-4, 17, 10));
		mapGen.CreateCorridor (nCorridor, finalRoom, 5);
		mapGen.CreateTiles ();
		createSomeRandomEnemies();

		// ...Just for debugging the end logic. Thanks!!
		this.endPosition = new Vector2(2, 2);
	}

	public void addTile(Vector3 position, Tile tileType) {

		// Check if tile already exists
		if (canMoveTo(position)) {
			// Instantiate in the scene.
			Transform parent = this.transform;
			Quaternion rotation = Quaternion.identity;

			position.z = position.y + TILE_Z_INDEX;

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

	public Vector3 getPosFromCoords(Coords coords) {
		return new Vector3(coords.x * tileSize, coords.y * tileSize);
	}

	public string getKeyFromPosition(Vector2 position) {
		return position.x + "-" + position.y;
	}

	public bool canMoveTo(Vector2 position) {
		string key = getKeyFromPosition(position);
		return !tiles.ContainsKey(key) || tiles[key] != Tile.Wall;
	}

	public bool isHeroInCoords(Coords coords) {
		if (Hero.instance == false)
			return false;
		Coords heroCoords = new Coords(Hero.instance.targetPos);
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

	public Tile getTile(Vector2 position) {
		string key = position.x + "-" + position.y;
		if(tiles.ContainsKey(key)) {
			return tiles[key];
		} else {
			return Tile.Floor;
		}
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

		string key = position.x + "-" + position.y;

		if(this.gold.ContainsKey(key) == false) {
			this.gold.Add(key, new List<Object>());
		}

		gold[key].Add(g);
	}

	public void CreateGold(Coords position) {
		CreateGold(new Vector2(position.x, position.y));
	}

	public bool HasGold(Vector2 position) {
		string key = position.x + "-" + position.y;
		return gold.ContainsKey(key)
			&& gold[key].Count > 0;
	}

	public List<Object> GetGolds(Vector2 position) {
		return this.gold[position.x + "-" + position.y];
	}

	public void RemoveGold(Vector2 position) {
		List<Object> golds = GetGolds(position);
		foreach(Object gold in golds) {
			Object.Destroy(gold);
		}
		golds.Clear();
	}
}
