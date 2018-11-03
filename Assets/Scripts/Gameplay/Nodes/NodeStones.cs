using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeStones : Node
{
    public int increment;

    protected override bool Build()
    {
        if (!base.Build())
            return false;

        GameManager.instance.stoneRate += increment;
        occupied = true;

        return true;
    }
}