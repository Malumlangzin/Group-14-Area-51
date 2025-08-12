using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float standHeight = 1.8f;
    public float crouchSpeed = 3f;
    public bool isCrouching = false;

    [Header("Jump Settings")]
    public float jumpHeight = 5f;

    [Header("Run Settings")]
    public float runSpeed = 15f;

    [Header("Zoom Settings")]
    public float zoomedOutFOV = 100f;
    public float zoomedInFOV = 10f;
    public float normalFOV;
    public Camera playerCamera;
    public float zoomStep = 2f;

    [Header("PickUp Settings")]
    public float pickupRange = 3f;
    public float holdDistance = 2f;
    public float pickupForce = 150f;
    public float throwForce = 10f;

    private Rigidbody heldObject;
    private Transform holdPoint;



    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = cameraTransform.GetComponent<Camera>();
        normalFOV = playerCamera.fieldOfView;

        holdPoint = new GameObject("HoldPoint").transform;
        holdPoint.SetParent(cameraTransform);
        holdPoint.localPosition = new Vector3(0f, -0.2f, holdDistance);
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleHeldObject();

        playerCamera = cameraTransform.GetComponent<Camera>();
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 10f);
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
        {
            isCrouching = true;
        }
        else if (context.canceled)
        {
            isCrouching = false;
        }

        HandleCrouch();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(-2f * gravity * 3f);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed = runSpeed;
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();

        if (scrollValue != 0)
        {
            normalFOV -= scrollValue * zoomStep; // scroll up = zoom in
            normalFOV = Mathf.Clamp(normalFOV, zoomedInFOV, zoomedOutFOV);
        }
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.started && heldObject != null)
        {
            ThrowObject();
        }
    }


    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit,
        verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void HandleCrouch()
    {
        if (isCrouching)
        {
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed; //slow down speed
        }
        else
        {
            controller.height = standHeight;
            moveSpeed = 5f; // Reset to normal speed
        }
    }

    public void HandleRun()
    {
        if (moveInput.magnitude > 0)
        {
            moveSpeed = runSpeed; // Set to run speed when moving
        }
        else
        {
            moveSpeed = 5f; // Reset to normal speed when not moving
        }
    }

    public void HandleJump()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime); //handles jump
    }

    private void HandleHeldObject()
    {
        if (heldObject != null)
        {
            Vector3 moveDirection = (holdPoint.position - heldObject.position);
            float distance = moveDirection.magnitude;

            float forceMultiplier = Mathf.Clamp(distance * 20f, 10f, 50f);
            heldObject.linearVelocity = moveDirection * forceMultiplier * Time.deltaTime;

            heldObject.angularVelocity = Vector3.zero;
        }
    }

    private void TryPickup()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickupItem"))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    heldObject = rb;
                    heldObject.useGravity = false;
                    heldObject.linearDamping = 0f;   // slows movement
                    heldObject.angularDamping = 2f; // stops spinning
                }
            }
        }
    }

    private void DropObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject.angularDamping = 0f;
        heldObject = null;
    }

    private void ThrowObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            heldObject.useGravity = true;
            heldObject.linearDamping = 20f;
            heldObject.angularDamping = 2f;

            heldObject.AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);

            heldObject = null;
        }


    }
}


