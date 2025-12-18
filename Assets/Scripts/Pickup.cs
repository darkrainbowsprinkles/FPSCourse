using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController controller))
        {
            OnPickup(controller);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickup(PlayerController controller);
}
