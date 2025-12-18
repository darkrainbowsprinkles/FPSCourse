using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float sprintMultiplier = 2f;
    [SerializeField] CinemachineCamera firstPersonCamera;
    [SerializeField] CinemachineInputAxisController axisController;
    [SerializeField] Transform gunContainer;
    [SerializeField] GunData defaultGun;
    [SerializeField] AmmoSlot[] ammoSlots;
    PlayerInput playerInput;
    CharacterController controller;
    GunData currentGunData;
    Gun currentGun;
    bool isZooming;
    float defaultFOV;
    float defaultRotationSpeed;
    float timeSinceLastShot = Mathf.Infinity;
    Dictionary<AmmoType, int> ammoLookup;
    
    public event Action OnGunEquipped;
    public event Action OnAmmoAdjusted;

    public GunData GetCurrentGunData()
    {
        return currentGunData;
    }

    public bool IsZooming()
    {
        return isZooming;
    }
 
    public void EquipGun(GunData gunData)
    {
        if (currentGun != null)
        {
            Destroy(currentGun.gameObject);
        }

        currentGun = gunData.Spawn(gunContainer);
        currentGunData = gunData;
        OnGunEquipped?.Invoke();
    }

    public void AdjustAmmo(AmmoType ammoType, int amount)
    {
        if (!ammoLookup.ContainsKey(ammoType))
        {
            Debug.LogError($"Ammo type {ammoType} not found");
            return;
        }

        ammoLookup[ammoType] += amount;
        OnAmmoAdjusted?.Invoke();
    }

    public int GetAmmo(AmmoType ammoType)
    {
        if (!ammoLookup.ContainsKey(ammoType))
        {
            Debug.LogError($"Ammo type {ammoType} not found");
            return -1;
        }

        return ammoLookup[ammoType];
    }

    [Serializable]
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
        defaultFOV = firstPersonCamera.Lens.FieldOfView;
        defaultRotationSpeed = axisController.Controllers[0].Input.Gain;
        FillAmmoLookup();
        EquipGun(defaultGun);
    }

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        HandleMovement();
        HandleFiring();
        HandleZoom();
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

    void Shoot()
    {
        currentGun.Fire(currentGunData.GetDamage(), currentGunData.GetRange());
        timeSinceLastShot = 0f;
        AdjustAmmo(currentGunData.GetAmmoType(), -1);
    }

    void HandleZoom()
    {
        if (!currentGunData.CanZoom())
        {
            return;
        }

        if (playerInput.actions["Zoom"].IsPressed())
        {
            isZooming = true;
            firstPersonCamera.Lens.FieldOfView = currentGunData.GetZoomAmount();
            SetCameraRotationSpeed(currentGunData.GetZoomRotationSpeed());
        }
        else
        {
            isZooming = false;
            firstPersonCamera.Lens.FieldOfView = defaultFOV;
            SetCameraRotationSpeed(defaultRotationSpeed);
        }
    }

    void SetCameraRotationSpeed(float speed)
    {
        axisController.Controllers[0].Input.Gain = speed;
        axisController.Controllers[1].Input.Gain = -speed;
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
