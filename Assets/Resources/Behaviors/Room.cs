using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	public static int numRooms = 0;
	public static Dictionary<int, Room> rooms = new Dictionary<int, Room> ();
	private static Room startRoom;
	private static Room endRoom;

	private int uid;
	private Vector2 pos;
	private Vector2 dim;
	private int area;
	private Map.Tile[] tiles;
	private List<int> connections;
	private bool start;
	private bool end;

	public Room(int width, int height, int posX, int posY)
	{
		this.uid = ++numRooms;
		this.pos = new Vector2 (posX, posY);
		this.dim = new Vector2 (width, height);
		this.area = width * height;
		rooms.Add (this.uid, this);
		this.tiles = new Map.Tile [width * height];
		this.connections = new List<int> ();
		this.start = false;
		this.end = false;
	}

	public Room GetRoom(int uid)
	{
		if (rooms.ContainsKey (uid))
		{
			return rooms [uid];
		}
		return null;
	}

	public static Room GetStart()
	{
		return startRoom;
	}

	public static Room GetEnd()
	{
		return endRoom;
	}

	public void DesignateStart()
	{
		startRoom = this;
		this.start = true;
	}

	public void DesignateEnd()
	{
		endRoom = this;
		this.end = true;
		for (int i = 0; i < this.tiles.Length; i++)
		{
			if (i > (this.GetWidth() * 2) + 1 && i < (this.GetWidth() * 3) - 2)
			{
				this.tiles [i] = Map.Tile.RugEndFloor;
			}
			else if (i > (this.GetWidth() * 3) + 1 && i < this.GetWidth() * this.GetHeight() - (this.GetWidth() * 2) - 2)
			{
				if (i % this.GetWidth() > 1 && i % this.GetWidth() < this.GetWidth() - 2) {
					this.tiles [i] = Map.Tile.RugFloor;
				}
			}
		}
	}

	public bool IsStart()
	{
		return this.start;
	}

	public bool IsEnd()
	{
		return this.end;
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

	public int GetInnerArea()
	{
		return (GetWidth () - 2) * (GetHeight () - 2);
	}

	public int GetTotalArea()
	{
		return GetWidth () * GetHeight ();
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
