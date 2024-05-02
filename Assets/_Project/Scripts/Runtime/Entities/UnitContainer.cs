using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitContainer : MonoBehaviour
{
    #region FIELDS

    public int team;
    public List<Unit> units = new List<Unit>();

    #endregion FIELDS

    #region UNITY METHODS

    private void Start()
    {
        /*GameObject[] unit = new GameObject[1000];
        int count = 0;
        for (int i = 0; i <= 1000; i++)
        {
            GameObject item = GameObject.Find($"Unit ({i})");
            if (item)
            {
                unit[count] = item;
                count++;
            }
            else
            {
                break;
            }
        }
        System.Array.Resize(ref unit, count);
        foreach (GameObject item in unit)
        {
            units.Add(item.GetComponent<Unit>());
        }*/
    }

    #endregion UNITY METHODS

    #region METHODS

    public void AddUnit(Unit unit, int2 TilePos)
    {
        //unit.unitProperties.team = team;
        //unit.Move(TilePos);
        //Debug.Log("unit moved, adding to list");
        units.Add(unit);
        units[units.Count - 1].team = team;
    }

    #endregion METHODS
}