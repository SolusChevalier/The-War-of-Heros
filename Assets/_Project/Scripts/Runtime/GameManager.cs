using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //make att tiles that are ocupied display that they are ocupied in start
    public int TeamTurn = 1;

    //private bool isTurn = true;
    public TeamManager Team1, Team2;

    public UnitContainer unitContainer1, unitContainer2;
    public TileManager tileManager;

    private void Awake()
    {
        Team1.isTurn = true;
        Team2.isTurn = false;
        Team2.SetUnitLock(false);
        Team1.SetUnitLock(true);
    }

    public void Update()
    {
        if (TeamTurn == 1 && !Team2.isTurn)
        {
            Debug.Log("Team 1's turn");
            Team1.isTurn = true;
            Team2.isTurn = false;
            TeamTurn = 2;
            Team2.SetUnitLock(false);
            Team1.SetUnitLock(true);
        }
        else if (TeamTurn == 2 && !Team1.isTurn)
        {
            Debug.Log("Team 2's turn");
            Team1.isTurn = false;
            Team2.isTurn = true;
            TeamTurn = 1;
            Team1.SetUnitLock(false);
            Team2.SetUnitLock(true);
        }
    }
}