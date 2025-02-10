using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class WeaponFire : MonoBehaviour
{
    public Weapon weapon;
    public GameObject impactEffect;
    public Image crosshair;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoLeftText;
    public TextMeshProUGUI totalAmmoText;
    public GameObject ammoText;
    public TextMeshProUGUI reloadingText;
    public TextMeshProUGUI semiAutoText;
    public TextMeshProUGUI fullAutoText;

    [Header("Recoil & Spread")]
    public float baseSpread = 0.01f;
    public float maxSpread = 0.1f;
    public float spreadIncreaseRate = 0.005f;
    public float spreadRecoveryRate = 0.02f;
    private float currentSpread = 0f;

    [Header("Crosshair Animation")]
    public float crosshairExpandAmount = 10f;
    public float crosshairRecoverySpeed = 2f;
    private Vector2 originalCrosshairSize;
    private bool isFiring = false;

    private float nextFireTime = 0f;
    private bool isReloading = false;
    private Coroutine noAmmoCoroutine;
    private bool isFullAuto = false;

    private void Start()
    {
        GlobalVariables.playerPrimaryAmmo = weapon.magazineSize;
        GlobalVariables.playerPrimaryTotalAmmo = weapon.magazineSize * 4;

        originalCrosshairSize = crosshair.rectTransform.sizeDelta;
        UpdateAmmoUI();
        UpdateFireModeUI();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFullAuto = !isFullAuto;
            UpdateFireModeUI();
        }

        if (isFullAuto && Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            TryShoot();
        }
        else if (!isFullAuto && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GlobalVariables.playerPrimaryTotalAmmo > 0)
                StartCoroutine(Reload());
            else
                ShowNoAmmoWarning();
        }

        if (!isFiring)
        {
            currentSpread = Mathf.Max(currentSpread - spreadRecoveryRate * Time.deltaTime, baseSpread);
            crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, originalCrosshairSize, Time.deltaTime * crosshairRecoverySpeed);
        }

        isFiring = false;
        UpdateAmmoUI();
    }

    private void TryShoot()
    {
        if (GlobalVariables.playerPrimaryAmmo <= 0)
        {
            if (GlobalVariables.playerPrimaryTotalAmmo > 0)
                StartCoroutine(Reload());
            else
                ShowNoAmmoWarning();
            return;
        }

        nextFireTime = Time.time + weapon.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        isFiring = true;
        IncreaseRecoil();

        if (weapon.gunshotEffect != null && weapon.bulletOut != null)
        {
            GameObject gunshot = Instantiate(weapon.gunshotEffect, weapon.bulletOut.position, weapon.bulletOut.rotation);
            gunshot.transform.SetParent(weapon.bulletOut);
        }

        Camera cam = Camera.main;
        Vector3 recoilOffset = GetRecoilOffset();
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0) + recoilOffset);

        if (Physics.Raycast(ray, out RaycastHit hit, weapon.range))
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-hit.normal * weapon.impactForce, ForceMode.Impulse);
            }
        }

        GlobalVariables.playerPrimaryAmmo--;
        UpdateAmmoUI();
    }

    private void IncreaseRecoil()
    {
        currentSpread = Mathf.Min(currentSpread + spreadIncreaseRate, maxSpread);

        Vector2 newSize = originalCrosshairSize + new Vector2(crosshairExpandAmount, crosshairExpandAmount);
        crosshair.rectTransform.sizeDelta = Vector2.Lerp(crosshair.rectTransform.sizeDelta, newSize, Time.deltaTime * 10f);
    }

    private Vector3 GetRecoilOffset()
    {
        float recoilX = Random.Range(-currentSpread, currentSpread);
        float recoilY = Random.Range(-currentSpread, currentSpread);
        return new Vector3(recoilX * Screen.width, recoilY * Screen.height, 0);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        weapon.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);

        ammoText.SetActive(false);
        reloadingText.gameObject.SetActive(true);
        reloadingText.text = "Reloading...";

        yield return new WaitForSeconds(weapon.reloadTime);

        int ammoNeeded = weapon.magazineSize - GlobalVariables.playerPrimaryAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, GlobalVariables.playerPrimaryTotalAmmo);

        GlobalVariables.playerPrimaryAmmo += ammoToReload;
        GlobalVariables.playerPrimaryTotalAmmo -= ammoToReload;

        weapon.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
        isReloading = false;

        ammoText.SetActive(true);
        reloadingText.gameObject.SetActive(false);

        UpdateAmmoUI();
    }

    private void ShowNoAmmoWarning()
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

    private void UpdateAmmoUI()
    {
        ammoLeftText.text = GlobalVariables.playerPrimaryAmmo.ToString();
        totalAmmoText.text = GlobalVariables.playerPrimaryTotalAmmo.ToString();
    }

    private void UpdateFireModeUI()
    {
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
}