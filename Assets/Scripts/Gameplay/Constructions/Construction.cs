using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Construction : MonoBehaviour
{
    public bool built;
    public int cost;

    protected Transform _transform;
    protected SpriteRenderer _sprite;

    protected Node node;

    protected virtual void Awake()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.material.color = new Color(1f, 1f, 1f, .5f);
    }

    public virtual void CheckAvailable(Node node)
    {
        if (CanBuild(node))
            _sprite.material.color = new Color(1f, 1f, 1f, .5f);
        else
            _sprite.material.color = new Color(1f, 0f, 0f, .5f);

        _sprite.sortingOrder = 9999;
    }


    public virtual bool Build(Node node)
    {
        if (!CanBuild(node, true))
            return false;

        built = true;
        node.occupied = true;
        node.contentSprite = _sprite;
        transform.parent = node.transform;

        this.node = node;

        GameManager.instance.gold -= cost;
        _sprite.material.color = Color.white;

        SetSortingOrder(node.GetComponent<SpriteRenderer>().sortingOrder + 102);

        _transform.DOKill();
        _transform.DOPunchScale(Vector3.one * .5f, .25f).OnComplete(() =>
        {
            _transform.localScale = Vector3.one;
        });

        SoundManager.PlaySound("build");
        GameManager.instance.ScreenShake(.1f, .1f, 50);

        return true;
    }

    public virtual void Demolish()
    {
        Destroy(gameObject);
    }
    
    public virtual void SetSortingOrder(int sortingOrder)
    {
        if (_sprite)
            _sprite.sortingOrder = sortingOrder;
    }

    protected virtual bool CanBuild(Node node, bool log = false)
    {
        if (node.content != NODE_CONTENT.EMPTY || node.occupied)
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

    protected bool IsLit()
    {
        if (!Physics2D.OverlapCircle(_transform.position, 1.5f, LayerMask.GetMask("Light")))
            return false;

        return true;
    }
}
