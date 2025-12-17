using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunData : ScriptableObject
{
    [SerializeField] Gun gunPrefab;
    [SerializeField] float damage = 10f;
    [SerializeField] float cooldown = 0.4f;
    [SerializeField] float range = 30f;
    [SerializeField] bool isAutomatic = false;

    public Gun Spawn(Transform gunContainer)
    {
        Gun gunInstance = Instantiate(gunPrefab, gunContainer);
        return gunInstance;
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

    public bool IsAutomatic()
    {
        return isAutomatic;
    }
}
