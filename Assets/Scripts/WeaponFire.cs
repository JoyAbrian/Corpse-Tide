using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public Weapon weapon;
    private float nextFireTime = 0f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + weapon.fireRate;
            Shoot();
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
            if (weapon.impactEffect != null)
            {
                GameObject impact = Instantiate(weapon.impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-hit.normal * weapon.impactForce, ForceMode.Impulse);
            }
        }
    }
}