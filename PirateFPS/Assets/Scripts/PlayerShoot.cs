using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileGunTutorial : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public GameObject projectilePrefab;

    [Header("Shooting")]
    public float shootForce = 25f;
    public float fireRate = 0.2f;

    private PlayerInput playerInput;
    private InputAction fireAction;

    private float nextFireTime;

    public GunAnimation gunAnimator;

    public GunExplosion gunExplosion;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Shoot"];
    }

    void OnEnable()
    {
        fireAction.Enable();
    }

    void OnDisable()
    {
        fireAction.Disable();
    }

    void Update()
    {
        if (fireAction.triggered && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        // 1. Ray from center of the screen (true aim)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100f);

        // 2. Offset spawn position (matches UI gun)
        Vector3 spawnPosition =
            playerCamera.transform.position +
            (playerCamera.transform.forward * 0.5f) +
            (playerCamera.transform.right * 0.35f);

        // 3. Recalculate direction from spawn → target
        Vector3 direction = (targetPoint - spawnPosition).normalized;

        // 4. Spawn projectile
        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * shootForce;

        // 5. Ignore collision with player
        Collider projCol = projectile.GetComponent<Collider>();
        Collider playerCol = GetComponent<Collider>();
        if (projCol && playerCol)
            Physics.IgnoreCollision(projCol, playerCol);

        // 6. UI recoil
        gunAnimator?.PlayRecoil();

        // 7. UI gun explosion
        gunExplosion?.Play();

        // Debug
        Debug.DrawRay(spawnPosition, direction * 5f, Color.red, 0.5f);
        Debug.Log(direction);
    }
}
