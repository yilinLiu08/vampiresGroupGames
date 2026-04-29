using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class FightSelection : MonoBehaviour
{
    public GameObject fightSelect;

    public GameObject fightSelectButton;



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fightSelectButton.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fightSelectButton.SetActive(false);
        }
    }

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
        fightSelectButton.SetActive(false);
    }


    

}
