using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	Map map;
	int wallZ = 10;

	public enum Situated
	{
		AboveLeft,
		AboveCenter,
		AboveRight,
		LeftMiddle,
		RightMiddle,
		BelowLeft,
		BelowCenter,
		BelowRight,
		Overlaps
	}

	public enum HAligned
	{
		Left,
		Center,
		Right
	}

	public enum VAligned
	{
		Above,
		Middle,
		Below
	}

	void Start ()
	{
		map = Map.instance;
	}

	// Connects room1 to room2
	public void ConnectRooms(Room r1, Room r2)
	{
		// Connect the room data structure
		r1.ConnectTo (r2.GetID ());
		r2.ConnectTo (r1.GetID ());

		// Create the corridor between the two rooms
	}

	// Creates a corridor between room1 and room2
	public void CreateCorridor(Room r1, Room r2, int corridorWidth)
	{
		// Get the position of each room
		Vector2 r1Pos = r1.GetPosition ();
		Vector2 r2Pos = r2.GetPosition ();

		// Get the dimensions of each room
		Vector2 r1Dim = r1.GetDimensions ();
		Vector2 r2Dim = r2.GetDimensions ();

		if (GetRelativePosition (r1, r2, corridorWidth) == Situated.AboveCenter) {
			int leftLimit = (int) Mathf.Max (r1Pos.x, r2Pos.x);
			int rightLimit = (int) Mathf.Min (r1Pos.x + r1Dim.x - 1, r2Pos.x + r2Dim.x - 1);
			// Debug.Log ("leftLimit: " + leftLimit);
			// Debug.Log ("rightLimit: " + rightLimit);
			int leftPad = (rightLimit - leftLimit - corridorWidth + 1) / 2;
			int rightPad = (rightLimit - leftLimit - corridorWidth + 1) - leftPad;
			// Debug.Log ("leftPad: " + leftPad);
			// Debug.Log ("rightPad: " + rightPad);

			// Create doorway from room1
			for (int i = 0; i < corridorWidth - 2; i++) {
				r1.SetTileAt ((int) (leftLimit + leftPad + 1 + i), (int) (r1Pos.y + r1Dim.y - 1), Map.Tile.Floor);
			}

			// Create doorway to room2
			for (int i = 0; i < corridorWidth - 2; i++) {
				r2.SetTileAt ((int) (leftLimit + leftPad + 1 + i), (int) (r2Pos.y), Map.Tile.Floor);
			}
		}
	}

	public Room CreateRoom(int width, int height, Vector3 pos)
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
		return room;
	}

	public void CreateTiles()
	{
		// Debug.Log ("Creating Tiles...");
		// Debug.Log ("# Rooms: " + Room.rooms.Count);
		foreach (Room room in Room.rooms.Values)
		{
			for (int row = 0; row < room.GetDimensions().y; row++) 
			{
				for (int col = 0; col < room.GetDimensions().x; col++)
				{
					// Debug.Log ("Row, Col: " + row + ", " + col);
					Vector2 pos = room.GetPosition();
					string key = map.getKeyFromPosition (new Vector2 (pos.x + col, pos.y + row));
					// Debug.Log ("Key: " + key);
					if (!map.tiles.ContainsKey (key)) {
						map.addTile (new Vector3 (pos.x + col, pos.y + row, wallZ), room.GetTileAt (row, col));
					} else {
						// Debug.Log ("Key already exists");
					}
				}
			}
		}
	}



	// Tells how r2 is situated relative to r1, given the corridor width
	public Situated GetRelativePosition(Room r1, Room r2, int corridorWidth)
	{
		// Get the position of each room
		Vector2 r1Pos = r1.GetPosition ();
		Vector2 r2Pos = r2.GetPosition ();

		// Get the dimensions of each room
		Vector2 r1Dim = r1.GetDimensions ();
		Vector2 r2Dim = r2.GetDimensions ();

		// Determine the horizontal alignment of room2 relative to room1
		HAligned hAlignment;
		if (r2Pos.x + r2Dim.x < r1Pos.x + corridorWidth)
		{
			hAlignment = HAligned.Left;
		} else if (r2Pos.x > r1Pos.x + r1Dim.x - corridorWidth)
		{
			hAlignment = HAligned.Right;
		} else
		{
			hAlignment = HAligned.Center;
		}

		// Determine the vertical alingmeent of room2 relative to room1
		VAligned vAlignment;
		if (r2Pos.y + r2Dim.y < r1Pos.y + corridorWidth)
		{
			vAlignment = VAligned.Below;
		} else if (r2Pos.y > r1Pos.y + r1Dim.y - corridorWidth)
		{
			vAlignment = VAligned.Above;
		} else
		{
			vAlignment = VAligned.Middle;
		}

		// What a mess...
		if (VAligned.Above == vAlignment) {
			if (HAligned.Left == hAlignment) {
				return Situated.AboveLeft;
			} else if (HAligned.Center == hAlignment) {
				return Situated.AboveCenter;
			} else if (HAligned.Right == hAlignment) {
				return Situated.AboveRight;
			}
		} else if (VAligned.Middle == vAlignment) {
			if (HAligned.Left == hAlignment) {
				return Situated.LeftMiddle;
			} else if (HAligned.Right == hAlignment) {
				return Situated.RightMiddle;
			}
		} else if (VAligned.Below == vAlignment) {
			if (HAligned.Left == hAlignment) {
				return Situated.BelowLeft;
			} else if (HAligned.Center == hAlignment) {
				return Situated.BelowCenter;
			} else if (HAligned.Right == hAlignment) {
				return Situated.BelowRight;
			}
		}

		return Situated.Overlaps;
	}

}
