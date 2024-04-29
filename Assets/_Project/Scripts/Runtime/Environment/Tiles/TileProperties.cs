using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    public Vector3 StartPos;

    private void Start()
    {
        StartPos = transform.position;
    }
}