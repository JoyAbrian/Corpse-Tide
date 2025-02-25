using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class WeaponUIManager : MonoBehaviour
{
    public Image weaponIconImage;
    public Image crosshair;

    [Header("Ammo UI")]
    public GameObject ammoText;
    public TextMeshProUGUI ammoLeftText;
    public TextMeshProUGUI totalAmmoText;
    public TextMeshProUGUI reloadingText;

    [Header("Fire Mode UI")]
    public GameObject fireModeText;
    public TextMeshProUGUI semiAutoText;
    public TextMeshProUGUI fullAutoText;

    [Header("Crosshair Animation")]
    public float crosshairExpandAmount = 10f;
    public float crosshairRecoverySpeed = 2f;
    private Vector2 originalCrosshairSize;

    private Coroutine noAmmoCoroutine;

    private void Start()
    {
        originalCrosshairSize = crosshair.rectTransform.sizeDelta;
    }

    public void UpdateAmmoUI(int currentAmmo, int totalAmmo)
    {
        ammoLeftText.text = currentAmmo.ToString();
        totalAmmoText.text = totalAmmo.ToString();
    }

    public void UpdateFireModeUI(bool isFullAuto, bool hasFireType)
    {
        if (hasFireType)
        {
            fireModeText.SetActive(true);

            if (isFullAuto)
            {
                semiAutoText.fontStyle = FontStyles.Normal;
                semiAutoText.color = Color.black;
                fullAutoText.fontStyle = FontStyles.Bold;
                fullAutoText.color = Color.white;
            }
            else
            {
                semiAutoText.fontStyle = FontStyles.Bold;
                semiAutoText.color = Color.white;
                fullAutoText.fontStyle = FontStyles.Normal;
                fullAutoText.color = Color.black;
            }
        }
        else
        {
            fireModeText.SetActive(false);
        }
    }

    public void ShowReloadingText(string text)
    {
        reloadingText.gameObject.SetActive(true);
        reloadingText.text = text;
    }

    public void HideReloadingText()
    {
        reloadingText.gameObject.SetActive(false);
    }

    public void ShowNoAmmoWarning()
    {
        ammoText.gameObject.SetActive(false);

        if (noAmmoCoroutine != null) StopCoroutine(noAmmoCoroutine);
        reloadingText.gameObject.SetActive(true);
        reloadingText.text = "No Ammo!";
        noAmmoCoroutine = StartCoroutine(AnimateNoAmmoText());
    }

    private IEnumerator AnimateNoAmmoText()
    {
        float duration = 1f;
        float elapsedTime = 0f;
        Color startColor = Color.red;
        Color endColor = Color.black;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.PingPong(elapsedTime / duration, 1);
            reloadingText.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
    }

    public void ExpandCrosshair()
    {
        Vector2 newSize = originalCrosshairSize + new Vector2(crosshairExpandAmount, crosshairExpandAmount);
        crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, newSize, Time.deltaTime * 10f);
    }

    public void RecoverCrosshair()
    {
        crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, originalCrosshairSize, Time.deltaTime * crosshairRecoverySpeed);
    }

    public void UpdateWeaponIcon(Sprite newIcon)
    {
        if (weaponIconImage != null)
        {
            weaponIconImage.sprite = newIcon;
        }
    }

    public void SelectWeapon(Weapon weapon)
    {
        UpdateWeaponIcon(weapon.weaponIcon);
        UpdateAmmoUI(GlobalVariables.playerPrimaryAmmo, GlobalVariables.playerPrimaryTotalAmmo);
        UpdateFireModeUI(WeaponFire.isFullAuto, weapon.hasFireType);
    }
}