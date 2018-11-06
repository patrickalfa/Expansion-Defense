using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public Transform target;

    private Transform _transform;

    private void Start()
    {
        _transform = _transform;
    }

    private void FixedUpdate()
    {
        if (!_target)
            DestroySelf();
        else
        {
            Vector3 dir = (_target.position - _transform.position).normalized;
            _transform.Translate(dir * speed * Time.fixedDeltaTime);
            _transform.up = dir;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<IDamageable>().TakeDamage(damage);
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}