using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float sensitivity = 0.1f;
    public float smoothing = 10f;
    public float maxLookAngle = 30f;

    private float yaw;
    private float pitch;
    private float smoothX;
    private float smoothY;

    private Transform cam;
    private InputAction lookAction;

    void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];

        cam = GetComponentInChildren<Camera>().transform;
    }

    void OnEnable()
    {
        lookAction.Enable();
    }

    void OnDisable()
    {
        lookAction.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        smoothX = Mathf.Lerp(smoothX, look.x * sensitivity, 1f / smoothing);
        smoothY = Mathf.Lerp(smoothY, look.y * sensitivity, 1f / smoothing);

        yaw += smoothX;
        pitch -= smoothY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        // Yaw = player body
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Pitch = camera only
        cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
