using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunData : ScriptableObject
{
    [SerializeField] Gun gunPrefab;
    [SerializeField] AmmoType ammoType;
    [SerializeField] Sprite icon;
    [SerializeField] Texture2D crosshair;
    [SerializeField] bool isAutomatic = false;
    [SerializeField] bool canZoom = false;
    [SerializeField] float damage = 10f;
    [SerializeField] float cooldown = 0.4f;
    [SerializeField] float range = 30f;
    [SerializeField] int magazineSize = 10;
    [SerializeField] float zoomAmount = 30f;
    [SerializeField] float zoomRotationSpeed = 0.5f;

    public Gun Spawn(Transform gunContainer)
    {
        Gun gunInstance = Instantiate(gunPrefab, gunContainer);
        return gunInstance;
    }

    public AmmoType GetAmmoType()
    {
        return ammoType;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public Texture2D GetCrosshair()
    {
        return crosshair;
    }

    public bool IsAutomatic()
    {
        return isAutomatic;
    }

    public bool CanZoom()
    {
        return canZoom;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public float GetRange()
    {
        return range;
    }

    public int GetMagazineSize()
    {
        return magazineSize;
    }
    
    public float GetZoomAmount()
    {
        return zoomAmount;
    }

    public float GetZoomRotationSpeed()
    {
        return zoomRotationSpeed;
    }
}
