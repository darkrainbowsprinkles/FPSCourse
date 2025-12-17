using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject muzzleFlashEffect;

    public void Fire(float damage, float range)
    {
        Instantiate(muzzleFlashEffect, muzzle);

        RaycastHit hit;

        bool hitFound = Physics.Raycast(Camera.main.transform.position, 
            Camera.main.transform.forward, out hit, range);
    
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
