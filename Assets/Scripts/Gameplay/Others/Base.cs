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

        m_health = maxHealth;
    }

    private void UpdateGem(float punch)
    {
        float factor = (float)m_health / (float)maxHealth;

        _sprite.material.color = Color.Lerp(Color.grey, Color.white, factor);
        _light.intensity = Mathf.Lerp(0f, 6f, factor);
        _animator.speed = Mathf.Lerp(0f, 1f, factor);

        _transform.DOKill();
        _transform.DOPunchScale(Vector3.one * punch, .1f).OnComplete(() =>
        {
            _transform.localScale = Vector3.one;
        });
    }

    public void Heal(int amount)
    {
        if (m_health == maxHealth)
            return;

        m_health = Mathf.Clamp(m_health + amount, 0, maxHealth);

        UpdateGem(.1f);
    }

    public bool TakeDamage(int damage)
    {
        m_health = Mathf.Clamp(m_health - damage, 0, maxHealth);

        UpdateGem(.25f);

        SoundManager.PlaySound("hurt_crystal");
        GameManager.instance.FreezeFrame(.1f);
        GameManager.instance.ScreenShake(.1f, .1f, 50);

        if (m_health == 0)
            GameManager.instance.EndGame(false);

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
