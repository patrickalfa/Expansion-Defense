using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Construction
{
    public int damage;
    public float range;
    public float reloadTime;

    private bool canFire = true;

    private void FixedUpdate()
    {
        Collider2D col = Physics2D.OverlapCircle(_transform.position, range, LayerMask.GetMask("Enemy"));

        if (col && canFire)
        {
            col.GetComponent<IDamageable>().TakeDamage(damage);
            canFire = false;
            Invoke("Reload", reloadTime);
        }
    }

    private void Reload()
    {
        canFire = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_transform.position, range);
    }
#endif
}