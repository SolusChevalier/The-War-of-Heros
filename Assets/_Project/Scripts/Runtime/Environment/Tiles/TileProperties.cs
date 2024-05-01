using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties
{
    public Vector3 StartPos;
    public Vector3 PlacementPoint;
    public bool _selected;
    public bool canHover;
    public bool hover;
    public bool highLow;

    public void startProps(Vector3 stPos, Vector3 PPpoint)
    {
        StartPos = stPos;
        PlacementPoint = PPpoint;
        _selected = false;
        canHover = true;
        hover = false;
        highLow = false;
    }
}