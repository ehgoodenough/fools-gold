using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	public static int numRooms = 0;
	public static Dictionary<int, Room> rooms = new Dictionary<int, Room> ();

	private int uid;
	private Vector2 dim;
	private Vector2 pos;
	private Map.Tile[] tiles;
	private List<int> connections;


	public Room(int width, int height, int posX, int posY)
	{
		this.uid = ++numRooms;
		this.dim = new Vector2 (width, height);
		this.pos = new Vector2 (posX, posY);
		rooms.Add (this.uid, this);
		this.tiles = new Map.Tile [width * height];
		this.connections = new List<int> ();
	}

	// Set tiles in terms of global coordinates
	public void SetTileAt(int x, int y, Map.Tile tileType)
	{
		// Ensure that coorindates passed are within the room
		if (x < this.pos.x || x > this.pos.x + this.dim.x - 1 || y < this.pos.y || y > this.pos.y + this.dim.y - 1) {
			Debug.Log ("Attempting to set tile outside of Room.");
			return;
		}
		// Otherwise, convert the coordinates and set the tile
		int row = y - (int) this.pos.y;
		int col = x - (int) this.pos.x;
		SetTile (row, col, tileType);
	}
	
	// Sets tiles in terms of room's local coordinates
	public void SetTile(int row, int col, Map.Tile tileType)
	{
		if (row < this.dim.y && col < this.dim.x) {
			this.tiles [row * (int)this.dim.x + col] = tileType;
		}
	}

	public Map.Tile GetTileAt(int row, int col)
	{
		return this.tiles [row * (int) this.dim.x + col];
	}

	public void ConnectTo(int otherRoom)
	{
		if (!connections.Contains(otherRoom))
		{
			this.connections.Add (otherRoom);
		}
	}

	public bool IsConnectedTo(int otherRoom)
	{
		return this.connections.Contains (otherRoom);
	}

	public void MovePosition(int deltaX, int deltaY) 
	{
		this.pos = new Vector2 (this.dim.x + deltaX, this.dim.y + deltaY);
	}

	public void SetPosition(int posX, int posY)
	{
		this.pos = new Vector2 (posX, posY);
	}

	public int GetWidth()
	{
		return (int) this.dim.x;
	}

	public int GetHeight()
	{
		return (int) this.dim.y;
	}

	public Vector2 GetDimensions()
	{
		return this.dim;
	}

	public Vector2 GetPosition()
	{
		return this.pos;
	}

	public int GetID()
	{
		return this.uid;
	}

	public int GetNumRooms()
	{
		return numRooms;
	}
}
