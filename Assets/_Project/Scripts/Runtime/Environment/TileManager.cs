using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileContainer tileContainer;
    public GameManager gameManager;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    public Tile selectedTile, TargetTile;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Awake()
    {
        //StartCoroutine(waitReset(1f));
    }

    private void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        foreach (Tile tile in tileContainer.tiles)
        {
            if (!tile.selectable)
            {
                tile.StopHover();
            }
            else
            {
                tile.Hover();
            }
            //tile.StopHover();
        }
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (Input.GetButtonDown("Fire1") & tile.selectable)
                {
                    if (selectedTile == null)
                    {
                        selectedTile = tile;
                        tile.Select();
                    }
                    else if (selectedTile == tile)
                    {
                        selectedTile = null;
                        tile.Select();
                        resetTiles();
                        if (TargetTile != null)
                        {
                            TargetTile.Select();
                            TargetTile = null;
                        }
                    }
                    else
                    {
                        if (TargetTile != null)
                        {
                            TargetTile.Select();
                            TargetTile = null;
                        }
                        //TargetTile.Select();
                        TargetTile = tile;
                        tile.Select();
                    }
                }
                if (!tile.properties.hover || tile.selectable)// || tile.selectable
                {
                    tile.Hover();
                    //Debug.Log(tileContainer.KeyByValue(tile));
                }
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            resetTiles();
        }
    }

    public void resetTiles()
    {
        foreach (Tile tile in tileContainer.tiles)
        {
            tile.StopHover();
        }
        selectedTile = null;
        TargetTile = null;
        gameManager.SetTileSelectable(gameManager.teamPlayer());
    }

    public void resetTileSelectable()
    {
        tileContainer.ResetTileSelectable();
    }

    public Tile GetTile(int2 pos)
    {
        //Debug.Log("Getting tile: " + tileContainer.PosTileDict[pos]);
        return tileContainer.PosTileDict[pos];
    }

    public override string ToString()
    {
        return "TileManager";
    }

    public void PopTilesInRad(int2 PiecePos, int rad, int team, bool MovAtt)
    {
        int2[] selectableTiles = new int2[(rad * 2 + 1) * (rad * 2 + 1)];
        for (int i = -rad; i <= rad; i++)
        {
            for (int j = -rad; j <= rad; j++)
            {
                int2 newPos = new int2(PiecePos.x + i, PiecePos.y + j);
                if (tileContainer.PosTileDict.ContainsKey(newPos))
                {
                    if (MovAtt)
                    {
                        if (!tileContainer.PosTileDict[newPos].properties.Occupied)
                        {
                            tileContainer.PosTileDict[newPos].selectable = true;
                        }
                    }
                    else
                    {
                        if (tileContainer.PosTileDict[newPos].properties.Occupied) // & tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != team
                        {
                            if (tileContainer.PosTileDict[newPos].properties.OccupyingUnit.team != team)
                            {
                                tileContainer.PosTileDict[newPos].selectable = true;
                                tileContainer.PosTileDict[newPos].properties.canHover = true;
                            }
                        }
                    }

                    //tileContainer.PosTileDict[newPos].selectable = true;
                    //tileContainer.PosTileDict[newPos].HoverHigh();
                }
            }
        }
    }
}