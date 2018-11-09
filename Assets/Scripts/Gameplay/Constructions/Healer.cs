using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : Construction
{
    public int healRateValue;

    public override bool Build(Node node)
    {
        if (!base.Build(node))
            return false;

        GameManager.instance.healRate += healRateValue;
        return true;
    }

    public override void Demolish()
    {
        GameManager.instance.healRate -= healRateValue;
        base.Demolish();
    }
}
