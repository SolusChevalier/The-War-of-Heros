using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class GameStateRep : MonoBehaviour
{
    #region FIELDS

    public List<Unit> Player1Units = new List<Unit>();
    public List<Unit> Player2Units = new List<Unit>();
    public float Player1Value;
    public float Player2Value;
    public bool GameOver;

    #endregion FIELDS

    public void UpdateGameState(List<Unit> P1Units, List<Unit> P2Units, bool GGs)
    {
        Player1Units = P1Units;
        Player2Units = P2Units;
        Player1Value = GetPlayerValue(1);
        Player2Value = GetPlayerValue(2);
        GameOver = GGs;
    }

    public float GetPlayerValue(int player)
    {
        float value = 0;
        if (player == 1)
        {
            foreach (var unit in Player1Units)
            {
                value += unit.GetUnitValue();
            }
            float teamValue = ((float)Player1Units.Count / (float)14);
            value *= teamValue;
            value = (float)(Math.Round(((double)value), 2));
        }
        else
        {
            foreach (var unit in Player2Units)
            {
                value += unit.GetUnitValue();
            }
            float teamValue = ((float)Player2Units.Count / (float)14);
            value *= teamValue;
            value = (float)(Math.Round(((double)value), 2));
        }

        return value;
    }

    public GameStateRep GetGameState()
    {
        return this;
    }
}