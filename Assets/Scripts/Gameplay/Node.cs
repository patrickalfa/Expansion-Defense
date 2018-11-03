using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{   
    [Header("Attributes")]
    public int x, y;
    public bool built;
    public int cost;

    [Header("Control variables")]
    private bool over;

    [Header("References")]
    public Sprite groundBuilt;
    public Sprite contentBuilt;

    private Transform _transform;
    private SpriteRenderer _sprite;
    private SpriteRenderer _content;
    private Transform _outline;
    private Transform _overlay;

    private void Start()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();
        _outline = _transform.Find("Outline");
        _overlay = _transform.Find("Overlay");

        if (_transform.Find("Content"))
            _content = _transform.Find("Content").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!over)
            UpdateSortingOrder(Mathf.RoundToInt(transform.position.y * 100f) * -1);
    }

    private void OnMouseEnter()
    {
        if (GameManager.instance.stage == GAME_STAGE.EXPANSION)
        {
            over = true;

            UpdateSortingOrder(999);

            if (!built)
                _overlay.gameObject.SetActive(true);

            _transform.DOKill();
            _transform.DOPunchScale(Vector3.one * .1f, .25f).OnComplete(() =>
            {
                _transform.localScale = Vector3.one;
                UpdateSortingOrder(Mathf.RoundToInt(transform.position.y * 100f) * -1);
            });
        }
    }

    private void OnMouseExit()
    {
        over = false;

        if (!built)
            _overlay.gameObject.SetActive(false);

        _transform.DOKill();
        _transform.localScale = Vector3.one;
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.state == GAME_STATE.DRAGGING)
            return;

        if (GameManager.instance.stage == GAME_STAGE.EXPANSION)
        {
            if (!built)
                Build();
        }
    }

    protected virtual bool Build()
    {
        if (GameManager.instance.wood < cost)
            return false;

        built = true;
        GameManager.instance.wood -= cost;

        _sprite.sprite = groundBuilt;
        if (_content)
            _content.sprite = contentBuilt;

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

        return true;
    }

    private void UpdateSortingOrder(int sortingOrder)
    {
        _sprite.sortingOrder = sortingOrder;

        if (_content)
            _content.sortingOrder = sortingOrder + 102;

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
    }

    public void UpdateNodeOutline()
    {
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
    }

    public void SetColor(Color color)
    {
        _sprite.color = color;
        if (_content)
            _content.color = color;
    }
}
