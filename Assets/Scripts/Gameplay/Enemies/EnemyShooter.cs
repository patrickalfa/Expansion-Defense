using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    public float range;
    public float reloadTime;
    public float rotationSpeed;
    public GameObject pfProjectile;

    protected bool canFire = true;

    protected override void Move()
    {
        float distance = Vector2.Distance(_target.position, _transform.position);

        if (distance <= range)
        {
            _rigidbody.velocity = Vector2.zero;

            _transform.RotateAround(_target.position,
                Vector3.forward, rotationSpeed * Time.fixedDeltaTime);

            if (canFire)
                Shoot(_target);
        }
        else
            base.Move();

    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        return;
    }

    protected void Shoot(Transform target)
    {
        Projectile p = Instantiate(pfProjectile, _transform.position + (_transform.up * .25f),
                        _transform.rotation, _transform.parent).GetComponent<Projectile>();

        p.damage = damage;
        p.target = target;

        _transform.up = (target.position - _transform.position).normalized;
        _transform.GetComponent<Animator>().SetTrigger("Fire");

        canFire = false;
        Invoke("Reload", reloadTime);
    }

    protected void Reload()
    {
        canFire = true;
    }
}
