using System.Collections.Generic;
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
    Dictionary<AmmoType, int> ammoLookup;

    public GunData GetCurrentGunData()
    {
        return currentGunData;
    }
 
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
        if (!ammoLookup.ContainsKey(ammoType))
        {
            Debug.LogError($"Ammo type {ammoType} not found");
            return;
        }

        ammoLookup[ammoType] += amount;
        
        print(GetAmmo(ammoType));
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
        FillAmmoLookup();
        EquipGun(defaultGun);
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        HandleMovement();
        HandleFiring();
    }

    void FillAmmoLookup()
    {
        ammoLookup = new Dictionary<AmmoType, int>();

        foreach (var slot in ammoSlots)
        {
            ammoLookup[slot.ammoType] = slot.ammoAmount;
        }
    }

    void HandleFiring()
    {
        if (timeSinceLastShot < currentGunData.GetCooldown())
        {
            return;
        }

        AmmoType currentAmmoType = currentGunData.GetAmmoType();

        if (GetAmmo(currentAmmoType) == 0)
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

    int GetAmmo(AmmoType ammoType)
    {
        if (!ammoLookup.ContainsKey(ammoType))
        {
            Debug.LogError($"Ammo type {ammoType} not found");
            return -1;
        }

        return ammoLookup[ammoType];
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
