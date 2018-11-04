using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeWoods : Node
{
    public int increment;

    protected override void Start()
    {
        base.Start();

        float hMin = (40f / 360f);
        float hMax = (80f / 360f);
        _content.material.color = Random.ColorHSV(hMin, hMax, 0f, 1f, 1f, 1f);
    }

    protected override bool Build()
    {
        if (!base.Build())
            return false;

        GameManager.instance.woodRate += increment;
        occupied = true;

        return true;
    }
}