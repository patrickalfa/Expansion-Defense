using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum NODE_CONTENT
{
    EMPTY,
    WOODS,
    STONES,
    WATER,
    BASE
}

public class Node : MonoBehaviour
{   
    [Header("Attributes")]
    public int x, y;
    public bool built;
    public bool occupied;
    public int cost;
    public NODE_CONTENT content;

    [Header("Control variables")]
    protected bool over;

    [Header("References")]
    public Sprite groundBuilt;
    public SpriteRenderer contentSprite;

    protected Transform _transform;
    protected SpriteRenderer _sprite;
    protected Transform _outline;
    protected Transform _overlay;

    protected virtual void Start()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();
        _outline = _transform.Find("Outline");
        _overlay = _transform.Find("Overlay");

        if (content != NODE_CONTENT.EMPTY && content != NODE_CONTENT.WATER)
            contentSprite = _transform.Find("Content").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!over)
            UpdateSortingOrder(Mathf.RoundToInt(transform.position.y * 100f) * -1);
    }

    private void OnMouseEnter()
    {
        over = true;

        if (GameManager.instance.stage == GAME_STAGE.EXPANSION || occupied)
        {
            UpdateSortingOrder(9999);

            if (IsAdjacent() && !built && content != NODE_CONTENT.WATER)
                _overlay.gameObject.SetActive(true);

            _transform.DOKill();
            _transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(() =>
            {
                _transform.localScale = Vector3.one;
                UpdateSortingOrder(Mathf.RoundToInt(transform.position.y * 100f) * -1);
            });
        }
        if (GameManager.instance.stage == GAME_STAGE.CONSTRUCTION)
        {
            if (!Constructor.instance._construction)
                return;

            Constructor.instance._construction.gameObject.SetActive(built && !occupied);
            Constructor.instance._construction.transform.position = _transform.position;
            Constructor.instance._construction.SetSortingOrder(_sprite.sortingOrder + 102);
            Constructor.instance._construction.CheckAvailable(this);
        }

        if (GameManager.instance.stage != GAME_STAGE.DEFENSE)
            SoundManager.PlaySound("over" + Random.Range(0, 3), false, .5f);
    }

    private void OnMouseExit()
    {
        over = false;

        _overlay.gameObject.SetActive(false);

        _transform.DOKill();
        _transform.localScale = Vector3.one;

        if (contentSprite)
            contentSprite.gameObject.SetActive(true);
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.state == GAME_STATE.DRAGGING)
            return;

        if (GameManager.instance.stage == GAME_STAGE.EXPANSION)
        {
            if (!built && content != NODE_CONTENT.WATER)
                Build();
        }
        else if (GameManager.instance.stage == GAME_STAGE.CONSTRUCTION)
        {
            if (built && !occupied)
                Constructor.instance.Construct(this);
        }
    }

    protected virtual bool Build()
    {
        if (!CanBuild())
            return false;

        built = true;
        GameManager.instance.wood -= cost;

        _sprite.sprite = groundBuilt;

        SetColor(Color.white);

        UpdateOutlines();
        UpdateSortingOrder(999);
        _overlay.gameObject.SetActive(false);

        _transform.DOKill();
        _transform.DOPunchScale(Vector3.one * .5f, .25f).OnComplete(() =>
        {
            _transform.localScale = Vector3.one;
            UpdateSortingOrder(Mathf.RoundToInt(transform.position.y * 100f) * -1);
        });

        if (content == NODE_CONTENT.WOODS)
            contentSprite.transform.localScale = Vector3.one * .75f;
        else if (content == NODE_CONTENT.STONES)
            contentSprite.transform.localScale = Vector3.one * .95f;

        SoundManager.PlaySound("build");
        GameManager.instance.ScreenShake(.1f, .1f, 50);

        return true;
    }

    private bool CanBuild()
    {
        return (IsAdjacent() && IsAffordable() && !built);
    }

    private bool IsAffordable()
    {
        if (GameManager.instance.wood < cost)
        {
            UIManager.instance.Log("NOT ENOUGH WOOD.",
                new Color(1f, 0f, 0f, .75f));

            return false;
        }

        return true;
    }

    private bool IsAdjacent()
    {
        Node node;

        node = Generator.instance.GetNode(x - 1, y);
        bool l = (node && node.built);
        node = Generator.instance.GetNode(x + 1, y);
        bool r = (node && node.built);
        node = Generator.instance.GetNode(x, y - 1);
        bool u = (node && node.built);
        node = Generator.instance.GetNode(x, y + 1);
        bool d = (node && node.built);

        return (l || r || u || d);
    }

    private void UpdateSortingOrder(int sortingOrder)
    {
        _sprite.sortingOrder = sortingOrder;

        if (contentSprite)
            contentSprite.sortingOrder = sortingOrder + 102;

        SpriteRenderer[] outlines = _outline.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer o in outlines)
            o.sortingOrder = sortingOrder + 1;
    }

    private void UpdateOutlines()
    {
        _outline.gameObject.SetActive(true);

        UpdateNodeOutline();

        Node node;
        node = Generator.instance.GetNode(x - 1, y);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x + 1, y);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x, y - 1);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x, y + 1);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x - 1, y - 1);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x - 1, y +  1);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x + 1, y - 1);
        if (node) node.UpdateNodeOutline();
        node = Generator.instance.GetNode(x + 1, y + 1);
        if (node) node.UpdateNodeOutline();
    }

    public void UpdateNodeOutline()
    {
        if (!built)
            return;

        Node node;
        node = Generator.instance.GetNode(x - 1, y);
        bool l = node && node.built;
        node = Generator.instance.GetNode(x + 1, y);
        bool r = node && node.built;
        node = Generator.instance.GetNode(x, y - 1);
        bool u = node && node.built;
        node = Generator.instance.GetNode(x, y + 1);
        bool d = node && node.built;

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
        bool lu = node && node.built;
        node = Generator.instance.GetNode(x + 1, y - 1);
        bool ru = node && node.built;
        node = Generator.instance.GetNode(x - 1, y + 1);
        bool ld = node && node.built;
        node = Generator.instance.GetNode(x + 1, y + 1);
        bool rd = node && node.built;

        Transform outerCorners = _outline.Find("OuterCorners");
        outerCorners.Find("TopLeft").gameObject.SetActive((l && u) && !lu);
        outerCorners.Find("TopRight").gameObject.SetActive((r && u) && !ru);
        outerCorners.Find("BottomLeft").gameObject.SetActive((l && d) && !ld);
        outerCorners.Find("BottomRight").gameObject.SetActive((r && d) && !rd);
    }

    public void SetColor(Color color)
    {
        _sprite.color = color;
        if (contentSprite)
            contentSprite.color = color;
    }
}
