using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    #region FIELDS

    public TileManager tileManager;
    public TileContainer tileContainer;
    public UnitContainer teamContainer;
    public GameManager gameManager;
    public GameObject inputCanvas;
    public bool fail = false;
    public int teamNumber;
    public int UnitCount = 1;
    public bool isTurn = false;
    public bool prepMove = false;
    public bool prepAttack = false;
    public GameObject ArcherPrefabs, CavPrefab, SpearPrefab, SwordPrefab;
    public int2[] unitPositions;
    public UnitTypes[] unitTypes;

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        UnitCount = 14;
    }

    private void Awake()
    {
        teamContainer.team = teamNumber;
        StartCoroutine(UnitLoad(0.5f));
    }

    private void Update()
    {
        if (UnitCount <= 0)
        {
            EventManager.PlayerWin?.Invoke(teamNumber);
        }
        if (!isTurn) return;
        StartTurn();
    }

    #endregion UNITY METHODS

    #region METHODS

    public IEnumerator UnitLoad(float time)
    {
        yield return new WaitForSeconds(time);
        LoadUnits();
        //UnitCount = team.units.Count;
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
        /*        unit.GetComponent<Unit>().tileManager = tileManager;
                unit.GetComponent<Unit>().teamManager = this;
                unit.GetComponent<Unit>().team = unit.GetComponent<Unit>().unitProperties.team = teamNumber;
                unit.GetComponent<Unit>().team = unit.GetComponent<Unit>().team = teamNumber;*/
        tmpUnit.tileManager = tileManager;
        tmpUnit.teamManager = this;
        tmpUnit.team = tmpUnit.unitProperties.team = teamNumber;

        teamContainer.AddUnit(tmpUnit, pos);

        tileContainer.PosTileDict[pos].selectable = true;
        bool comp = false;
        tmpUnit.Move(pos, out comp);
    }

    public void StartTurn()
    {
        if (tileManager.selectedTile == null) return;

        if (tileManager.selectedTile.properties.Occupied == false) return;

        if (tileManager.selectedTile.properties.OccupyingUnit.team != teamNumber) return;
        inputCanvas.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            movement();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            attack();
        }
        if (tileManager.TargetTile == null) return;
        if (tileManager.TargetTile != null & prepAttack)//TakeAction - Attack
        {
            int damage;
            bool complete = false;
            try
            {
                damage = tileManager.selectedTile.properties.OccupyingUnit.unitProperties.attack - tileManager.TargetTile.properties.OccupyingUnit.unitProperties.defense;
                complete = false;
                tileManager.TargetTile.properties.OccupyingUnit.TakeDamage(damage, out complete);
            }
            catch (NullReferenceException)
            {
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
                Debug.Log("Null reference");
                return;
            }

            if (complete)
            {
                tileManager.resetTiles();
                isTurn = false;
                prepMove = false;
                prepAttack = false;
                inputCanvas.SetActive(false);
            }
            else
            {
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
            }
        }
        if (tileManager.TargetTile != null & prepMove)//TakeAction - Move
        {
            bool complete = false;
            try
            {
                tileManager.selectedTile.properties.OccupyingUnit.Move(tileContainer.KeyByValue(tileManager.TargetTile), out complete);
            }
            catch (NullReferenceException)
            {
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
                return;
            }

            if (complete)
            {
                tileManager.resetTiles();
                isTurn = false;
                prepMove = false;
                inputCanvas.SetActive(false);
                prepAttack = false;
            }
            else
            {
                tileManager.resetTiles();
                prepMove = false;
                prepAttack = false;
            }
        }
    }

    private void MoveUnit()
    {
        bool complete = false;
        tileManager.selectedTile.properties.OccupyingUnit.Move(tileContainer.KeyByValue(tileManager.TargetTile), out complete);
    }

    public void movement()
    {
        prepMove = true;
        prepAttack = false;
        foreach (Tile tile in tileContainer.tiles)
        {
            tile.selectable = false;
        }
        tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.movementRange, teamNumber, true);
    }

    public void attack()
    {
        prepMove = false;
        prepAttack = true;
        foreach (Tile tile in tileContainer.tiles)
        {
            tile.selectable = false;
            tile.properties.canHover = true;
        }
        tileManager.PopTilesInRad(tileContainer.KeyByValue(tileManager.selectedTile), tileManager.selectedTile.properties.OccupyingUnit.unitProperties.attackRange, teamNumber, false);
    }

    public void SetUnitLock(bool lockState)
    {
        int2[] tmpPosList = new int2[teamContainer.units.Count];
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tmpPosList[i] = teamContainer.units[i].unitProperties.Pos;
        }
        tileContainer.SetTileLock(tmpPosList, lockState);
    }

    public void SetTileSelectable(bool Selectable)
    {
        int2[] tmpPosList = new int2[teamContainer.units.Count];
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tmpPosList[i] = teamContainer.units[i].unitProperties.Pos;
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
        int2[] tmpPosList = new int2[teamContainer.units.Count];
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tmpPosList[i] = teamContainer.units[i].unitProperties.Pos;
        }
        tileContainer.SetTileHover(tmpPosList, hoverState);
    }

    public void setCanHover(bool hover)
    {
        int2[] tmpPosList = new int2[teamContainer.units.Count];
        for (int i = 0; i < teamContainer.units.Count; i++)
        {
            tmpPosList[i] = teamContainer.units[i].unitProperties.Pos;
        }
        tileContainer.setCanHover(tmpPosList, hover);
    }

    #endregion METHODS
}