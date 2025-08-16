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
    public float jumpBoostMultiplier = 1.5f;

    [Header("Run Settings")]
    public float runSpeed = 10f;

    [Header("Zoom Settings")] 
    public float zoomedOutFOV = 100f;
    public float zoomedInFOV = 5f;
    public float normalFOV;
    public Camera playerCamera;
    public float zoomStep = 2f;

    [Header("PickUp Settings")]
    public float pickupRange = 5f;
    private PickUpObject heldObject;
    public Transform holdPoint;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float throwUpwardBoost = 2f;

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

    }

    private void Update()
    {
        HandleMovement();
        HandleLook();

        playerCamera = cameraTransform.GetComponent<Camera>();
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * 10f);

        if(heldObject != null)
        {

            heldObject.MoveToHoldPoint(holdPoint.position);
        }
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
              velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight) * jumpBoostMultiplier; 
          }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed = runSpeed;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV + 10f, 0.2f);
        }
        else if (context.canceled)
        {
            moveSpeed = 5f;
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

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (heldObject == null)
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickup = hit.collider.GetComponent<PickUpObject>();

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
            moveSpeed = crouchSpeed; 
        }
        else
        {
            controller.height = standHeight;
            moveSpeed = 5f; 
        }
    }

    public void HandleRun()
    {
        if (moveInput.magnitude > 0)
        {
            moveSpeed = runSpeed; 
        }
        else
        {
            moveSpeed = 5f; 
        }
    }

    public void HandleJump()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 1f;
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime); 
    }

}
