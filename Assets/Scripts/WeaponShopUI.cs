using TMPro;
using UnityEngine;

public class WeaponShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerMoneyText;
    private void Update()
    {
        playerMoneyText.text = "Player Money: $" + GlobalVariables.playerMoney;
    }
}
