using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    [SerializeField] private GameObject weaponShopUI;
    [SerializeField] private GameObject player;

    private Movement movement;
    private WeaponFire weaponFire;

    private void Start()
    {
        CloseShop();
        if (player != null)
        {
            movement = player.GetComponent<Movement>();
            weaponFire = player.GetComponent<WeaponFire>();
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleShop();
        }
    }

    private void ToggleShop()
    {
        bool isActive = !weaponShopUI.activeSelf;
        weaponShopUI.SetActive(isActive);

        if (isActive)
        {
            OpenShop();
        }
        else
        {
            CloseShop();
        }
    }

    private void OpenShop()
    {
        if (movement != null)
        {
            movement.enabled = false;
        }

        if (weaponFire != null)
        {
            weaponFire.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseShop()
    {
        if (movement != null)
        {
            movement.enabled = true;
        }

        if (weaponFire != null)
        {
            weaponFire.enabled = true;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}