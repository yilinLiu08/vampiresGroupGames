using UnityEngine;
using UnityEngine.SceneManagement; // Make sure to add this namespace
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    
    public void OnButtonClick()
    {
        SceneManager.LoadScene("2_Platformer");
    }

}