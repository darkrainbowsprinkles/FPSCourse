using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int ammoAmount;

    protected override void OnPickup(PlayerController controller)
    {
        controller.AdjustAmmo(ammoType, ammoAmount);
    }
}
