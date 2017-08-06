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
		Wall,
		GoldWall,
		VineWall,
		CrackedFloor,
		BloodyFloor
	}
			
	private const float TILE_Z_INDEX = 0.9f;
	public float tileSize = 1;

	private Object wallObject;
	private Object floorObject;
	private Object goldObject;
	private Object goldWallObject;
	private Object vineWallObject;
	private Object crackedFloorObject;
	private Object bloodyFloorObject;

	public static Map instance;

	public Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
	private Dictionary<string, Enemy> _enemies = new Dictionary<string, Enemy>();
	private Dictionary<string, List<Gold>> gold = new Dictionary<string, List<Gold>>();

	public Vector2 endPosition;
	public int numGoldOnMap;

	void Awake() {
		instance = this;
		wallObject = Resources.Load("Prefabs/Wall") as Object;
		floorObject = Resources.Load("Prefabs/Floor") as Object;
		goldObject = Resources.Load("Prefabs/Gold") as Object;
		goldWallObject = Resources.Load("Prefabs/GoldWall") as Object;
		vineWallObject = Resources.Load("Prefabs/VineWall") as Object;
		crackedFloorObject = Resources.Load("Prefabs/CrackedFloor") as Object;
		bloodyFloorObject = Resources.Load("Prefabs/BloodyFloor") as Object;
	}

	void Start() {
		// Debug.Log ("Map.Start()");
		Debug.Log ("GameManager.currentLevel: " + GameManager.currentLevel);
		numGoldOnMap = 0;
		GetComponent<MapGenerator> ().GenerateLevel(GameManager.currentLevel);
		createSomeRandomEnemies();
		setHeroStartPosition ();
		setScammerStartPosition ();
		// ...Just for debugging the end logic. Thanks!!
		// this.endPosition = new Vector2(0, 21);
	}

	void createSomeRandomEnemies() {
		foreach (Room room in Room.rooms.Values)
		{
			int posX = (int) room.GetPosition ().x;
			int posY = (int) room.GetPosition ().y;
			int width = (int) room.GetDimensions ().x;
			int height = (int) room.GetDimensions ().y;

			// NOTE: Ignoring the start and end rooms
			if (Room.GetStart() == room || Room.GetEnd() == room) {
				continue;
			}

			// Generate a number of enemies proportionate to the size of the room
			int avgSpawnDist = 7 - GameManager.currentLevel;
			int numEnemies = Mathf.CeilToInt(room.GetInnerArea() / (float) (avgSpawnDist * avgSpawnDist));
			// Debug.Log ("room Area: " + room.GetInnerArea());
			Debug.Log ("numEnemies: " + numEnemies);

			for (int i = 0; i < numEnemies; i++) {
				int attempts = 0;
				int maxAttempts = 10;
				Enemy enemyCreated = null;
				while (null == enemyCreated && maxAttempts > attempts)
				{
					attempts++;

					int randPosX = posX + Random.Range (1, width - 1);
					int randPosY = posY + Random.Range (1, height - 1);

					enemyCreated = Enemy.create (new Coords (randPosX, randPosY));
					if (enemyCreated) {
						Debug.Log ("Enemy Created!");
						numGoldOnMap++;
					}
				}
			}
			Debug.Log ("numGoldOnMap: " + numGoldOnMap);
		}
	}

	public void createSomeRandomGold() {
		// TODO: Create gold randomly along the walls and in corners
	}

	public void setHeroStartPosition() {
		Room startRoom = Room.GetStart ();
		int x = (int) startRoom.GetPosition().x + Mathf.FloorToInt (startRoom.GetWidth () / 2f);
		int y = (int) startRoom.GetPosition().y + Mathf.FloorToInt (startRoom.GetHeight () / 2f);
		Hero.instance.setPosition ( new Vector3 (x, y, y));
		GameObject camera = GameObject.Find ("Camera");
		camera.transform.position = new Vector3 (x, y, camera.transform.position.z);
	}

	public void setScammerStartPosition() {
		Room endRoom = Room.GetEnd ();
		int x = (int) endRoom.GetPosition().x + Mathf.FloorToInt (endRoom.GetWidth () / 2f);
		int y = (int) endRoom.GetPosition().y + Mathf.FloorToInt (endRoom.GetHeight () / 2f);
		GameObject scammer = GameObject.Find ("Scammer");
		scammer.transform.position = new Vector3 (x, y, y);
		Vector3 scammerPos = scammer.transform.position;
		endPosition = new Vector2 (scammerPos.x, scammerPos.y);
		// Andrew says: "I'm repurposing the "end position" as the position of the scammer."
	}

	public void addTile(Vector3 position, Tile tileType) {

		// Check if tile already exists
		string key = getKeyFromPosition(position);
		if (!tiles.ContainsKey(key)) {
			// Instantiate in the scene.
			Transform parent = this.transform;
			Quaternion rotation = Quaternion.identity;

			switch (tileType)
			{
			case Tile.Wall:
				position.z = position.y + TILE_Z_INDEX;
				Object.Instantiate (this.wallObject, position, rotation, parent);
				break;
			case Tile.Floor:
				position.z = 900;
				Object.Instantiate (this.floorObject, position, rotation, parent);
				break;
			case Tile.CrackedFloor:
				position.z = 900;
				Object.Instantiate (this.crackedFloorObject, position, rotation, parent);
				break;
			case Tile.BloodyFloor:
				position.z = 900;
				Object.Instantiate (this.bloodyFloorObject, position, rotation, parent);
				break;
			case Tile.GoldWall:
				position.z = position.y + TILE_Z_INDEX;
				Object.Instantiate (this.goldWallObject, position, rotation, parent);
				break;
			case Tile.VineWall:
				position.z = position.y + TILE_Z_INDEX;
				Object.Instantiate (this.vineWallObject, position, rotation, parent);
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
		if (!tiles.ContainsKey (key)) {
			return true;
		} else if (tiles [key] == Tile.Floor || tiles [key] == Tile.CrackedFloor || tiles [key] == Tile.BloodyFloor) {
			return true;
		} else {
			return false;
		}
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

	public void CreateGold(Coords coords, int amount) {
		Vector2 position = new Vector2(coords.x, coords.y);
		
		// Debug.Log ("Creating Gold...");
		Transform parent = this.transform;
		Quaternion rotation = Quaternion.identity;
		GameObject obj = Instantiate(goldObject, position, rotation, parent) as GameObject;
		Gold g = obj.GetComponent<Gold>();
		g.setAmount(amount);

		string key = position.x + "-" + position.y;

		if (gold.ContainsKey(key) == false) {
			gold.Add(key, new List<Gold>());
		}

		for (int i = 0; i < amount; i++) {
			gold[key].Add(g);
		}
	}

	public bool HasGold(Vector2 position) {
		string key = position.x + "-" + position.y;
		return gold.ContainsKey(key)
			&& gold[key].Count > 0;
	}

	public List<Gold> GetGolds(Vector2 position) {
		return gold[position.x + "-" + position.y];
	}

	public void RemoveGold(Vector2 position, float delay = 0) {
		List<Gold> golds = GetGolds(position);
		foreach(Gold gold in golds) {
			Destroy(gold.gameObject, delay);
		}
		golds.Clear();
	}
}
