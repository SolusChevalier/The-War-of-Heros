using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    // this is where we will generate the map but for now it just stores the tiles in a manually ordered list
    public List<Tile> tiles = new List<Tile>();

    public Dictionary<int2, Tile> PosTileDict = new Dictionary<int2, Tile>();

    private void Start()
    {
        int count = 0;
        for (int Y = 0; Y <= 7; Y++)
        {
            for (int X = 0; X <= 15; X++)
            {
                PosTileDict.Add(new int2(X, Y), tiles[count]);
                count++;
            }
        }
        /*
        Tile[] tilesArray = tiles.ToArray();
        var job = new DictLoad
        {
            Input = tilesArray,
            Output = PosTileDict
        };
        job.Schedule().Complete();
        */
    }

    public int2 KeyByValue(Tile value)
    {
        foreach (KeyValuePair<int2, Tile> entry in PosTileDict)
        {
            if (entry.Value == value)
            {
                return entry.Key;
            }
        }
        return new int2(-1, -1);
    }

    public Tile ValueByKey(int2 key)
    {
        return PosTileDict[key];
    }

    public void SetTileLock(int2[] tiles, bool lockState)
    {
        foreach (int2 tile in tiles)
        {
            PosTileDict[tile].properties.canHover = lockState;
        }
    }

    public void ResetTileSelectable()
    {
        foreach (Tile tile in tiles)
        {
            tile.selectable = false;
        }
    }

    public void SetTileSelectable(int2[] tiles, bool SelectableState)
    {
        foreach (int2 tile in tiles)
        {
            PosTileDict[tile].selectable = SelectableState;
        }
    }

    public void SetTileHover(int2[] tiles, bool HoverState)
    {
        foreach (int2 tile in tiles)
        {
            PosTileDict[tile].properties.hover = HoverState;
        }
    }

    public void setCanHover(int2[] tiles, bool hov)
    {
        foreach (int2 tile in tiles)
        {
            PosTileDict[tile].properties.canHover = hov;
        }
    }

    /*[BurstCompile(CompileSynchronously = true)]
    private struct DictLoad : IJob
    {
        [ReadOnly]
        public Tile[] Input;

        [WriteOnly]
        public Dictionary<int2, Tile> Output;

        public void Execute()
        {
            int count = 0;
            for (int Y = 0; Y <= 7; Y++)
            {
                for (int X = 0; X <= 15; X++)
                {
                    Output.Add(new int2(X, Y), Input[count]);
                    count++;
                }
            }
        }
    }*/
}