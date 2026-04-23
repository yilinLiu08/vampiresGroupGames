using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class RedColorEvent : MonoBehaviour
{
    public UnityEvent CharacterRedColor;
    public UnityEvent BlueBar;
    public UnityEvent Enemy;
    public UnityEvent Arrow;
    public UnityEvent Inventory;

    [YarnCommand("Character")]
    public void CharacterEvent()
    {
        CharacterRedColor.Invoke();
    }

    [YarnCommand("BlueBar")]
    public void BlueBarEvent()
    {
        BlueBar.Invoke();
    }
    [YarnCommand("Enemy")]
    public void EnemyEvent()
    {
        Enemy.Invoke();
    }
    [YarnCommand("Arrow")]
    public void ArrowEvent()
    {
        Arrow.Invoke();
    }

    [YarnCommand("Inventory")]
    public void InventoryEvent()
    {
        Inventory.Invoke();
    }
}