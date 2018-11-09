using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Base : MonoBehaviour, IDamageable
{
    public int maxHealth;

    private int m_health;

    private Transform _transform;
    private SpriteRenderer _sprite;
    private Light _light;
    private Animator _animator;

    public float health
    {
        get { return m_health; }
    }

    private void Start()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _light = _transform.GetComponentInChildren<Light>();

        health = maxHealth;
    }

    public void Heal(int amount)
    {
        m_health = Mathf.Clamp(m_health + amount, 0, maxHealth);
    }

    public bool TakeDamage(int damage)
    {
        m_health = Mathf.Clamp(m_health - damage, 0, maxHealth);

        float factor = (float)m_health / (float)maxHealth;

        _sprite.material.color = Color.Lerp(Color.grey, Color.white, factor);
        _light.intensity = Mathf.Lerp(0f, 6f, factor);
        _animator.speed = Mathf.Lerp(0f, 1f, factor);

        _transform.DOKill();
        _transform.DOPunchScale(Vector3.one * .25f, .1f).OnComplete(() =>
        {
            _transform.localScale = Vector3.one;
        });

        return (m_health == 0);
    }

    public bool IsTargeted()
    {
        return false;
    }

    public void SetTarget()
    {
    }
}
