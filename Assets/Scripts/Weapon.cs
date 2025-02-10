using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.5f;
    public float impactForce = 30f;
    public float reloadTime = 2f;
    public int magazineSize = 30;

    [Header("Weapon Components")]
    public Transform bulletOut;
    public GameObject gunshotEffect;
}
