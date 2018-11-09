using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyShooter
{
    public float minActiveTime, maxActiveTime;
    public float minRotationTime, maxRotationTime;

    public GameObject pfEnemy;

    protected bool retreating = false;
    protected bool waiting = false;

    protected bool canSpawn = true;

    protected override void Move()
    {
        float distance = Vector2.Distance(_target.position, _transform.position);

        if (retreating)
        {
            if (distance > range * 3f)
            {
                if (!waiting)
                {
                    waiting = true;
                    Invoke("CancelRetreat", Random.Range(minActiveTime, maxActiveTime));
                }

                _rigidbody.velocity = Vector2.zero;

                _transform.RotateAround(_target.position, Vector3.forward, rotationSpeed * 1f * Time.fixedDeltaTime);

                _transform.up = (_target.position - transform.position).normalized;

                if (canSpawn)
                    SpawnEnemy();
            }
            else
            {
                Vector2 dir = (transform.position - _target.position).normalized;

                Collider2D[] cols = Physics2D.OverlapCircleAll(_transform.position, 1f, LayerMask.GetMask("Enemy"));
                foreach (Collider2D col in cols)
                    dir += (Vector2)(_transform.position - col.transform.position).normalized * .25f;

                _rigidbody.velocity = dir.normalized * speed * 3f;
                _transform.up = dir;
            }
        }
        else
        {
            if (distance <= range && !waiting)
            {
                waiting = true;
                Invoke("Retreat", Random.Range(minActiveTime, maxActiveTime));
            }

            base.Move();
        }
    }

    protected void Retreat()
    {
        retreating = true;
        waiting = false;
    }

    protected void CancelRetreat()
    {
        retreating = false;
        waiting = false;
    }

    protected void SpawnEnemy()
    {
        Enemy e = Instantiate(pfEnemy, _transform.position, _transform.rotation,
            Spawner.instance.transform).GetComponent<Enemy>();

        canSpawn = false;
        Invoke("ReloadSpawning", reloadTime);
    }

    protected void ReloadSpawning()
    {
        canSpawn = true;
    }
}
