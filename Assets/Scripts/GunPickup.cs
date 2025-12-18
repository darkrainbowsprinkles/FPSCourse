using UnityEngine;

public class GunPickup : Pickup
{
    [SerializeField] GunData gunData;

    protected override void OnPickup(PlayerController controller)
    {
        controller.EquipGun(gunData);
    }
}
