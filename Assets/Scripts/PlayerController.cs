using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float sprintMultiplier = 2f;
    [SerializeField] Transform gunContainer;
    [SerializeField] GunData gunData;
    PlayerInput playerInput;
    CharacterController controller;
    Gun gun;
    float timeSinceLastShot = Mathf.Infinity;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        gun = gunData.Spawn(gunContainer);
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        HandleMovement();
        HandleFiring();
    }

    void HandleFiring()
    {
        if (timeSinceLastShot < gunData.GetCooldown())
        {
            return;
        }

        InputAction fireInput = playerInput.actions["Fire"];

        if (gunData.IsAutomatic() && fireInput.IsPressed())
        {
            gun.Fire(gunData.GetDamage(), gunData.GetRange());
            timeSinceLastShot = 0f;
        }
        else if (!gunData.IsAutomatic() && fireInput.WasPressedThisFrame())
        {
            gun.Fire(gunData.GetDamage(), gunData.GetRange());
            timeSinceLastShot = 0f;
        }
    }

    void HandleMovement()
    {
        float speed = movementSpeed;

        if (playerInput.actions["Sprint"].IsPressed())
        {
            speed = movementSpeed * sprintMultiplier;
        }

        controller.Move(speed * Time.deltaTime * CalculateMovement());
    }

    Vector3 CalculateMovement()
    {
        Vector2 inputValue = playerInput.actions["Move"].ReadValue<Vector2>();
        
        Vector3 right = (Camera.main.transform.right * inputValue.x).normalized;
        right.y = 0;

        Vector3 forward = (Camera.main.transform.forward * inputValue.y).normalized;
        forward.y = 0;

        return forward + right;
    }
}
