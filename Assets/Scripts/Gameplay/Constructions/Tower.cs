using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tower : Construction
{
    [Header("Attributes")]
    public int damage;
    public float range;
    public float reloadTime;

    private bool canFire = true;

    private GameObject _rangeIndicator;
    private Transform _projectile;

    protected override void Start()
    {
        base.Start();

        _rangeIndicator = _transform.Find("Range").gameObject;
        _rangeIndicator.transform.localScale = Vector3.one * range * 2f;
        _projectile = _transform.Find("Projectile");
    }

    private void FixedUpdate()
    {
        Collider2D col = Physics2D.OverlapCircle(_transform.position, range, LayerMask.GetMask("Enemy"));
        if (col && canFire)
            Shoot(col.transform);
    }

    public override bool Build(Node node)
    {
        if (!base.Build(node))
            return false;

        node.GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        _rangeIndicator.SetActive(false);

        return true;
    }

    private void Shoot(Transform target)
    {
        _projectile.position = _transform.position;
        _projectile.gameObject.SetActive(true);
        _projectile.DOMove(target.position, .1f).OnComplete(() =>
        {
            _projectile.gameObject.SetActive(false);
            target.GetComponent<IDamageable>().TakeDamage(damage);
        });

        canFire = false;
        Invoke("Reload", reloadTime);
    }

    private void Reload()
    {
        canFire = true;
    }

    private void OnMouseEnter()
    {
        if (built)
            _rangeIndicator.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (built)
            _rangeIndicator.SetActive(false);
    }
}