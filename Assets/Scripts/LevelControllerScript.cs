﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelControllerScript : MonoBehaviour {
	//controls the generation and control of individual levels
	//number of tiles this level supports
	GameObject unusedTile;
	GameObject wall;
	GameObject playerCharacter;
	GameObject levelGenerator;

    GameObject dumbGen;

    List<GameObject> itemPool = new List<GameObject>();

	SlumsLevelGeneratorScript slumsLevelGeneratorScript;

	float oddColAdjustment = 0;
	float oddRowAdjustment = 0;

	public int mapRows;
	public int mapCols;

	//grid stores a list at each location, which stores all matrixOccupants
	private GameObject [,] levelGrid; 

	public int GetMapRows(){
		return mapRows;
	}
	public int GetMapCols(){
		return mapCols;
	}
		
	void Start () {
		//create levelGrid
		levelGrid = new GameObject [mapCols,mapRows];
		unusedTile = Resources.Load ("Prefabs/Environment/Unused") as GameObject;
		wall = Resources.Load ("Prefabs/Environment/Wall") as GameObject;
		playerCharacter = Resources.Load ("Prefabs/PlayerCharacter") as GameObject;
		if (mapCols % 2 == 0) {oddColAdjustment = -.5f;}
		if (mapRows % 2 == 0) {oddRowAdjustment = -.5f;}
		SpawnTiles();

		SpawnPlayerCharacter (mapCols/2,mapRows/2);
		GameObject slumsGeneratorPref = Resources.Load ("Prefabs/LevelGenerators/SlumsLevelGenerator") as GameObject;
        dumbGen = Resources.Load("Prefabs/LevelGenerators/DungeonGenerator") as GameObject;
        //GameObject myDumbGen = Instantiate(dumbGen, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        //GameObject slumsGenerator = Instantiate(slumsGeneratorPref, CoordToVector3(0, 0, 0), Quaternion.identity) as GameObject;
        //slumsLevelGeneratorScript = slumsGenerator.GetComponent<SlumsLevelGeneratorScript> ();
		levelGenerator = Resources.Load("Prefabs/LevelGenerators/LevelGeneratorDefault") as GameObject;
		GameObject lg = Instantiate (levelGenerator, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
    }

	private void SpawnTiles(){
		for (int r = 0; r < mapRows; r++) {
			for (int c = 0; c < mapCols; c++) {
				//ReplaceTile (c, r, unusedTile);
            }
		}
	}
	//converts coordinate in grid to location in scene
	public Vector3 CoordToVector3(int x ,int y, int z){
		return new Vector3 (x - mapCols / 2 - oddColAdjustment, y - mapRows / 2 - oddRowAdjustment, z);
	}
		
    //fix this
	private void SpawnPlayerCharacter(int x, int y){
		GameObject spawnPlayerCharacter = Instantiate (playerCharacter, CoordToVector3(x,y,-1), Quaternion.identity) as GameObject;
        spawnPlayerCharacter.GetComponent<PlayerCharacterScript>().SetPosVariables(x, y);
        
	}
	//replaces tile in level grid
	public void ReplaceTile(int x, int y, GameObject newTile){
		if (levelGrid [x, y] != null) {
			Destroy (levelGrid [x, y]);
		}
		levelGrid [x, y] = Instantiate (newTile, CoordToVector3(x,y, 0), Quaternion.identity) as GameObject;
	}

    public void SpawnItem(int x, int y, GameObject newItem)
    {
        Debug.Log("SpawneItem called");
       GameObject spawnedItem = Instantiate(newItem, CoordToVector3(x, y, -1), Quaternion.identity) as GameObject;
       spawnedItem.GetComponent<ItemScript>().SetPosition(x, y);
       itemPool.Add(spawnedItem);
    }

    public GameObject[,] GetLevelGrid()
    {
        return levelGrid;
        Debug.Log("got level grid");
    }
	public void SetLevelGrid(GameObject[,] lg){
		levelGrid = lg;
	}
    public List<GameObject> GetItemPool()
    {
        return itemPool;
    }
	public void ReplaceTileSprite(int x, int y, Sprite newSprite){
		levelGrid [x, y].GetComponent<SpriteRenderer> ().sprite = newSprite;
	}
}
