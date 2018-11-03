using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int size;
    public int seed;
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

        Random.InitState(seed);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                CreateNode(x, y, Random.Range(0, pfNodes.Length - 1));
            }
        }

        CreateBaseNode((int)Random.Range(size * .25f, size * .75f),
                        (int)Random.Range(size * .25f, size * .75f));
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

        nodes[x, y].built = true;
        GameManager.instance._baseNode = nodes[x, y];
    }

    public Node GetNode(int x, int y)
    {
        if (x >= size || y >= size || x < 0 || y < 0)
            return null;

        return nodes[x, y];
    }
}