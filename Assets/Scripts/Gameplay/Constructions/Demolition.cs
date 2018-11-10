using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demolition : Construction
{
    protected override bool CanBuild(Node node, bool log = false)
    {
        if (!node.occupied)
        {
            if (node.content != NODE_CONTENT.STONES &&
                node.content != NODE_CONTENT.WOODS)
            {
                if (log)
                    UIManager.instance.Log("CAN'T DEMOLISH.",
                        new Color(1f, 0f, 0f, .75f));

                return false;
            }
        }
        if (GameManager.instance.gold < cost)
        {
            if (log)
                UIManager.instance.Log("NOT ENOUGH GOLD.",
                    new Color(1f, 0f, 0f, .75f));

            return false;
        }

        return true;
    }

    public override bool Build(Node node)
    {
        if (!CanBuild(node, true))
            return false;

        GameManager.instance.gold -= cost;

        Construction c = node.transform.GetComponentInChildren<Construction>();
        if (c)
            c.Demolish();
        else
        {
            Destroy(node.transform.Find("Content").gameObject);
            node.content = NODE_CONTENT.EMPTY;
        }

        Destroy(gameObject);

        SoundManager.PlaySound("build");

        return true;
    }
}