using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileContainer tileContainer;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Start()
    {
        mainCamera = Camera.main;
        tileContainer = GetComponent<TileContainer>();
    }

    private void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            foreach (Tile tile in tileContainer.tiles)
            {
                if (!tile._selected)
                {
                    tile.StopHover();
                }
                //tile.StopHover();
            }
            if (hit.collider.CompareTag("Tile"))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (Input.GetButtonDown("Fire1"))
                {
                    tile.Select();
                }
                if (!tile.properties.hover)
                {
                    tile.Hover();
                }
            }
        }
    }
}