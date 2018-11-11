using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public int damage;
    public float speed;
    public Transform target;
    public float lifetime;

    private Transform _transform;
    private float time;

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

            time += Time.fixedDeltaTime;
            if (time >= lifetime)
                DestroySelf();
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