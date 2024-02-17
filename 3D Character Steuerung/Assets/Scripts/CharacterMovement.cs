using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    bool jumpPressed;

    float stickDeadzone = 0.1f;

    void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.magnitude > 0.1f; // Using a deadzone for movement
        };
        input.CharacterControls.Movement.canceled += _ => movementPressed = false;

        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Jump.performed += ctx => Jump();
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleRotation()
    {
        if (currentMovement != Vector2.zero)
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);
            Vector3 positionToLookAt = currentPosition + newPosition;
            transform.LookAt(positionToLookAt);
        }
    }

    void HandleMovement()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

        if (jumpPressed && !isJumping)
        {
            animator.SetBool(isJumpingHash, true);
            jumpPressed = false; // Reset jumpPressed after jump
        }
        else if (!jumpPressed && isJumping)
        {
            animator.SetBool(isJumpingHash, false);
        }
    }

    void Jump()
    {
        jumpPressed = true;
    }

    void OnEnable()
    {
        input.CharacterControls.Enable();
    }
    void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
