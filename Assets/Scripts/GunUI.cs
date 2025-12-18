using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text ammoText;
    PlayerController playerController;

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        playerController.OnGunEquipped += RefreshIcon;
        playerController.OnAmmoAdjusted += RefreshAmmoText;
    }

    void OnDisable()
    {
        playerController.OnGunEquipped -= RefreshIcon;
        playerController.OnAmmoAdjusted -= RefreshAmmoText;
    }

    void RefreshIcon()
    {
        GunData currentGunData = playerController.GetCurrentGunData();

        if (currentGunData == null)
        {
            return;
        }

        iconImage.sprite = currentGunData.GetIcon();

        RefreshAmmoText();
    }

    void RefreshAmmoText()
    {
        GunData currentGunData = playerController.GetCurrentGunData();

        if (currentGunData == null)
        {
            return;
        }

        ammoText.text = $"{playerController.GetAmmo(currentGunData.GetAmmoType())}";
    }
}
