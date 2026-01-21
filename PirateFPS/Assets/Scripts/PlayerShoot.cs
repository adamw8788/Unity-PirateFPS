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
        Vector3 spawnPosition = playerCamera.transform.position;

        // Ray through the center of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 direction = ray.direction;
        
        direction.Normalize();

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * shootForce;

        // Debug line
        Debug.DrawRay(spawnPosition, direction * 5f, Color.red, 0.5f);
    }
}
