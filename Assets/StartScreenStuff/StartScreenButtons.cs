using UnityEngine;
using UnityEngine.SceneManagement; 
public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene(string poop)
    {
        SceneManager.LoadScene(poop);
    }
}