using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeWoods : Node
{
    public int increment;

    protected override bool Build()
    {
        if (!base.Build())
            return false;

        GameManager.instance.woodRate += increment;
        return true;
    }
}