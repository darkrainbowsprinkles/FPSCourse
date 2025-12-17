using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject muzzleFlashEffect;
    [SerializeField] GameObject hitEffect;
    Animator animator;

    public void Fire(float damage, float range)
    {
        animator.Play("Gun Shoot", 0, 0f);

        Instantiate(muzzleFlashEffect, muzzle);

        RaycastHit hit;

        bool hitFound = Physics.Raycast(Camera.main.transform.position, 
            Camera.main.transform.forward, out hit, range);
    
        if (!hitFound)
        {
            return;
        }

        Instantiate(hitEffect, hit.point, Quaternion.identity);

        if (hit.collider.TryGetComponent(out Health enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
}
