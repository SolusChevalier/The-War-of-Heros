using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileContainer : MonoBehaviour
{
    // this is where we will generate the map but for now it just stores the tiles in a manually ordered list
    public List<Tile> tiles = new List<Tile>();

    private void Start()
    {
        GameObject[] points = new GameObject[1000];
        int count = 0;
        for (int i = 0; i <= 1000; i++)
        {
            GameObject point = GameObject.Find($"Tile ({i})");
            if (point)
            {
                points[count] = point;
                count++;
            }
            else
            {
                break;
            }
        }
        System.Array.Resize(ref points, count);
        foreach (GameObject point in points)
        {
            tiles.Add(point.GetComponent<Tile>());
        }
        //tiles.AddRange(GetComponentsInChildren<Tile>());
    }
}