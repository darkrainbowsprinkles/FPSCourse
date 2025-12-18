using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float sprintMultiplier = 2f;
    [SerializeField] Transform gunContainer;
    [SerializeField] GunData defaultGun;
    [SerializeField] AmmoSlot[] ammoSlots;
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

    public void AdjustAmmo(AmmoType ammoType, int amount)
    {
        GetAmmoSlot(ammoType).ammoAmount += amount;
    }

    [System.Serializable]
    class AmmoSlot
    {
        public AmmoType ammoType;
        public int ammoAmount;
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

        AmmoSlot currentSlot = GetAmmoSlot(currentGunData.GetAmmoType());

        if (currentSlot.ammoAmount == 0)
        {
            return;
        }

        InputAction fireInput = playerInput.actions["Fire"];

        if (currentGunData.IsAutomatic() && fireInput.IsPressed())
        {
            Shoot();
        }
        else if (!currentGunData.IsAutomatic() && fireInput.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (var slot in ammoSlots)
        {
            if (slot.ammoType == ammoType)
            {
                return slot;
            }
        }

        return null;
    }

    void Shoot()
    {
        currentGun.Fire(currentGunData.GetDamage(), currentGunData.GetRange());
        timeSinceLastShot = 0f;
        AdjustAmmo(currentGunData.GetAmmoType(), -1);
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
