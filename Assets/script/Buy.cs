using UnityEngine;

public class Buy : MonoBehaviour
{
    [Header("Inventory Settings")]
    public Fruit itemToGive;

    [Header("UI Display Settings")]
    public GameObject selectionImage; 

    
    private static GameObject currentlyActiveImage;

    public void AddItem()
    {

        FruitInventory inv = FindObjectOfType<FruitInventory>();

        if (inv != null && itemToGive != null)
        {
            inv.AddFruit(itemToGive);
        }
        else
        {
            Debug.LogError("where da inventory");
        }
    }

        private void HandleImageDisplay()
        {
        if (selectionImage == null) return;

        
        if (currentlyActiveImage != null && currentlyActiveImage != selectionImage)
        {
            currentlyActiveImage.SetActive(false);
        }

        
        selectionImage.SetActive(true);

        
        currentlyActiveImage = selectionImage;
         }

    
    public void Deselect()
    {
        if (selectionImage != null)
        {
            selectionImage.SetActive(false);
            if (currentlyActiveImage == selectionImage) currentlyActiveImage = null;
        }
    }
}
