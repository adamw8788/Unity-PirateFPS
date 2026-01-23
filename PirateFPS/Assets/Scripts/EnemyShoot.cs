using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject projectilePrefab;

    [Header("Shooting")]
    public float shootForce = 18f;
    public float fireRate = 1.5f;
    public float projectileSpawnHeight = 0.4f;
    public float shootRange = 22f;

    private float nextFireTime;

    void Update()
    {
        if (!player) return;

        float sqrDist = (player.position - transform.position).sqrMagnitude;

        if (sqrDist > shootRange * shootRange)
            return;
        
        FacePlayer();
        TryShoot();
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;
        Shoot();
    }

    void Shoot()
    {
        Vector3 spawnPos = transform.position + Vector3.up * projectileSpawnHeight + transform.forward * 0.8f;
        Vector3 dir = (player.position - spawnPos).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(dir)
        );

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * shootForce;

        Collider projCol = proj.GetComponent<Collider>();
        Collider enemyCol = GetComponent<Collider>();
        if (projCol && enemyCol)
            Physics.IgnoreCollision(projCol, enemyCol);
    }
}
