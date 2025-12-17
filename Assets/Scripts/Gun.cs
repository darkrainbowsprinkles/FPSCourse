using UnityEngine;

public class Gun : MonoBehaviour
{
    public void Fire()
    {
        bool hitFound = Physics.Raycast(Camera.main.transform.position, 
            Camera.main.transform.forward, out RaycastHit hit, 100);
    
        if (!hitFound)
        {
            return;
        }

        print(hit.collider.name);
    }
}
