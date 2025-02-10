using TMPro;
using UnityEngine;

public class WeaponAmmoManager : MonoBehaviour
{
    //public Image weaponImage;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI totalAmmoText;

    private void Update()
    {
        ammoText.text = GlobalVariables.playerPrimaryAmmo.ToString();
        totalAmmoText.text = GlobalVariables.playerPrimaryTotalAmmo.ToString();
    }
}
