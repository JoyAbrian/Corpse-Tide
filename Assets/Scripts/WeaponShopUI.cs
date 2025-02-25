using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopUI : MonoBehaviour
{
    public WeaponFire weaponFire;
    public GameObject weaponPrimary;
    public WeaponUIManager WeaponUI;
    [SerializeField] private List<Weapon> weapons;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI playerMoneyText;
    [SerializeField] private WeaponDisplay weaponDisplay;
    [SerializeField] private Transform contentParent;

    private void Start()
    {
        foreach (Weapon weapon in weapons.OrderBy(w => w.weaponPrice))
        {
            WeaponDisplay weaponUI = Instantiate(weaponDisplay, contentParent);
            weaponUI.SetWeapon(weapon);
            Button button = weaponUI.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnWeaponSelected(weapon));
            }
        }
    }

    private void Update()
    {
        playerMoneyText.text = "Player Money: $" + GlobalVariables.playerMoney;
    }

    private void OnWeaponSelected(Weapon weapon)
    {
        if (GlobalVariables.playerMoney < weapon.weaponPrice) return;

        foreach (Transform child in weaponPrimary.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject newWeaponObj = Instantiate(weapon.gameObject, weaponPrimary.transform);
        newWeaponObj.SetActive(true);

        Weapon newWeapon = newWeaponObj.GetComponent<Weapon>();
        weaponFire.weapon = newWeapon;

        WeaponUI.SelectWeapon(newWeapon);

        GlobalVariables.playerMoney -= weapon.weaponPrice;
        GlobalVariables.playerPrimaryAmmo = newWeapon.magazineSize;
        GlobalVariables.playerPrimaryTotalAmmo = newWeapon.magazineSize * 4;
    }
}
