using UnityEngine;
using UnityEngine.InputSystem;


public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float gravity = -19.81f;
    public float currentSpeed;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 0.6f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float standHeight = 1.8f;
    public float crouchSpeed = 3f;
    public bool isCrouching = false;

    [Header("Jump Settings")]
    public float jumpHeight = 3.5f;
    public float jumpBoostMultiplier = 1f;
    private bool jumpRequested = false;

    [Header("Run Settings")]
    public float runSpeed = 20f;

    [Header("Zoom Settings")]
    public float zoomedOutFOV = 100f;
    public float zoomedInFOV = 20f;
    public float normalFOV = 60f;
    public Camera playerCamera;
    public float zoomStep = 2f;

    [Header("PickUp Settings")]
    public float pickupRange = 15f;
    public float carrySpeed = 15f;
    private Tools heldObject;
    public Transform holdPoint;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 2f;

    [Header("Animation Settings")]
    public Animator animator;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        currentSpeed = moveSpeed;
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform != null)
            playerCamera = cameraTransform.GetComponent<Camera>();

        if (playerCamera != null)
            playerCamera.fieldOfView = normalFOV;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleLook();

        if (playerCamera != null)
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 10f);

        if (heldObject != null)
            heldObject.MoveToHoldPoint(holdPoint.position);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            isCrouching = true;
        else if (context.canceled)
            isCrouching = false;

        HandleCrouch();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            jumpRequested = true;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentSpeed = runSpeed;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV + 10f, 0.2f);
        }
        else if (context.canceled)
        {
            currentSpeed = moveSpeed;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, 0.2f);
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();

        if (scrollValue != 0)
        {
            normalFOV -= scrollValue * zoomStep;
            normalFOV = Mathf.Clamp(normalFOV, zoomedInFOV, zoomedOutFOV);
        }
    }

    public void OnZoomIn(InputAction.CallbackContext context)
    {
        if (context.performed)
            normalFOV = zoomedInFOV;
    }

    public void OnZoomOut(InputAction.CallbackContext context)
    {
        if (context.performed)
            normalFOV = zoomedOutFOV;
    }

    public void OnNormal(InputAction.CallbackContext context)
    {
        if (context.performed)
            normalFOV = 60f;
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                Tools pickup = hit.collider.GetComponent<Tools>();
                if (pickup != null)
                {
                    pickup.PickUp(holdPoint);
                    heldObject = pickup;
                    currentSpeed = carrySpeed;
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
            currentSpeed = moveSpeed;
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (heldObject == null) return;

        Vector3 dir = cameraTransform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;

        heldObject.Throw(impulse);
        heldObject = null;
    }

    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        float movementMagnitude = new Vector2(moveInput.x, moveInput.y).magnitude;
        animator.SetFloat("Speed", movementMagnitude);
    }

    public void HandleJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = -2f;

            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * jumpBoostMultiplier;
                jumpRequested = false;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (velocity.y < -50f)
            velocity.y = -50f;

        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        float mouseX = lookInput.x * SensitivityManager.value;
        float mouseY = lookInput.y * SensitivityManager.value;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void HandleCrouch()
    {
        if (isCrouching)
        {
            controller.height = crouchHeight;
            currentSpeed = crouchSpeed;
        }
        else
        {
            controller.height = standHeight;
            currentSpeed = moveSpeed;
        }
    }

    public void HandleRun()
    {
        if (moveInput.magnitude > 0)
            currentSpeed = runSpeed;
        else
            currentSpeed = moveSpeed;

        float movementMagnitude = new Vector2(moveInput.x, moveInput.y).magnitude;
        animator.SetFloat("Speed", movementMagnitude);
    }

    public void Quitgame()
    {
        Application.Quit();
    }
}
