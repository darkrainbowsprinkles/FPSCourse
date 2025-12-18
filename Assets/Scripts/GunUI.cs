using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] Image gunIconImage;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] Image ammoIconImage;
    [SerializeField] AmmoIcon[] ammoIcons;
    [SerializeField] RawImage crosshairImage;
    [SerializeField] RawImage scopeImage;
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

    void Update()
    {
        GunData currentGunData = playerController.GetCurrentGunData();

        if (currentGunData == null)
        {
            return;
        }

        if (currentGunData.GetScope() == null)
        {
            return;
        }

        scopeImage.gameObject.SetActive(playerController.IsZooming());
        crosshairImage.gameObject.SetActive(!playerController.IsZooming());
    }

    void OnEnable()
    {
        playerController.OnGunEquipped += RefreshGunData;
        playerController.OnAmmoAdjusted += RefreshAmmoText;
    }

    void OnDisable()
    {
        playerController.OnGunEquipped -= RefreshGunData;
        playerController.OnAmmoAdjusted -= RefreshAmmoText;
    }

    void RefreshGunData()
    {
        GunData currentGunData = playerController.GetCurrentGunData();

        if (currentGunData == null)
        {
            return;
        }

        gunIconImage.sprite = currentGunData.GetIcon();
        ammoIconImage.sprite = GetAmmoIcon(currentGunData.GetAmmoType());
        crosshairImage.texture = currentGunData.GetCrosshair();
        scopeImage.texture = currentGunData.GetScope();

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
