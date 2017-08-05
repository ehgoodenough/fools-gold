using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

	public static int numRooms;
	public static Dictionary<int, Room> rooms;

	private int uid;
	private Vector2 dim;
	private Vector2 pos;
	private List<int> connections;

	public Room(int width, int height, int posX, int posY)
	{
		this.uid = ++numRooms;
		this.dim = new Vector2 (width, height);
		this.pos = new Vector2 (posX, posY);
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
