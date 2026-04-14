using UnityEngine;

public class AddItem : MonoBehaviour
{
    public FruitInventory inventory;
    public FruitGrid fruitGrid;

    public Fruit fruitToAdd;

    public void AddFruit()
    {
        if (inventory == null)
        {
            Debug.Log("Inventory is missing.");
            return;
        }

        if (fruitToAdd == null)
        {
            Debug.Log("Fruit to add is missing.");
            return;
        }

        inventory.AddFruit(fruitToAdd);

        if (fruitGrid != null)
        {
            fruitGrid.RefreshGrid();
        }

        Debug.Log("Added fruit: " + fruitToAdd.itemName);
    }
}