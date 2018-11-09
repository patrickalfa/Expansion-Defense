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
        if (node.content != NODE_CONTENT.WOODS || node.occupied)
        {
            if (log)
                UIManager.instance.Log("CAN'T BUILD.",
                    new Color(1f, 0f, 0f, .75f));

            return false;
        }
        if (GameManager.instance.gold < cost)
        {
            if (log)
                UIManager.instance.Log("NOT ENOUGH GOLD.",
                    new Color(1f, 0f, 0f, .75f));

            return false;
        }
        if (!IsLit())
        {
            if (log)
                UIManager.instance.Log("NOT ENOUGH LIGHT.",
                    new Color(1f, 0f, 0f, .75f));

            return false;
        }

        return true;
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
