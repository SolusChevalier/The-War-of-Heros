using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitProperties unitProperties;
    public TileManager tileManager;
    public int team;
    private bool ontile = false;

    private void Awake()
    {
        //unitProperties = GetComponent<UnitProperties>();

        //GameObject environment = GameObject.FindGameObjectsWithTag("Terrain")[0];
        //tileManager = environment.GetComponent<TileManager>();
        //unitProperties.team = team;
        //tileManager = FindObjectOfType<TileManager>();
    }

    private void Update()
    {
        if (ontile)
        {
            Tile tile = tileManager.GetTile(unitProperties.Pos);
            transform.position = tile.properties.PlacementPoint.position;
        }
    }

    public void Move(int2 newPos)
    {
        //Debug.Log("Moving unit, finding tile: ");
        Tile tile = tileManager.GetTile(newPos);
        //Debug.Log("Tile found 1");
        Tile currentTile = tileManager.GetTile(unitProperties.Pos);
        //Debug.Log("Current tile found");
        currentTile.properties.Occupied = false;
        currentTile.properties.OccupyingUnit = null;
        //Debug.Log("Tile found 2");
        if (tile.properties.Occupied)
        {
            Debug.Log("Tile is occupied");
            return;
        }
        //Debug.Log("Tile is not occupied, setting tile to occupied");
        tile.properties.Occupied = true;
        tile.properties.OccupyingUnit = this;
        unitProperties.Pos = newPos;
        //Debug.Log("Tile set to occupied, moving unit to placement point");
        transform.position = tile.properties.PlacementPoint.position;
        ontile = true;
    }
}