/*using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 20f;
    private CharacterController myCC;
    public Animator camAnim;
    private bool isWalking;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float myGravity = -10f;
    void Start()
    {
        myCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        CheckForHeadBob();

        camAnim.SetBool("isWalking", isWalking);
    }

    void GetInput()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();
        inputVector = transform.TransformDirection(inputVector);

        movementVector = (inputVector * playerSpeed) + (Vector3.up * myGravity);
    }

    void MovePlayer()
    {
        myCC.Move(movementVector * Time.deltaTime);
    }

    void CheckForHeadBob()
    {
        if(myCC.velocity.magnitude>0.1f)
        {
            isWalking = true;
        } else
        {
            isWalking = false;
        }
    }
} */

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float playerSpeed = 10f;
    public float momentumDamping = 5f;
    public float gravity = -10f;
    public Animator camAnim;

    private CharacterController myCC;
    private Vector3 movementVector;
    private bool isWalking;

    // New Input System
    private InputAction moveAction;

    void Awake()
    {
        myCC = GetComponent<CharacterController>();

        // Grab the PlayerInput component and the Move action
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        CheckForHeadBob();

        camAnim.SetBool("isWalking", isWalking);
    }

    Vector3 currentMoveDir;
    void GetInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Convert input to world-space direction
        Vector3 inputDir = new Vector3(input.x, 0f, input.y);
        inputDir = transform.TransformDirection(inputDir.normalized);

        if (input.sqrMagnitude > 0.01f)
        {
            // Player is actively inputting → snap to input
            currentMoveDir = inputDir;
        }
        else
        {
            // No input → slowly decay momentum
            currentMoveDir = Vector3.Lerp(
                currentMoveDir,
                Vector3.zero,
                momentumDamping * Time.deltaTime
            );
        }

        movementVector = (currentMoveDir * playerSpeed) + Vector3.up * gravity;
    }

    void MovePlayer()
    {
        myCC.Move(movementVector * Time.deltaTime);
    }

    void CheckForHeadBob()
    {
        isWalking = myCC.velocity.magnitude > 0.1f;
    }
}

