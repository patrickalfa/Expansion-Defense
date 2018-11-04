using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Base : MonoBehaviour, IDamageable
{
    public int maxHealth;

    private int health;

    private Transform _transform;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();

        health = maxHealth;
    }

    public bool TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        _sprite.color = Color.Lerp(Color.black, Color.cyan, (float)health / (float)maxHealth);

        _transform.DOKill();
        _transform.DOPunchScale(Vector3.one * .25f, .1f);

        return (health == 0);
    }
}
