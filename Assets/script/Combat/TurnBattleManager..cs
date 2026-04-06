using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnBattleManager : MonoBehaviour
{
    public enum ActionType
    {
        None,
        Attack,
        Skill
    }

    [Header("Players")]
    public BattleUnit[] players = new BattleUnit[4];

    [Header("Enemies")]
    public BattleUnit[] enemies;

    [Header("Turn Order")]
    public BattleUnit[] turnOrder;

    [Header("UI")]
    public TextMeshProUGUI messageText;
    public Button attackButton;
    public Button skillButton;

    [Header("Enemy Target Buttons")]
    public Button[] enemyTargetButtons;

    [Header("Settings")]
    public float enemyActionDelay = 1f;

    private int currentTurnIndex = 0;
    private BattleUnit currentUnit;
    private ActionType selectedAction = ActionType.None;
    private bool waitingForPlayerTarget = false;
    private bool battleEnded = false;



    private void Start()
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(OnClickAttack);

        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(OnClickSkill);

        for (int i = 0; i < enemyTargetButtons.Length; i++)
        {
            int index = i;
            enemyTargetButtons[i].onClick.RemoveAllListeners();
            enemyTargetButtons[i].onClick.AddListener(() => OnSelectEnemyTarget(enemies[index]));
        }

        HideActionButtons();
        HideTargetButtons();
        ClearAllHighlights();

        StartTurn();
    }



    private void StartTurn()
    {
        if (battleEnded)
        {
            return;
        }

        CheckBattleResult();

        if (battleEnded)
        {
            return;
        }

        currentUnit = GetNextAliveUnit();

        selectedAction = ActionType.None;
        waitingForPlayerTarget = false;

        ClearAllHighlights();
        currentUnit.SetHighlight(true);

        if (currentUnit.isPlayer)
        {
            StartPlayerTurn();
        }
        else
        {
            StartCoroutine(EnemyTurnRoutine());
        }
    }



    private BattleUnit GetNextAliveUnit()
    {
        int checkedCount = 0;

        while (checkedCount < turnOrder.Length)
        {
            if (currentTurnIndex >= turnOrder.Length)
            {
                currentTurnIndex = 0;
            }

            BattleUnit unit = turnOrder[currentTurnIndex];
            currentTurnIndex++;
            checkedCount++;

            if (!unit.IsDead())
            {
                return unit;
            }
        }

        return null;
    }



    private void StartPlayerTurn()
    {
        ShowActionButtons();
        HideTargetButtons();

        messageText.text = currentUnit.unitName + " turn. Choose Attack or Skill.";
    }



    private IEnumerator EnemyTurnRoutine()
    {
        HideActionButtons();
        HideTargetButtons();

        messageText.text = currentUnit.unitName + " is thinking...";

        yield return new WaitForSeconds(enemyActionDelay);

        BattleUnit target = GetRandomAlivePlayer();

        ClearAllHighlights();
        currentUnit.SetHighlight(true);
        target.SetHighlight(true);

        messageText.text = currentUnit.unitName + " attacks " + target.unitName + ".";

        yield return new WaitForSeconds(0.4f);

        target.TakeDamage(currentUnit.attackDamage);

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded)
        {
            StartTurn();
        }
    }



    public void OnClickAttack()
    {
        selectedAction = ActionType.Attack;
        waitingForPlayerTarget = true;

        ShowAvailableEnemyTargets();

        messageText.text = "Choose an enemy to attack.";
    }



    public void OnClickSkill()
    {
        selectedAction = ActionType.Skill;
        waitingForPlayerTarget = true;

        ShowAvailableEnemyTargets();

        messageText.text = "Choose an enemy to use skill on.";
    }



    public void OnSelectEnemyTarget(BattleUnit target)
    {
        if (!waitingForPlayerTarget)
        {
            return;
        }

        if (target.IsDead())
        {
            return;
        }

        ClearAllHighlights();
        currentUnit.SetHighlight(true);
        target.SetHighlight(true);

        int damage = 0;

        if (selectedAction == ActionType.Attack)
        {
            damage = currentUnit.attackDamage;
            messageText.text = currentUnit.unitName + " attacks " + target.unitName + ".";
        }

        if (selectedAction == ActionType.Skill)
        {
            damage = currentUnit.skillDamage;
            messageText.text = currentUnit.unitName + " uses skill on " + target.unitName + ".";
        }

        waitingForPlayerTarget = false;
        selectedAction = ActionType.None;

        HideActionButtons();
        HideTargetButtons();

        StartCoroutine(PlayerAttackRoutine(target, damage));
    }



    private IEnumerator PlayerAttackRoutine(BattleUnit target, int damage)
    {
        yield return new WaitForSeconds(0.4f);

        target.TakeDamage(damage);

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded)
        {
            StartTurn();
        }
    }



    private void ShowAvailableEnemyTargets()
    {
        HideTargetButtons();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemyTargetButtons[i].gameObject.SetActive(!enemies[i].IsDead());
        }
    }



    private BattleUnit GetRandomAlivePlayer()
    {
        List<BattleUnit> alivePlayers = new List<BattleUnit>();

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsDead())
            {
                alivePlayers.Add(players[i]);
            }
        }

        int randomIndex = Random.Range(0, alivePlayers.Count);
        return alivePlayers[randomIndex];
    }



    private void CheckBattleResult()
    {
        bool playersDead = true;
        bool enemiesDead = true;

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsDead())
            {
                playersDead = false;
                break;
            }
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead())
            {
                enemiesDead = false;
                break;
            }
        }

        if (playersDead)
        {
            battleEnded = true;
            HideActionButtons();
            HideTargetButtons();
            ClearAllHighlights();
            messageText.text = "Defeat";
        }

        if (enemiesDead)
        {
            battleEnded = true;
            HideActionButtons();
            HideTargetButtons();
            ClearAllHighlights();
            messageText.text = "Victory";
        }
    }



    private void ShowActionButtons()
    {
        attackButton.gameObject.SetActive(true);
        skillButton.gameObject.SetActive(true);
    }



    private void HideActionButtons()
    {
        attackButton.gameObject.SetActive(false);
        skillButton.gameObject.SetActive(false);
    }



    private void HideTargetButtons()
    {
        for (int i = 0; i < enemyTargetButtons.Length; i++)
        {
            enemyTargetButtons[i].gameObject.SetActive(false);
        }
    }



    private void ClearAllHighlights()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetHighlight(false);
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetHighlight(false);
        }
    }
}