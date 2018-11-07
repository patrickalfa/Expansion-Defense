using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeStones : Node
{
    public int increment;

    protected override void Start()
    {
        base.Start();

        float h = (30f / 360f);
        float sMax = (20f / 100f);
        float vMin = (80f / 100f);
        _content.material.color = Random.ColorHSV(h, h, 0f, sMax, vMin, 1f);
    }

    protected override bool Build()
    {
        if (!base.Build())
            return false;

        GameManager.instance.goldRate += increment;
        occupied = true;

        return true;
    }
}