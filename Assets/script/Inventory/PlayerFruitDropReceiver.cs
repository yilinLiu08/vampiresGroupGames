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

        targetUnit.UseFruit(fruitData.currentFruit);

        fruitData.wasDroppedOnValidTarget = true;
    }
}