using UnityEngine;
using System.Collections;

public class WeaponFire : MonoBehaviour
{
    public Weapon weapon;
    public GameObject impactEffect;
    public WeaponUIManager uiManager;

    [Header("Recoil & Spread")]
    public float baseSpread = 0.01f;
    public float maxSpread = 0.1f;
    public float spreadIncreaseRate = 0.005f;
    public float spreadRecoveryRate = 0.02f;
    private float currentSpread = 0f;

    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isFullAuto = false;
    private bool isFiring = false;

    private void Start()
    {
        GlobalVariables.playerPrimaryAmmo = weapon.magazineSize;
        GlobalVariables.playerPrimaryTotalAmmo = weapon.magazineSize * 4;

        uiManager.UpdateAmmoUI(GlobalVariables.playerPrimaryAmmo, GlobalVariables.playerPrimaryTotalAmmo);

        if (weapon.hasFireType)
        {
            uiManager.UpdateFireModeUI(isFullAuto, weapon.hasFireType);
        }
        else
        {
            uiManager.fireModeText.SetActive(false);
        }
    }

    private void Update()
    {
        if (isReloading) return;

        if (weapon.hasFireType)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isFullAuto = !isFullAuto;
                uiManager.UpdateFireModeUI(isFullAuto, weapon.hasFireType);
            }
        }

        if (weapon.hasFireType)
        {
            if (isFullAuto && Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                TryShoot();
            }
            else if (!isFullAuto && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                TryShoot();
            }
        }
        else if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GlobalVariables.playerPrimaryTotalAmmo > 0)
                StartCoroutine(Reload());
            else
                uiManager.ShowNoAmmoWarning();
        }

        if (!isFiring)
        {
            currentSpread = Mathf.Max(currentSpread - spreadRecoveryRate * Time.deltaTime, baseSpread);
            uiManager.RecoverCrosshair();
        }

        isFiring = false;
        uiManager.UpdateAmmoUI(GlobalVariables.playerPrimaryAmmo, GlobalVariables.playerPrimaryTotalAmmo);
    }

    private void TryShoot()
    {
        if (GlobalVariables.playerPrimaryAmmo <= 0)
        {
            if (GlobalVariables.playerPrimaryTotalAmmo > 0)
                StartCoroutine(Reload());
            else
                uiManager.ShowNoAmmoWarning();
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
        uiManager.UpdateAmmoUI(GlobalVariables.playerPrimaryAmmo, GlobalVariables.playerPrimaryTotalAmmo);
    }

    private void IncreaseRecoil()
    {
        currentSpread = Mathf.Min(currentSpread + spreadIncreaseRate, maxSpread);
        uiManager.ExpandCrosshair();
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
        uiManager.ammoText.gameObject.SetActive(false);
        uiManager.ShowReloadingText("Reloading...");

        yield return new WaitForSeconds(weapon.reloadTime);

        int ammoNeeded = weapon.magazineSize - GlobalVariables.playerPrimaryAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, GlobalVariables.playerPrimaryTotalAmmo);

        GlobalVariables.playerPrimaryAmmo += ammoToReload;
        GlobalVariables.playerPrimaryTotalAmmo -= ammoToReload;

        weapon.gameObject.SetActive(true);
        isReloading = false;

        uiManager.HideReloadingText();
        uiManager.UpdateAmmoUI(GlobalVariables.playerPrimaryAmmo, GlobalVariables.playerPrimaryTotalAmmo);
        uiManager.ammoText.gameObject.SetActive(true);
    }
}