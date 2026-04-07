using UnityEngine;
using UnityEngine.InputSystem;

public class OpenInventory : MonoBehaviour
{
    public GameObject inventory;


    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            inventory.SetActive(false);
        }
    }
    public void Toggle()
    {

        inventory.SetActive(true);
    }

    
}