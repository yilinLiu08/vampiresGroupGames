using UnityEngine;

public class Linker : MonoBehaviour
{
    void Start()
    {
        
        FruitInventory survivingInventory = FindObjectOfType<FruitInventory>();

       
        FruitGrid myGrid = GetComponent<FruitGrid>();

        if (survivingInventory != null && myGrid != null)
        {
           
            myGrid.inventory = survivingInventory;

            
            myGrid.RefreshGrid();
            Debug.Log("connect to global inventory");
        }
        else
        {
            Debug.LogWarning("where da inventory");
        }
    }
}