using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int size;
    public GameObject[] pfNodes;

    private Node[,] nodes;

    private static Generator m_instance;
    public static Generator instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<Generator>();
            return m_instance;
        }
    }

    public void Generate()
    {
        nodes = new Node[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                int index = 0;
                float val = Random.value;
                if (val > .7505f)
                    index = 1;
                else if (val > .51f)
                    index = 2;
                else if (val > .5f)
                    index = 3;

                CreateNode(x, y, index);
            }
        }

        CreateLakes();
        CreateBaseNode(size / 2, size / 2);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (nodes[x, y].content == NODE_CONTENT.WATER)
                    nodes[x, y].GetComponent<WaterTiling>().UpdateNodeOutline(x, y);
            }
        }
    }

    private void CreateNode(int x, int y, int index)
    {
        GameObject node = Instantiate(pfNodes[index],
                            new Vector3(x - (size / 2), (size / 2) - y),
                            Quaternion.identity, transform);

        node.name = "Node (" + x + ", " + y + ")";
        nodes[x, y] = node.GetComponent<Node>();
        nodes[x, y].x = x;
        nodes[x, y].y = y;
    }

    private void CreateLakes()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (nodes[x, y].content == NODE_CONTENT.WATER)
                {
                    Node node = Generator.instance.GetNode(x - 1, y);
                    if (node && Random.value < .5f) CreateWaterNode(x - 1, y);
                    node = Generator.instance.GetNode(x + 1, y);
                    if (node && Random.value < .5f) CreateWaterNode(x + 1, y);
                    node = Generator.instance.GetNode(x, y - 1);
                    if (node && Random.value < .5f) CreateWaterNode(x, y - 1);
                    node = Generator.instance.GetNode(x, y + 1);
                    if (node && Random.value < .5f) CreateWaterNode(x, y + 1);
                }
            }
        }
    }

    private void CreateWaterNode(int x, int y)
    {
        Destroy(nodes[x, y].gameObject);
        CreateNode(x, y, 3);
    }

    private void CreateBaseNode(int x, int y)
    {
        Destroy(nodes[x, y].gameObject);
        CreateNode(x, y, pfNodes.Length - 1);

        GameManager.instance._baseNode = nodes[x, y];
    }

    public Node GetNode(int x, int y)
    {
        if (x >= size || y >= size || x < 0 || y < 0)
            return null;

        return nodes[x, y];
    }
}