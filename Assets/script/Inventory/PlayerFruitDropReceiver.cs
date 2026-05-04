using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerFruitDropReceiver : MonoBehaviour, IDropHandler
{
    public BattleUnit targetUnit;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Player OnDrop triggered");

        if (eventData == null)
        {
            return;
        }

        if (eventData.pointerDrag == null)
        {
            return;
        }

        if (TurnBattleManager.Instance == null)
        {
            return;
        }

        if (!TurnBattleManager.Instance.CanUseInventoryItem())
        {
            TurnBattleManager.Instance.ShowFruitMessage("Choose Inventory first.");
            return;
        }

        FruitData fruitData = eventData.pointerDrag.GetComponent<FruitData>();

        if (fruitData == null)
        {
            return;
        }

        if (fruitData.currentFruit == null)
        {
            return;
        }

        if (targetUnit == null)
        {
            return;
        }

        bool used = targetUnit.UseFruit(fruitData.currentFruit);

        if (!used)
        {
            TurnBattleManager.Instance.ShowFruitMessage("This item cannot be used here.");
            Debug.Log("Fruit use failed on target: " + targetUnit.unitName);
            return;
        }

        fruitData.CompleteSuccessfulDrop();
        TurnBattleManager.Instance.NotifyInventoryItemUsed();
    }
}