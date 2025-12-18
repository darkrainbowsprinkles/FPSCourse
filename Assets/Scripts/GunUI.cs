using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] Image gunIconImage;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] Image ammoIconImage;
    [SerializeField] AmmoIcon[] ammoIcons;
    PlayerController playerController;

    [System.Serializable]
    class AmmoIcon
    {
        public AmmoType ammoType;
        public Sprite icon;
    }

    void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        playerController.OnGunEquipped += RefreshIcons;
        playerController.OnAmmoAdjusted += RefreshAmmoText;
    }

    void OnDisable()
    {
        playerController.OnGunEquipped -= RefreshIcons;
        playerController.OnAmmoAdjusted -= RefreshAmmoText;
    }

    void RefreshIcons()
    {
        GunData currentGunData = playerController.GetCurrentGunData();

        if (currentGunData == null)
        {
            return;
        }

        gunIconImage.sprite = currentGunData.GetIcon();
        ammoIconImage.sprite = GetAmmoIcon(currentGunData.GetAmmoType());

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

    Sprite GetAmmoIcon(AmmoType ammoType)
    {
        foreach (var ammoIcon in ammoIcons)
        {
            if (ammoIcon.ammoType == ammoType)
            {
                return ammoIcon.icon;
            }
        }

        return null;
    }
}
