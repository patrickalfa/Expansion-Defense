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
        _transform = transform;
    }

    private void FixedUpdate()
    {
        if (!target)
            DestroySelf();
        else
        {
            Vector3 dir = (target.position - _transform.position).normalized;
            _transform.up = dir;
            _transform.position += _transform.up * speed * Time.fixedDeltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform == target)
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