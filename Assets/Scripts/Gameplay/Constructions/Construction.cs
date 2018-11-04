using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public bool built;
    public int cost;

    protected Transform _transform;
    protected SpriteRenderer _sprite;

    protected Node node;

    private void Start()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();
    }

    public virtual bool Build(Node node)
    {
        if (GameManager.instance.stone < cost)
            return false;

        built = true;
        node.occupied = true;

        this.node = node;

        GameManager.instance.stone -= cost;
        _sprite.color = Color.white;

        return true;
    }
}
