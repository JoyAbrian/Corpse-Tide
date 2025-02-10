using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponFire : MonoBehaviour
{
    public Weapon weapon;
    public GameObject impactEffect;
    public Image crosshair;

    private float nextFireTime = 0f;
    private bool isReloading = false;

    private void Start()
    {
        GlobalVariables.playerPrimaryAmmo = weapon.magazineSize;
        GlobalVariables.playerPrimaryTotalAmmo = weapon.magazineSize * 4;
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + weapon.fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (weapon.gunshotEffect != null && weapon.bulletOut != null)
        {
            GameObject gunshot = Instantiate(weapon.gunshotEffect, weapon.bulletOut.position, weapon.bulletOut.rotation);
            gunshot.transform.SetParent(weapon.bulletOut);
        }

        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, weapon.range))
        {
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-hit.normal * weapon.impactForce, ForceMode.Impulse);
            }
        }
        GlobalVariables.playerPrimaryAmmo--;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        weapon.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(false);

        yield return new WaitForSeconds(weapon.reloadTime);
        GlobalVariables.playerPrimaryTotalAmmo -= weapon.magazineSize - GlobalVariables.playerPrimaryAmmo;
        GlobalVariables.playerPrimaryAmmo = weapon.magazineSize;

        weapon.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(true);
        isReloading = false;        
    }
}