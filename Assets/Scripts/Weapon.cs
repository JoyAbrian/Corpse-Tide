using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon UI")]
    public string weaponName;
    public Sprite weaponIcon;
    public int weaponPrice;

    [Header("Weapon Stats")]
    public bool hasFireType;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.5f;
    public float impactForce = 30f;
    public float reloadTime = 2f;
    public int magazineSize = 30;

    [Header("Recoil & Spread")]
    public float baseSpread = 0.02f;
    public float maxSpread = 0.08f;
    public float spreadIncreaseRate = 0.004f;
    public float spreadRecoveryRate = 0.025f;
    public float currentSpread;

    [Header("Weapon Components")]
    public Transform bulletOut;
    public GameObject gunshotEffect;
}
