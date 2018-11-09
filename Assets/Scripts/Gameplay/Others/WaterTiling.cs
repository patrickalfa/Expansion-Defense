using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTiling : MonoBehaviour
{
    public void UpdateOutlines(int x, int y)
    {
        Transform _outline = transform.Find("Outline");

        _outline.gameObject.SetActive(true);

        UpdateNodeOutline(x, y);

        Node node;
        node = Generator.instance.GetNode(x - 1, y);
        if (node && node.content == NODE_CONTENT.WATER)
            node.GetComponent<WaterTiling>().UpdateNodeOutline(x - 1, y);

        node = Generator.instance.GetNode(x + 1, y);
        if (node && node.content == NODE_CONTENT.WATER)
            node.GetComponent<WaterTiling>().UpdateNodeOutline(x + 1, y);

        node = Generator.instance.GetNode(x, y - 1);
        if (node && node.content == NODE_CONTENT.WATER)
            node.GetComponent<WaterTiling>().UpdateNodeOutline(x, y - 1);

        node = Generator.instance.GetNode(x, y + 1);
        if (node && node.content == NODE_CONTENT.WATER)
            node.GetComponent<WaterTiling>().UpdateNodeOutline(x, y + 1);
    }

    public void UpdateNodeOutline(int x, int y)
    {
        Transform _outline = transform.Find("Outline");

        Node node;
        node = Generator.instance.GetNode(x - 1, y);
        bool l = node && node.content == NODE_CONTENT.WATER;
        node = Generator.instance.GetNode(x + 1, y);
        bool r = node && node.content == NODE_CONTENT.WATER;
        node = Generator.instance.GetNode(x, y - 1);
        bool u = node && node.content == NODE_CONTENT.WATER;
        node = Generator.instance.GetNode(x, y + 1);
        bool d = node && node.content == NODE_CONTENT.WATER;

        _outline.transform.Find("Left").gameObject.SetActive(!l);
        _outline.transform.Find("Right").gameObject.SetActive(!r);
        _outline.transform.Find("Up").gameObject.SetActive(!u);
        _outline.transform.Find("Down").gameObject.SetActive(!d);

        ////////////////////////////////////////////////////////////////////////////////////

        foreach (Transform c in _outline.Find("InnerCorners"))
        {
            c.gameObject.SetActive(true);

            if (c.name.Contains("Left") && l)
                c.gameObject.SetActive(false);
            if (c.name.Contains("Right") && r)
                c.gameObject.SetActive(false);
            if (c.name.Contains("Top") && u)
                c.gameObject.SetActive(false);
            if (c.name.Contains("Bottom") && d)
                c.gameObject.SetActive(false);
        }

        ////////////////////////////////////////////////////////////////////////////////////

        node = Generator.instance.GetNode(x - 1, y - 1);
        bool lu = node && node.content == NODE_CONTENT.WATER;
        node = Generator.instance.GetNode(x + 1, y - 1);
        bool ru = node && node.content == NODE_CONTENT.WATER;
        node = Generator.instance.GetNode(x - 1, y + 1);
        bool ld = node && node.content == NODE_CONTENT.WATER;
        node = Generator.instance.GetNode(x + 1, y + 1);
        bool rd = node && node.content == NODE_CONTENT.WATER;

        Transform outerCorners = _outline.Find("OuterCorners");
        outerCorners.Find("TopLeft").gameObject.SetActive((l && u) && !lu);
        outerCorners.Find("TopRight").gameObject.SetActive((r && u) && !ru);
        outerCorners.Find("BottomLeft").gameObject.SetActive((l && d) && !ld);
        outerCorners.Find("BottomRight").gameObject.SetActive((r && d) && !rd);
    }
}
