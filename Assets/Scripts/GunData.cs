using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunData : ScriptableObject
{
    [SerializeField] Gun gunPrefab;
    [SerializeField] AmmoType ammoType;
    [SerializeField] Sprite icon;
    [SerializeField] Texture2D crosshair;
    [SerializeField] bool isAutomatic = false;
    [SerializeField] float damage = 10f;
    [SerializeField] float cooldown = 0.4f;
    [SerializeField] float range = 30f;
    [SerializeField] int magazineSize = 10;

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
}
