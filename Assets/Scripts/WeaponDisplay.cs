using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public Image weaponIcon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponPrice;

    private Weapon weapon;

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weaponIcon.sprite = weapon.weaponIcon;
        weaponName.text = weapon.weaponName;
        weaponPrice.text = $"${weapon.weaponPrice}";

        UpdatePriceColor();
    }

    private void Update()
    {
        UpdatePriceColor();
    }

    private void UpdatePriceColor()
    {
        if (GlobalVariables.playerMoney < weapon.weaponPrice)
        {
            weaponPrice.color = Color.red;
        }
        else
        {
            weaponPrice.color = Color.green;
        }
    }
}