using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static int playerScore = 0;
    public static int playerHealth = 100;

    [Header("Weapon UI")]
    public static Weapon playerWeapon;
    public static int playerPrimaryAmmo;
    public static int playerPrimaryTotalAmmo;
}
