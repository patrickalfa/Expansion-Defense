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
    private Light _light;

    private void Start()
    {
        _transform = transform;
        _sprite = GetComponent<SpriteRenderer>();
        _light = _transform.GetComponentInChildren<Light>();

        health = maxHealth;
    }

    public bool TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        _sprite.color = Color.Lerp(Color.black, Color.white, (float)health / (float)maxHealth);
        _light.intensity = Mathf.Lerp(0f, 3f, (float)health / (float)maxHealth);

        _transform.DOKill();
        _transform.DOPunchScale(Vector3.one * .25f, .1f).OnComplete(() =>
        {
            _transform.localScale = Vector3.one;
        });

        return (health == 0);
    }
}
