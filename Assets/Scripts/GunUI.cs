using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    PlayerController playerController;

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        GunData currentGunData = playerController.GetCurrentGunData();

        if (currentGunData == null)
        {
            return;
        }

        iconImage.sprite = currentGunData.GetIcon();
    }
}
