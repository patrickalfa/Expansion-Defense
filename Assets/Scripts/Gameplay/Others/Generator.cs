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
                if (val > .75f)
                    index = 1;
                else if (val > .5f)
                    index = 2;

                CreateNode(x, y, index);
            }
        }

        CreateBaseNode(size / 2, size / 2);
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