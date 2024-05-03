using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public UnitProperties unitProperties;
    public TileManager tileManager;
    public TeamManager teamManager;
    public int team;
    private bool ontile = false;

    private void Awake()
    {
        //unitProperties = GetComponent<UnitProperties>();
        unitProperties.OnDied.AddListener(HandleUnitDeath);
        /*GameObject team = GameObject.FindGameObjectsWithTag($"TeamManager{team}")[0];
        teamManager = team.GetComponent<TeamManager>();*/
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

    private void HandleUnitDeath()
    {
        tileManager.GetTile(unitProperties.Pos).properties.Occupied = false;
        tileManager.GetTile(unitProperties.Pos).properties.OccupyingUnit = null;
        teamManager.teamContainer.units.Remove(this);
        teamManager.UnitCount--;
        Destroy(gameObject);
    }

    public void TakeDamage(int dam, out bool complete)
    {
        complete = true;
        unitProperties.TakeDamage(dam);
    }

    public void Move(int2 newPos, out bool complete)
    {
        Tile tile = tileManager.GetTile(newPos);
        if (!tile.selectable | tile.properties.Occupied)
        {
            complete = false;
            return;
        }

        Tile currentTile = tileManager.GetTile(unitProperties.Pos);

        currentTile.properties.Occupied = false;
        currentTile.properties.OccupyingUnit = null;

        tile.properties.Occupied = true;
        tile.properties.OccupyingUnit = this;
        unitProperties.Pos = newPos;

        transform.position = tile.properties.PlacementPoint.position;
        ontile = true;
        complete = true;
        return;
    }
}