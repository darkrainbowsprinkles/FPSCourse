using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] bool destroyOnPick = false;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController controller))
        {
            OnPickup(controller);

            if (destroyOnPick)
            {
                Destroy(gameObject);
            }
        }
    }

    protected abstract void OnPickup(PlayerController controller);
}
