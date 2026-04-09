using UnityEngine;
using UnityEngine.InputSystem;

public class FightSelection : MonoBehaviour
{
    public GameObject fightSelect;


    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            fightSelect.SetActive(false);
        }
    }
    public void Toggle()
    {

        fightSelect.SetActive(true);
    }


}
