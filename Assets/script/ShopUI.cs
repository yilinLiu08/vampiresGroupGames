using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class ShopUI : MonoBehaviour
{
    public GameObject garlicButton;
    public GameObject garlicOff;
    public GameObject shopUI;

    [YarnCommand("open_shop")]


   
    public void OpenShop()
    {
        
        shopUI.SetActive(true);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            shopUI.SetActive(false);
        }
    }



    [YarnCommand("unlock_garlic")]

    public void Garlic()
    {
        garlicButton.SetActive(true);
        garlicOff.SetActive(false);
    }
}
