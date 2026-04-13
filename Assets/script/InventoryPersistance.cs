using UnityEngine;

public class InventoryPersistence : MonoBehaviour
{
    public static InventoryPersistence Instance;

    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            
            Destroy(gameObject);
            return;
        }

        
        Instance = this;

       
        DontDestroyOnLoad(gameObject);
    }
}