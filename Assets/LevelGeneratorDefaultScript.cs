﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneratorDefaultScript : MonoBehaviour {
	public GameObject unused;
	public GameObject floor;
	public GameObject wall;
	public GameObject door;
	public GameObject corridor;

	int _width, _height;
	List<Tile> _tiles;
	List<Rect> _rooms;
	List<Rect> _exits;

	int randomInt(int exclusiveMax){
		return (int)Random.Range (0, exclusiveMax-1); 
	}
	int randomInt(int min, int max){ //inclusive min/max
		return (int)Random.Range(min, max);
	}
	bool randomBool(double probability = 0.5){
		if (Random.value >= 0.5) {
			return true;
		}
		return false;
	}

	public class Rect{
		public int x, y;
		public int width, height;
	};

	enum Tile{
		Unused,
		Floor,
		Wall,
		Door,
		Corridor
	};

	enum Direction{
		North,
		South,
		East,
		West,
		DirectionCount
	};

	public void Dungeon(int width, int height){
		_width = width;
		_height = height;
		_tiles = new List<Tile>(width*height);
		for(int i = 0; i<_tiles.Count; ++i){
			_tiles.Add(Tile.Unused);
		}
		_rooms = new List<Rect> ();
		_exits = new List<Rect> ();
	}

	void generate(int maxFeatures){

	}

	private Direction randomDirection(){
		int r = (int)Random.Range(1, 4);
		if(r == 1){return Direction.North;}
		if(r == 2){return Direction.South;}
		if(r == 3) {return Direction.East;}
		if(r == 4) {return Direction.West;}
		else{return Direction.North;}
	}

	void print(){

	}

	Tile getTile(int x, int y){
		if (x < 0 || y < 0 || x >= _width || y >= _height) {
			return Tile.Unused;
		}
		return _tiles [x + y * _width];
	}

	void setTile(int x, int y, Tile tile){
		_tiles [x + y * _width] = tile;
	}

	//create features

	bool makeRoom(int x, int y, Direction dir, bool firstRoom = false){
		const int minRoomSize = 3;
		const int maxRoomSize = 6;

		Rect room = new Rect{};
		room.width = randomInt (minRoomSize, maxRoomSize);
		room.height = randomInt (minRoomSize, maxRoomSize);

		if (dir == Direction.North) {
			room.x = x - room.width / 2;
			room.y = y - room.height;
		} else if (dir == Direction.South) {
			room.x = x - room.width / 2;
			room.y = y + 1;
		} else if (dir == Direction.West) {
			room.x = x - room.width;
			room.y = y - room.height / 2;
		} else if (dir == Direction.East) {
			room.x = x + 1;
			room.y = y - room.height / 2;
		}

		if(placeRect(ref room, Tile.Floor)){
			_rooms.Add(room);
			if(dir != Direction.South || firstRoom){
				Rect r = new Rect {};
				r.x = room.x; r.y = room.y - 1; r.width = room.width; r.height = 1;
				_exits.Add(r);
			}
			if (dir != Direction.North || firstRoom) {
				Rect r = new Rect{ };
				r.x = room.x; r.y = room.y + room.height; r.width = room.width; r.height = 1;
				_exits.Add (r);
			}
			if (dir != Direction.East || firstRoom) {
				Rect r = new Rect { };
				r.x = room.x - 1; r.y = room.y; r.width = 1; r.height = room.height;
				_exits.Add (r);
			}
			if (dir != Direction.West || firstRoom) {
				Rect r = new Rect { };
				r.x = room.x + room.width; r.y = room.y; r.width = 1; r.height = room.height;
				_exits.Add (r);
			}

			return true;
		}
		return false; 
	}

	//make corridor
	bool makeCorridor(int x, int y, Direction dir){
		int minCorridorLength = 3;
		int maxCorridorLength = 6;

		Rect corridor = new Rect{};
		corridor.x = x;
		corridor.y = y;

		if (randomBool ()) { //horizontal corridor
			corridor.width = randomInt (minCorridorLength, maxCorridorLength);
			corridor.height = 1;

			if (dir == Direction.North) {
				corridor.y = y - 1;
				if (randomBool ()) {//west
					corridor.x = x - corridor.width + 1;
				}
			} else if (dir == Direction.South) {
				corridor.y = y + 1;
				if (randomBool ()) {// west
					corridor.x = x - corridor.width + 1;
				}
			} else if (dir == Direction.West) {
				corridor.x = x - corridor.width;
			} else if (dir == Direction.East) {
				corridor.x = x + 1;
			}

		} else {//vertical corridor
			corridor.width = 1;
			corridor.height = randomInt (minCorridorLength, maxCorridorLength);

			if (dir == Direction.North) {
				corridor.y = y - corridor.height;
			} else if (dir == Direction.South) {
				corridor.y = y + 1;
			} else if (dir == Direction.West) {
				corridor.x = x - 1;
				if (randomBool ()) {//north
					corridor.y = y - corridor.height + 1;
				}
			} else if (dir == Direction.East) {
				corridor.x = x + 1;
				if (randomBool ()) { //south
					corridor.y = y - corridor.height + 1;
				}
			}

		}
		if (placeRect (ref corridor, Tile.Corridor)) {
			if (dir != Direction.South && corridor.width != 1) {
				Rect r = new Rect { };
				r.x = corridor.x;
				r.y = corridor.y - 1;
				r.width = corridor.width;
				r.height = 1;
				_exits.Add (r);
			}
			if (dir != Direction.North && corridor.width != 1) {
				Rect r = new Rect{ };
				r.x = corridor.x;
				r.y = corridor.y + corridor.height;
				r.width = corridor.width;
				r.height = 1;
				_exits.Add (r);
			}
			if (dir != Direction.East && corridor.height != 1) {
				Rect r = new Rect { };
				r.x = corridor.x - 1;
				r.y = corridor.y;
				r.width = 1;
				r.height = corridor.height;
				_exits.Add (r);
			}
			if (dir != Direction.West || corridor.height != 1) {
				Rect r = new Rect { };
				r.x = corridor.x + corridor.width;
				r.y = corridor.y;
				r.width = 1;
				r.height = corridor.height;
				_exits.Add (r);
			}
			return true;
		}
		return false;
	}

	bool placeRect(ref Rect rect, Tile tile){
		if (rect.x < 1 || rect.y < 1 || rect.x + rect.width > _width - 1 || rect.y + rect.height > _height - 1) {
			return false;
		}
		for (int y = rect.y; y < rect.y + rect.height; ++y) {
			for (int x = rect.x; x < rect.x + rect.width; ++x) {
				if (getTile (x, y) != Tile.Unused) {
					return false; //this area is already used
				}
			}
		}
		for (int y = rect.y - 1; y < rect.y + rect.height + 1; ++y) {
			for (int x = rect.x - 1; x < rect.x + rect.width + 1; ++x) {
				if (x == rect.x - 1 || y == rect.y - 1 || x == rect.x + rect.width || y == rect.y + rect.height) {
					setTile (x, y, Tile.Wall);
				} else {
					setTile (x, y, tile);
				}
			}
		}
		return true;
	}

	bool placeObject(Tile tile){
		if (_rooms.Count < 1) {
			return false;
		}

		int r = randomInt (_rooms.Count);
		int x = randomInt (_rooms [r].x + 1, _rooms [r].x + _rooms [r].width - 2);
		int y = randomInt (_rooms [r].y + 1, _rooms [r].y + _rooms [r].height - 2);

		if (getTile (x, y) == Tile.Floor) {
			setTile (x, y, tile);

			_rooms.RemoveAt (r);
			return true;
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		corridor = Resources.Load ("Prefabs/Environment/Corridor") as GameObject;
		floor = Resources.Load("Prefabs/Environment/Floor") as GameObject;
		wall = Resources.Load("Prefabs/Environment/Wall") as GameObject;
		door = Resources.Load("Prefabs/Environment/Door") as GameObject;
		unused = Resources.Load("Prefabs/Environment/Unused") as GameObject;

		//Dungeon(80, 80)
		//generate(50);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
