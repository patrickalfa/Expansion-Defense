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

    [Header("References")]
    public Transform pfProjectile;

    private bool canFire = true;
    private Transform _turret;
    private GameObject _rangeIndicator;

    protected override void Awake()
    {
        base.Awake();

        _turret = _transform.Find("Turret");
        _rangeIndicator = _transform.Find("Range").gameObject;
        _rangeIndicator.transform.localScale = Vector3.one * range * 2f;
        _turret.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, .5f);
    }

    private void FixedUpdate()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(_transform.position, range, LayerMask.GetMask("Enemy"));

        bool shot = false;
        foreach (Collider2D col in cols)
        {
            if (col && canFire && !col.GetComponent<IDamageable>().IsTargeted())
            {
                Shoot(col.transform);
                shot = true;
                break;
            }
        }

        if (!shot && cols.Length > 0 && canFire)
            Shoot(cols[0].transform);
    }

    public override bool Build(Node node)
    {
        if (!base.Build(node))
            return false;

        node.GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        _rangeIndicator.SetActive(false);
        _turret.GetComponent<SpriteRenderer>().material.color = Color.white;

        return true;
    }

    public override void Demolish()
    {
        _transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        base.Demolish();
    }

    public override void CheckAvailable(Node node)
    {
        base.CheckAvailable(node);

        if (CanBuild(node))
            _turret.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, .5f);
        else
            _turret.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 0f, .5f);
    }

    private void Shoot(Transform target)
    {
        target.GetComponent<IDamageable>().SetTarget();

        Projectile p = Instantiate(pfProjectile, _turret.position,
                        _turret.rotation, _transform).GetComponent<Projectile>();

        p.damage = damage;
        p.target = target;

        _turret.up = (target.position - _transform.position).normalized;
        _turret.GetComponent<Animator>().SetTrigger("Fire");

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