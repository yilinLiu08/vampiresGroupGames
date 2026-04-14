using UnityEngine;

public class StartRound2Button : MonoBehaviour
{
    public TurnBattleManager battleManager;

    public void StartRound2()
    {
        battleManager.ForceStartRound2();
    }
}