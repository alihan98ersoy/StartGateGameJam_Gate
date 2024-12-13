using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Karakter : MonoBehaviour
{
    public Transform cameraTransform;

    public float moveSpeed = 0f;  // Karakterin hareket hýzý
    public float sprintSpeed = 3f; // Merdiven týrmanma hýzý
    public float walkSpeed = 1.5f;  // Karakterin yürüyüþ hýzý
    public float lookSpeed = 2f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalRotation = 0f;

    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        //Cursor.lockState = CursorLockMode.Locked;

        // Hareket aksiyonu tanýmlamasý
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector3.zero;
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Merdiven etkileþimi için
        controls.Player.Interact.performed += OnInteractPerformed;

        //Koþma
        controls.Player.Sprint.started += OnSprintStarted;
        controls.Player.Sprint.canceled += OnSprintCanceled;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        NeredeHareketOradaBereket();
    }

    private void OnEnable()
    {
        // Input Action'larý etkinleþtir
        controls.Enable();
    }

    private void OnDisable()
    {
        // Input Action'larý devre dýþý býrak
        controls.Disable();
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("OnSprintCanceled");
        if (moveSpeed != walkSpeed)
        {
            moveSpeed = walkSpeed;
        }
    }

    private void OnSprintStarted(InputAction.CallbackContext context)
    {
        Debug.Log("OnSprintStarted");
        if (moveSpeed != sprintSpeed)
        {
            moveSpeed = sprintSpeed;
        }
    }

    private void NeredeHareketOradaBereket()
    {
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.y * moveSpeed);
            //GetComponent<CharacterAnimationController>().PlayAnimation("A_Walk_F_Femn");
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            //animator.SetFloat("Speed", 0);
        }

        // Mouse ile etrafa bakma
        float mouseX = lookInput.x * lookSpeed;
        float mouseY = lookInput.y * lookSpeed;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("!!!!!int");
    }

}
