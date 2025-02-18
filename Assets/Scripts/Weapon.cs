using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon UI")]
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Weapon Stats")]
    public bool hasFireType;
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
