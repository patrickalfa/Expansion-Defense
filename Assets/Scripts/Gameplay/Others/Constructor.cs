using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructor : MonoBehaviour
{
    [Header("Control variables")]
    public int constructionIndex;

    [Header("References")]
    public Construction _construction;

    [Header("Prefabs")]
    public GameObject[] pfConstructions;

    private static Constructor m_instance;
    public static Constructor instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<Constructor>();
            return m_instance;
        }
    }

    public void Construct(Node node)
    {
        if (!_construction.Build(node))
            return;

        _construction = null;
        SelectContruction(constructionIndex);
    }

    public void SelectContruction(int index)
    {
        constructionIndex = index;

        if (_construction)
            Destroy(_construction.gameObject);

        _construction = Instantiate(pfConstructions[constructionIndex]).
                GetComponent<Construction>();
        _construction.gameObject.SetActive(false);
        _construction.SetSortingOrder(9999);
        _construction.transform.parent = transform;
    }
}
