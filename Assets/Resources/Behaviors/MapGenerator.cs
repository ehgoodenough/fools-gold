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
		for (int row = 0; row < height; row++) 
		{
			for (int col = 0; col < width; col++)
			{
				Debug.Log ("Row, Col: " + row + ", " + col);
				if (0 == row || height - 1 == row || 0 == col || width - 1 == col)
				{
					map.addTile (new Vector3 (pos.x + col, pos.y + row, wallZ), Map.Tile.Wall);
				} else {
					map.addTile (new Vector3 (pos.x + col, pos.y + row, wallZ), Map.Tile.Floor);
				}
			}
		}
	}
}
