using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    private bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        EnemyHealth enemy =
            collision.gameObject.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

