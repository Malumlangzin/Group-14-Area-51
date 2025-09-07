using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseMoveSpeed = 7f;
    public float runSpeed = 20f;
    public float crouchSpeed = 3f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseLookSensitivity = 2f;
    public float controllerLookSensitivity = 80f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float standHeight = 1.8f;
    public float crouchHeight = 0.5f;

    [Header("Jump & Gravity Settings")]
    public float jumpHeight = 3f;           
    public float jumpBoostMultiplier = 1.2f;
    public float gravity = -30f;            
    public float fallMultiplier = 2f;       

    [Header("Zoom Settings")]
    public Camera playerCamera;
    public float zoomedOutFOV = 100f;
    public float zoomedInFOV = 5f;
    public float zoomStep = 2f;

    [Header("PickUp Settings")]
    public float pickupRange = 10f;
    public Transform holdPoint;
    private Tools heldObject;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 2f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private float currentSpeed;
    private float normalFOV;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool usingController = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = cameraTransform.GetComponent<Camera>();
        normalFOV = playerCamera.fieldOfView;

        usingController = Gamepad.current != null;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 10f);

        if (heldObject != null)
            heldObject.MoveToHoldPoint(holdPoint.position);
    }

    // ----------------- INPUT CALLBACKS -----------------
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
        if (context.performed) isCrouching = !isCrouching;
        HandleCrouch();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight) * jumpBoostMultiplier;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed) isRunning = true;
        if (context.canceled) isRunning = false;
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
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!context.performed || heldObject == null) return;

        Vector3 dir = cameraTransform.forward;
        Vector3 impulse = dir * throwForce + Vector3.up * throwUpwardBoost;
        heldObject.Throw(impulse);
        heldObject = null;
    }

    // ----------------- HANDLERS -----------------
    private void HandleMovement()
    {
        // Speed selection
        currentSpeed = baseMoveSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        if (isRunning) currentSpeed = runSpeed;

        // Movement
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity + Jump
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // keeps grounded

        velocity.y += gravity * Time.deltaTime;

        // Extra falling force for responsiveness
        if (velocity.y < 0)
            velocity.y += gravity * (fallMultiplier - 1) * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        float sensitivity = usingController ? controllerLookSensitivity : mouseLookSensitivity;

        verticalRotation -= lookInput.y * sensitivity * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x * sensitivity * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        controller.height = isCrouching ? crouchHeight : standHeight;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game called");
    }
}
