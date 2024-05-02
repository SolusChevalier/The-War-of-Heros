using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //make att tiles that are ocupied display that they are ocupied in start
    public int TeamTurn = 1;

    //private bool isTurn = true;
    public TeamManager Team1, Team2;

    public string Win1, Win2;

    public UnitContainer unitContainer1, unitContainer2;
    public TileManager tileManager;

    private void Awake()
    {
        Team1.isTurn = true;
        Team2.isTurn = false;
        //tileManager.resetTileSelectable();
        Team1.SetUnitLock(true);
        Team1.SetTileSelectable(true);
        Team2.SetUnitLock(false);
        //Team1.SetTileSelectable(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (Team1.fail)
        {
            SceneManager.LoadScene(Win1);
        }
        else if (Team2.fail)
        {
            SceneManager.LoadScene(Win2);
        }
        if (TeamTurn == 1 && !Team2.isTurn)
        {
            //Debug.Log("Team 1's turn");
            Team1.isTurn = true;
            Team2.isTurn = false;
            TeamTurn = 2;
            tileManager.resetTileSelectable();
            Team1.SetUnitLock(true);
            Team1.SetTileSelectable(true);
            Team2.SetUnitLock(false);
            Team2.SetTileSelectable(false);
        }
        else if (TeamTurn == 2 && !Team1.isTurn)
        {
            //Debug.Log("Team 2's turn");
            Team1.isTurn = false;
            Team2.isTurn = true;
            TeamTurn = 1;
            tileManager.resetTileSelectable();
            Team1.SetUnitLock(false);
            Team1.SetTileSelectable(false);
            Team2.SetUnitLock(true);
            Team2.SetTileSelectable(true);
        }
    }

    public int teamPlayer()
    {
        if (Team1.isTurn)
        {
            return 1;
        }
        else if (Team2.isTurn)
        {
            return 2;
        }
        return 0;
    }

    public void SetTileSelectable(int team)
    {
        if (team == 1)
        {
            Team2.setTileHover(false);
            Team2.stopTeamHover();
            Team1.SetTileSelectable(true);
            Team1.setCanHover(true);
            Team2.setTileHover(true);
        }
        else if (team == 2)
        {
            Team1.setTileHover(false);
            Team1.stopTeamHover();
            Team2.SetTileSelectable(true);
            Team2.setCanHover(true);
            Team1.setTileHover(true);
        }
    }
}