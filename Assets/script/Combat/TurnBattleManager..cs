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

        int enemyButtonCount = Mathf.Min(enemyTargetButtons.Length, enemies.Length);

        for (int i = 0; i < enemyButtonCount; i++)
        {
            int index = i;
            enemyTargetButtons[i].onClick.RemoveAllListeners();
            enemyTargetButtons[i].onClick.AddListener(() => OnSelectEnemyTarget(enemies[index]));
        }

        HideActionButtons();
        HideEnemyTargetButtons();
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

        if (currentUnit == null)
        {
            return;
        }

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
        HideEnemyTargetButtons();

        messageText.text = currentUnit.unitName + " turn. Skill: " + currentUnit.GetSkillDescription();
    }



    private IEnumerator EnemyTurnRoutine()
    {
        HideActionButtons();
        HideEnemyTargetButtons();

        messageText.text = currentUnit.unitName + " is thinking...";

        yield return new WaitForSeconds(enemyActionDelay);

        BattleUnit target = GetRandomAlivePlayer();

        if (target == null)
        {
            yield break;
        }

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
        if (!currentUnit.TryUseMana())
        {
            messageText.text = currentUnit.unitName + " does not have enough mana.";
            return;
        }

        selectedAction = ActionType.Skill;

        if (currentUnit.skillType == BattleUnit.SkillType.Damage)
        {
            waitingForPlayerTarget = true;
            ShowAvailableEnemyTargets();

            messageText.text = currentUnit.unitName + " skill: " + currentUnit.GetSkillDescription();
            return;
        }

        if (currentUnit.skillType == BattleUnit.SkillType.TeamHeal)
        {
            waitingForPlayerTarget = false;
            HideEnemyTargetButtons();
            HideActionButtons();

            StartCoroutine(PlayerTeamHealRoutine());
            return;
        }

        if (currentUnit.skillType == BattleUnit.SkillType.TeamShield)
        {
            waitingForPlayerTarget = false;
            HideEnemyTargetButtons();
            HideActionButtons();

            StartCoroutine(PlayerTeamShieldRoutine());
            return;
        }

        if (currentUnit.skillType == BattleUnit.SkillType.AoE)
        {
            waitingForPlayerTarget = false;
            HideEnemyTargetButtons();
            HideActionButtons();

            StartCoroutine(PlayerAoERoutine());
        }
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
        HideEnemyTargetButtons();

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



    private IEnumerator PlayerTeamHealRoutine()
    {
        messageText.text = currentUnit.unitName + " uses skill: heal all allies.";

        yield return new WaitForSeconds(0.4f);

        BattleUnit[] allyTeam = GetAlliesOf(currentUnit);

        for (int i = 0; i < allyTeam.Length; i++)
        {
            if (!allyTeam[i].IsDead())
            {
                allyTeam[i].Heal(currentUnit.healAmount);
            }
        }

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded)
        {
            StartTurn();
        }
    }



    private IEnumerator PlayerTeamShieldRoutine()
    {
        messageText.text = currentUnit.unitName + " uses skill: all allies ignore the next hit.";

        BattleUnit[] allyTeam = GetAlliesOf(currentUnit);

        for (int i = 0; i < allyTeam.Length; i++)
        {
            if (!allyTeam[i].IsDead())
            {
                allyTeam[i].GiveShield();
            }
        }

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded)
        {
            StartTurn();
        }
    }



    private IEnumerator PlayerAoERoutine()
    {
        messageText.text = currentUnit.unitName + " uses skill: deal damage to all enemies.";

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].IsDead())
            {
                enemies[i].TakeDamage(currentUnit.skillDamage);
            }
        }

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded)
        {
            StartTurn();
        }
    }



    private void ShowAvailableEnemyTargets()
    {
        HideEnemyTargetButtons();

        int count = Mathf.Min(enemies.Length, enemyTargetButtons.Length);

        for (int i = 0; i < count; i++)
        {
            enemyTargetButtons[i].gameObject.SetActive(!enemies[i].IsDead());
        }
    }



    private BattleUnit[] GetAlliesOf(BattleUnit unit)
    {
        if (unit.isPlayer)
        {
            return players;
        }

        return enemies;
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

        if (alivePlayers.Count == 0)
        {
            return null;
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
            HideEnemyTargetButtons();
            ClearAllHighlights();
            messageText.text = "All players are defeated.";
        }

        if (enemiesDead)
        {
            battleEnded = true;
            HideActionButtons();
            HideEnemyTargetButtons();
            ClearAllHighlights();
            messageText.text = "All enemies are defeated.";
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



    private void HideEnemyTargetButtons()
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