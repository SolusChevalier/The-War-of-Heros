using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
        StartCoroutine(UnitLoad(1f));
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
        //Debug.Log("got comp");
        team.AddUnit(tmpUnit, pos);
        //Debug.Log("added unit");
        //tmpUnit.tileManager = tileManager;
        //tmpUnit.team = teamNumber;
        //Debug.Log("set tile manager");
        //StartCoroutine(wait(0.5f));
        tmpUnit.Move(pos);
        //Debug.Log("moved unit");
    }

    public void StartTurn()
    {
        //Debug.Log("Starting Turn");
        if (tileManager.selectedTile == null) return;
        //Debug.Log("Tile Selected");
        if (tileManager.selectedTile.properties.Occupied == false) return;
        //Debug.Log("Selected is occupied");
        if (tileManager.selectedTile.properties.OccupyingUnit.team != teamNumber) return;
        tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.movementRange, teamNumber);
        //Debug.Log("Unit Selected is of your team");

        //up to here works

        if (tileManager.TargetTile != null)
        {
            prepMove = true;
            Debug.Log("Unit Selected, prep move");
            /*if (tileManager.TargetTile != null)
            {
                Debug.Log("Target Tile Selected");
                if (tileManager.TargetTile.properties.OccupyingUnit.team != teamNumber)
                {
                    Debug.Log("Enemy Unit Selected");
                    MoveUnit();
                }
            }*/
            if (!prepMove)
            {
                prepMove = true;
                //tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.movementRange);
            }
            else
            {
                MoveUnit();
                tileManager.resetTiles();
                prepMove = false;
                isTurn = false;
            }
        }
    }

    private void MoveUnit()
    {
        Debug.Log("Moving Unit:");
        tileManager.selectedTile.properties.OccupyingUnit.Move(tileContainer.KeyByValue(tileManager.TargetTile));
        Debug.Log("Moved Unit:");
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

    #endregion METHODS
}