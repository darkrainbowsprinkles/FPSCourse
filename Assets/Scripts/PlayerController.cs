using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float sprintMultiplier = 2f;
    [SerializeField] Transform gunContainer;
    [SerializeField] GunData defaultGun;
    PlayerInput playerInput;
    CharacterController controller;
    GunData currentGunData;
    Gun currentGun;
    float timeSinceLastShot = Mathf.Infinity;

    public void EquipGun(GunData gunData)
    {
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
        }

        currentGun = gunData.Spawn(gunContainer);
        currentGunData = gunData;
    }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        EquipGun(defaultGun);
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        HandleMovement();
        HandleFiring();
    }

    void HandleFiring()
    {
        if (timeSinceLastShot < currentGunData.GetCooldown())
        {
            return;
        }

        InputAction fireInput = playerInput.actions["Fire"];

        if (currentGunData.IsAutomatic() && fireInput.IsPressed())
        {
            currentGun.Fire(currentGunData.GetDamage(), currentGunData.GetRange());
            timeSinceLastShot = 0f;
        }
        else if (!currentGunData.IsAutomatic() && fireInput.WasPressedThisFrame())
        {
            currentGun.Fire(currentGunData.GetDamage(), currentGunData.GetRange());
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
