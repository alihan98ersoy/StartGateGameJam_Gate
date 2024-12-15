using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Karakter : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private CharacterController controller;

    [SerializeField] private Camera cam;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] public float lookSensitivity = 1.0f;

    private float xRotation = 0f;

    // Movement Vars
    private Vector3 velocity;
    public float gravity = -9.81f;
    private bool grounded;

    // Zoom Vars - Zoom code adapted from @torahhorse's First Person Drifter scripts.
    public float zoomFOV = 35.0f;
    public float zoomSpeed = 9f;
    private float targetFOV;
    private float baseFOV;

    // Crouch Vars
    private float initHeight;
    [SerializeField] private float crouchHeight;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inputActions = new InputSystem_Actions();

        inputActions.Player.BirinciEkipman.performed += context => EkipmanDegisTiklandi(EKIPMANLAR.SCANNER);
        inputActions.Player.IkinciEkipman.performed += context => EkipmanDegisTiklandi(EKIPMANLAR.CROSS);
        inputActions.Player.Interact.performed += OnInteractPerformed;

       
       // Cursor.visible = true;
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("!!!!!!! OnInteractPerformed");

        if (UIManagers.Instance.ruhBilgiPanel.activeSelf) 
        {
            if (UIManagers.Instance.suankiEkipman == EKIPMANLAR.COOLDOWN) 
            {
                UIManagers.Instance.InteractionTextDegistir("Lütfen ekipmanýn soðumasýný bekle");

                StartCoroutine(Bekle(2f));

                IEnumerator Bekle(float sayi)
                {
                    yield return new WaitForSeconds(sayi);

                    if(UIManagers.Instance.InteractionText.text == "Lütfen ekipmanýn soðumasýný bekle")
                        UIManagers.Instance.InteractionTextDegistir("");
                }
            }
            else if(UIManagers.Instance.suankiEkipman == EKIPMANLAR.SCANNER)
            {
                if(GameManager.Instance.suankiKurban.ruhu.isimgizlimi == true)
                    GameManager.Instance.KurbaniTara(GameManager.Instance.suankiKurban);
            }
            else if (UIManagers.Instance.suankiEkipman == EKIPMANLAR.CROSS)
            {
                    GameManager.Instance.KurbanEt(GameManager.Instance.suankiKurban);
            }
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        initHeight = controller.height;
        //CenterCursor();
        SetBaseFOV(cam.fieldOfView);
    }
    private void CenterCursor()
    {
        // Ýmleci ekranýn ortasýna yerleþtir
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        //Cursor.SetCursor(null, screenCenter, CursorMode.Auto);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Update()
    {
        DoMovement();
        DoLooking();
        DoZoom();
        DoCrouch();
    }

    private void DoLooking()
    {
        if (UIManagers.Instance.CompletePanel.activeSelf)
            return;

        Vector2 looking = GetPlayerLook();
        float lookX = looking.x * lookSensitivity * Time.deltaTime;
        float lookY = looking.y * lookSensitivity * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    private void DoMovement()
    {
        grounded = controller.isGrounded;
        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector2 movement = GetPlayerMovement();
        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        controller.Move(move * movementSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void DoZoom()
    {
        if (inputActions.Player.Zoom.ReadValue<float>() > 0)
        {
            targetFOV = zoomFOV;
        }
        else
        {
            targetFOV = baseFOV;
        }
        UpdateZoom();
    }

    private void DoCrouch()
    {
        if (inputActions.Player.Crouch.ReadValue<float>() > 0)
        {
            controller.height = crouchHeight;
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), 2.0f, -1))
            {
                controller.height = crouchHeight;
            }
            else
            {
                controller.height = initHeight;
            }
        }
    }

    private void UpdateZoom()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void SetBaseFOV(float fov)
    {
        baseFOV = fov;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerLook()
    {
        return inputActions.Player.Look.ReadValue<Vector2>();
    }

    void EkipmanDegisTiklandi(EKIPMANLAR ekipman) 
    {
        if(UIManagers.Instance.suankiEkipman != EKIPMANLAR.COOLDOWN)
            UIManagers.Instance.EkipmanIkonuDegistir(ekipman);
    }

    

}