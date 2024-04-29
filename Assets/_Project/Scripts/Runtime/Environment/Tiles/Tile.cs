using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileProperties properties;
    private Vector3 _progression;
    private readonly Vector3 _speedIncrement = Vector3.one;
    public bool canHover = true;
    public bool hover = false;

    private void Start()
    {
        properties = GetComponent<TileProperties>();
    }

    // Update is called once per frame
    private void Update()
    {
        _progression += Time.deltaTime * _speedIncrement;
    }

    public void StartHover()
    {
        StartCoroutine(Hover(properties.StartPos + Vector3.up));
    }

    private IEnumerator Hover(Vector3 target)
    {
        while (hover)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
            yield return null;
        }
    }
}