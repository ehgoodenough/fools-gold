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
		// Create bottom wall
		for (int i = 0; i < width; i++) {
			map.addTile (new Vector3 (pos.x + i, pos.y + 0, wallZ));
		}

		// Create top wall
		for (int i = 0; i < width; i++) {
			map.addTile (new Vector3 (pos.x + i, pos.y + height-1, wallZ));
		}

		// Create left wall
		for (int i = 1; i < height-1; i++) {
			map.addTile (new Vector3 (pos.x + 0, pos.y + i, wallZ));
		}

		// Create right wall
		for (int i = 1; i < height-1; i++) {
			map.addTile (new Vector3 (pos.x + width-1, pos.y + i, wallZ));
		}

	}
}
