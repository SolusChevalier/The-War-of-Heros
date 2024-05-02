using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    #region FIELDS

    public TileManager tileManager;
    public TileContainer tileContainer;
    public UnitContainer team;
    public GameManager gameManager;
    public int teamNumber;
    public bool isTurn = false;
    public bool prepMove = false;
    public GameObject ArcherPrefabs, CavPrefab, SpearPrefab, SwordPrefab;
    public int2[] unitPositions;
    public UnitTypes[] unitTypes;

    #endregion FIELDS

    #region UNITY METHODS

    private void Awake()
    {
        getTeams();
        StartCoroutine(UnitLoad(0.5f));
        //tileManager.resetTiles();
        //LoadUnits();
        //Debug.Log("Instantiated units");
        /*instantiateUnit(new int2(1, 1), "Archer");
        instantiateUnit(new int2(2, 1), "Cavalry");
        instantiateUnit(new int2(3, 1), "Spearman");
        instantiateUnit(new int2(4, 1), "Swordsman");*/
    }

    private void Update()
    {
        if (!isTurn) return;
        StartTurn();
    }

    #endregion UNITY METHODS

    #region METHODS

    public void getTeams()
    {
        team = GetComponent<UnitContainer>();
        team.team = teamNumber;
    }

    public IEnumerator UnitLoad(float time)
    {
        yield return new WaitForSeconds(time);
        LoadUnits();
    }

    public IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void LoadUnits()
    {
        for (int i = 0; i < unitPositions.Length; i++)
        {
            instantiateUnit(unitPositions[i], unitTypes[i]);
        }
    }

    public void instantiateUnit(int2 pos, UnitTypes unitType)
    {
        GameObject unit = null;
        Unit tmpUnit = null;
        Transform placementPoint = tileManager.GetTile(pos).properties.PlacementPoint;
        switch (unitType)
        {
            case UnitTypes.Archer:
                unit = Instantiate(ArcherPrefabs, placementPoint.position, placementPoint.rotation);
                break;

            case UnitTypes.Cavalry:
                unit = Instantiate(CavPrefab, placementPoint.position, placementPoint.rotation);
                break;

            case UnitTypes.Spearman:
                unit = Instantiate(SpearPrefab, placementPoint.position, placementPoint.rotation);
                break;

            case UnitTypes.Swordsman:
                unit = Instantiate(SwordPrefab, placementPoint.position, placementPoint.rotation);
                break;
        }
        tmpUnit = unit.GetComponent<Unit>();
        unit.GetComponent<Unit>().tileManager = tileManager;
        unit.GetComponent<Unit>().team = unit.GetComponent<Unit>().unitProperties.team = teamNumber;

        team.AddUnit(tmpUnit, pos);

        tileContainer.PosTileDict[pos].selectable = true;
        bool comp = false;
        tmpUnit.Move(pos, out comp);
    }

    public void StartTurn()
    {
        if (tileManager.selectedTile == null) return;

        if (tileManager.selectedTile.properties.Occupied == false) return;

        if (tileManager.selectedTile.properties.OccupyingUnit.team != teamNumber) return;

        if (Input.GetButtonDown("Move"))
        {
            prepMove = true;
            foreach (Tile tile in tileContainer.tiles)
            {
                tile.selectable = false;
            }
            tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.movementRange, teamNumber, true);
        }
        if (Input.GetButtonDown("Attack"))
        {
            prepMove = true;
            tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.movementRange, teamNumber, false);
        }
        //have selection - pop up tiles
        if (!prepMove) return;

        if (tileManager.TargetTile != null)//TakeAction - Move or Attack
        {
            bool complete = false;
            tileManager.selectedTile.properties.OccupyingUnit.Move(tileContainer.KeyByValue(tileManager.TargetTile), out complete);

            if (complete)
            {
                tileManager.resetTiles();
                isTurn = false;
                prepMove = false;
            }
            else
            {
                tileManager.resetTiles();
                prepMove = false;
            }
        }
    }

    private void MoveUnit()
    {
        bool complete = false;
        tileManager.selectedTile.properties.OccupyingUnit.Move(tileContainer.KeyByValue(tileManager.TargetTile), out complete);
    }

    public void SetUnitLock(bool lockState)
    {
        int2[] tmpPosList = new int2[team.units.Count];
        for (int i = 0; i < team.units.Count; i++)
        {
            tmpPosList[i] = team.units[i].unitProperties.Pos;
        }
        tileContainer.SetTileLock(tmpPosList, lockState);
    }

    public void SetTileSelectable(bool Selectable)
    {
        int2[] tmpPosList = new int2[team.units.Count];
        for (int i = 0; i < team.units.Count; i++)
        {
            tmpPosList[i] = team.units[i].unitProperties.Pos;
        }
        tileContainer.SetTileSelectable(tmpPosList, Selectable);
    }

    public void stopTeamHover()
    {
        foreach (Tile tile in tileManager.tileContainer.tiles)
        {
            tile.StopHover();
        }
    }

    public void setTileHover(bool hoverState)
    {
        int2[] tmpPosList = new int2[team.units.Count];
        for (int i = 0; i < team.units.Count; i++)
        {
            tmpPosList[i] = team.units[i].unitProperties.Pos;
        }
        tileContainer.SetTileHover(tmpPosList, hoverState);
    }

    #endregion METHODS
}