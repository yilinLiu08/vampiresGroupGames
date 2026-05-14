using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class FightSelection : MonoBehaviour
{
    public CanvasGroup fightSelect;

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
            SetPopupState(false);
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetPopupState(false);
        }
    }

    public void Toggle()
    {
        fightSelectButton.SetActive(false);
        Debug.Log("turned canvas off");
        SetPopupState(true);
        //fightSelectButton.SetActive(false);
        
    }

    
   

    
    private void SetPopupState(bool isVisible)
    {
        fightSelect.alpha = isVisible ? 1 : 0;
        fightSelect.interactable = isVisible;
        fightSelect.blocksRaycasts = isVisible;
    }
}


    


