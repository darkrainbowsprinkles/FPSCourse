using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float damage = 10f;

    public void Fire()
    {
        RaycastHit hit;

        bool hitFound = Physics.Raycast(Camera.main.transform.position, 
            Camera.main.transform.forward, out hit, 100);
    
        if (!hitFound)
        {
            return;
        }

        if (hit.collider.TryGetComponent(out Health enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
