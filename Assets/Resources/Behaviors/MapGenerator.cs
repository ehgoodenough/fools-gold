using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	Map map;
	int wallZ = 10;

	void Start ()
	{
		map = Map.instance;
	}
	
	public void CreateRoom(int width, int height, Vector3 pos)
	{
		// Debug.Log ("Creating Room...");
		Room room = new Room (width, height, (int) pos.x, (int) pos.y);
		for (int row = 0; row < height; row++) 
		{
			for (int col = 0; col < width; col++)
			{
				// Debug.Log ("Row, Col: " + row + ", " + col);
				if (0 == row || height - 1 == row || 0 == col || width - 1 == col)
				{
					room.SetTile (row, col, Map.Tile.Wall);
				} else {
					room.SetTile (row, col, Map.Tile.Floor);
				}
			}
		}
	}

	public void CreateTiles()
	{
		// Debug.Log ("Creating Tiles...");
		foreach (Room room in Room.rooms.Values)
		{
			for (int row = 0; row < room.GetDimensions().y; row++) 
			{
				for (int col = 0; col < room.GetDimensions().x; col++)
				{
					// Debug.Log ("Row, Col: " + row + ", " + col);
					Vector2 pos = room.GetPosition();
					map.addTile (new Vector3 (pos.x + col, pos.y + row, wallZ), room.GetTileAt (row, col));
				}
			}
		}
	}

}
