using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileProperties properties;
    private Vector3 _progression;
    private readonly Vector3 _speedIncrement = Vector3.one;
    private Vector3 _motionEquation;
    private Vector3 _HoverAnchorLow, _HoverTarget, _HoverAnchorHigh;
    public bool selectable = false;

    private void Start()
    {
        properties = new TileProperties();
        Transform placementPoint = transform.GetChild(1).transform;
        properties.startProps(transform.position, placementPoint);
        _motionEquation = new Vector3(Mathf.Floor(Random.Range(0, 3)), Mathf.Floor(Random.Range(0, 3)), Mathf.Floor(Random.Range(0, 3)));
        _HoverAnchorLow = properties.StartPos + new Vector3(0, 0.2f, 0);
        _HoverAnchorHigh = properties.StartPos + new Vector3(0, 0.4f, 0);
        _HoverTarget = _HoverAnchorLow;
    }

    private void Update()
    {
        _progression += Time.deltaTime * _speedIncrement;
    }

    public void Hover()
    {
        if (!properties.canHover)
            return;
        properties.hover = true;
        StartCoroutine(HoverCo());
    }

    public void flipHoverHeight()
    {
        properties.highLow = !properties.highLow;

        if (properties.highLow)
            HoverHigh();
        else
            HoverLow();
    }

    public void HoverLow()
    {
        if (!properties.canHover)
            return;
        _HoverTarget = _HoverAnchorLow;
        //selectable = false;
    }

    public void HoverHigh()
    {
        if (!properties.canHover)
            return;
        _HoverTarget = _HoverAnchorHigh;
        //selectable = true;
    }

    public void StopHover()
    {
        properties.hover = false;
        //HoverLow();
        selectable = false;
        StopCoroutine(HoverCo());
        transform.position = Vector3.Lerp(transform.position, properties.StartPos, Time.deltaTime * 5f);
        HoverLow();
    }

    public IEnumerator HoverCo()
    {
        while (properties.hover && properties.canHover)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(
                    _HoverTarget.x, _HoverTarget.y + TrigMotionEquations((int)_motionEquation.y, _progression.y, 0.15f, 0.05f), _HoverTarget.z), Time.deltaTime * 5f);
            /*transform.position = Vector3.Lerp(transform.position, new Vector3(
                    HoverTarget.x + TrigMotionEquations((int)_motionEquation.x, _progression.x, 0.2f,
                        0.2f),
                    HoverTarget.y + TrigMotionEquations((int)_motionEquation.y, _progression.y, 0.2f,
                        0.2f),
                    HoverTarget.z + TrigMotionEquations((int)_motionEquation.z, _progression.z, 0.2f,
                        0.2f)), Time.deltaTime * 5f);*/
            yield return null;
        }
    }

    public void Select()
    {
        //_selected = true;
        flipHoverHeight();
        //selectable = true;
    }

    private float TrigMotionEquations(int equation, float progression, float frequency, float amplitude)
    {
        float result = 0f;
        switch (equation)
        {
            case 0:
                result = Mathf.Sin(1.8f * Mathf.Sin(Mathf.Cos(progression * 0.13f)) * Mathf.Cos((progression - 3f) * frequency)) * amplitude;
                break;

            case 1:
                result = Mathf.Sin((Mathf.Sin(progression * 0.8f) * 0.5f) * (Mathf.Cos(progression * 0.2f) * frequency)) * amplitude;
                break;

            case 2:
                result = (Mathf.Sin(progression * frequency)) * (Mathf.Sin((progression * 0.4f) * 0.3f)) * amplitude;
                break;
        }
        return result;
    }
}