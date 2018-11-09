using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : Construction
{
    public int woodRateValue;

    public override void CheckAvailable(Node node)
    {
        base.CheckAvailable(node);

        if (node.built && !node.occupied && node.content == NODE_CONTENT.WOODS)
            node.contentSprite.gameObject.SetActive(false);
    }

    protected override bool CanBuild(Node node, bool log = false)
    {
        return (base.CanBuild(node, log) && node.content == NODE_CONTENT.WOODS);
    }

    public override bool Build(Node node)
    {
        if (!base.Build(node))
            return false;

        Destroy(node.transform.Find("Content").gameObject);
        GameManager.instance.woodRate += woodRateValue;
        return true;
    }
}
