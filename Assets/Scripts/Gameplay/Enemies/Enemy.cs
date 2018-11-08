using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour, IDamageable
{
    public float speed;
    public int damage;
    public int maxHealth;
    public bool targeted;

    private float health;
    private bool canMove = true;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;

        Vector2 dir = (GameManager.instance._baseNode.transform.position - 
                        transform.position).normalized;

        Collider2D[] cols = Physics2D.OverlapCircleAll(_transform.position, 1f, LayerMask.GetMask("Enemy"));
        foreach (Collider2D col in cols)
            dir += (Vector2)(_transform.position - col.transform.position).normalized * .25f;

        _rigidbody.velocity = dir.normalized * speed;
        _transform.up = dir;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Base"))
        {
            GameManager.instance._baseNode.transform.Find("Content").
                   GetComponent<IDamageable>().TakeDamage(damage);

            Spawner.instance.enemiesAlive--;

            _transform.DOKill();

            Destroy(gameObject);
        }
    }

    public bool TakeDamage(int damage)
    {
        if (health == 0)
            return true;

        targeted = false;

        health = Mathf.Clamp(health - damage, 0, maxHealth);
        canMove = false;
        _rigidbody.velocity = Vector2.zero;

        _transform.DOKill();

        if (health == 0)
        {
            GetComponent<Collider2D>().enabled = false;
            _transform.DOScale(0f, .25f).OnComplete(() =>
            {
                Destroy(gameObject);
            });

            Spawner.instance.enemiesAlive--;

            return true;
        }

        _transform.DOPunchScale(Vector3.one * .25f, .25f).OnComplete(() =>
        {
            canMove = true;
        });

        return false;
    }
}
