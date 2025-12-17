using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] GunData gunData;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController controller))
        {
            controller.EquipGun(gunData);
            Destroy(gameObject);
        }
    }
}
