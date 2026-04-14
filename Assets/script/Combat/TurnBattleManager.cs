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

    [Header("Round Enemies")]
    public BattleUnit[] round1Enemies;
    public BattleUnit[] round2Enemies;

    [Header("Round Turn Order")]
    public BattleUnit[] round1TurnOrder;
    public BattleUnit[] round2TurnOrder;

    [Header("Current Round")]
    public BattleUnit[] currentEnemies;
    public BattleUnit[] turnOrder;

    [Header("UI")]
    public TextMeshProUGUI messageText;
    public Button attackButton;
    public Button skillButton;

    [Header("Settings")]
    public float enemyActionDelay = 1f;

    private int currentTurnIndex = 0;
    private int currentRound = 1;

    private BattleUnit currentUnit;
    private ActionType selectedAction = ActionType.None;

    private bool waitingForPlayerTarget = false;
    private bool battleEnded = false;
    private bool changingRound = false;

    private void Start()
    {
        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(OnClickAttack);

        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(OnClickSkill);

        HideActionButtons();
        HideAllEnemyTargetButtons();
        ClearAllHighlights();

        SetRound(1);
        StartTurn();
    }

    private void SetRound(int round)
    {
        currentRound = round;
        currentTurnIndex = 0;

        SetEnemyGroupActive(round1Enemies, false);
        SetEnemyGroupActive(round2Enemies, false);

        if (round == 1)
        {
            currentEnemies = round1Enemies;
            turnOrder = round1TurnOrder;
            SetEnemyGroupActive(round1Enemies, true);
        }
        else
        {
            currentEnemies = round2Enemies;
            turnOrder = round2TurnOrder;
            SetEnemyGroupActive(round2Enemies, true);
        }

        HideAllEnemyTargetButtons();
    }

    private void SetEnemyGroupActive(BattleUnit[] group, bool value)
    {
        for (int i = 0; i < group.Length; i++)
        {
            group[i].gameObject.SetActive(value);
            group[i].HideTargetButton();
        }
    }

    private void StartTurn()
    {
        if (battleEnded)
        {
            return;
        }

        if (changingRound)
        {
            return;
        }

        CheckBattleResult();

        if (battleEnded)
        {
            return;
        }

        if (changingRound)
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

        HideAllEnemyTargetButtons();
        ClearAllHighlights();
        currentUnit.SetHighlight(true);

        if (currentUnit.isPlayer)
        {
            StartPlayerTurn();
            return;
        }

        StartCoroutine(EnemyTurnRoutine());
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

            if (!unit.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (unit.IsDead())
            {
                continue;
            }

            return unit;
        }

        return null;
    }

    private void StartPlayerTurn()
    {
        ShowActionButtons();
        HideAllEnemyTargetButtons();

        messageText.text = currentUnit.unitName + " turn. Skill: " + currentUnit.GetSkillDescription();
    }

    private IEnumerator EnemyTurnRoutine()
    {
        HideActionButtons();
        HideAllEnemyTargetButtons();

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

        if (!battleEnded && !changingRound)
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

        waitingForPlayerTarget = false;
        HideAllEnemyTargetButtons();
        HideActionButtons();

        if (currentUnit.skillType == BattleUnit.SkillType.TeamHeal)
        {
            StartCoroutine(PlayerTeamHealRoutine());
            return;
        }

        if (currentUnit.skillType == BattleUnit.SkillType.TeamShield)
        {
            StartCoroutine(PlayerTeamShieldRoutine());
            return;
        }

        if (currentUnit.skillType == BattleUnit.SkillType.AoE)
        {
            StartCoroutine(PlayerAoERoutine());
        }
    }

    public void OnSelectEnemyTarget(BattleUnit target)
    {
        if (!waitingForPlayerTarget)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        if (!target.gameObject.activeInHierarchy)
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
        HideAllEnemyTargetButtons();

        StartCoroutine(PlayerAttackRoutine(target, damage));
    }

    private IEnumerator PlayerAttackRoutine(BattleUnit target, int damage)
    {
        yield return new WaitForSeconds(0.4f);

        target.TakeDamage(damage);

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded && !changingRound)
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

        if (!battleEnded && !changingRound)
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

        if (!battleEnded && !changingRound)
        {
            StartTurn();
        }
    }

    private IEnumerator PlayerAoERoutine()
    {
        messageText.text = currentUnit.unitName + " uses skill: deal damage to all enemies.";

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (!currentEnemies[i].IsDead() && currentEnemies[i].gameObject.activeInHierarchy)
            {
                currentEnemies[i].TakeDamage(currentUnit.skillDamage);
            }
        }

        yield return new WaitForSeconds(0.8f);

        CheckBattleResult();

        if (!battleEnded && !changingRound)
        {
            StartTurn();
        }
    }

    private void ShowAvailableEnemyTargets()
    {
        HideAllEnemyTargetButtons();

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            BattleUnit enemy = currentEnemies[i];

            if (!enemy.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (enemy.IsDead())
            {
                continue;
            }

            BattleUnit targetEnemy = enemy;
            targetEnemy.BindTargetButton(() => OnSelectEnemyTarget(targetEnemy));
        }
    }

    private BattleUnit[] GetAlliesOf(BattleUnit unit)
    {
        if (unit.isPlayer)
        {
            return players;
        }

        return currentEnemies;
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

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (!currentEnemies[i].IsDead() && currentEnemies[i].gameObject.activeInHierarchy)
            {
                enemiesDead = false;
                break;
            }
        }

        if (playersDead)
        {
            battleEnded = true;
            HideActionButtons();
            HideAllEnemyTargetButtons();
            ClearAllHighlights();
            messageText.text = "All players are defeated.";
            return;
        }

        if (enemiesDead)
        {
            if (currentRound == 1)
            {
                if (!changingRound)
                {
                    StartCoroutine(StartNextRoundRoutine());
                }

                return;
            }

            battleEnded = true;
            HideActionButtons();
            HideAllEnemyTargetButtons();
            ClearAllHighlights();
            messageText.text = "All enemies are defeated.";
        }
    }

    private IEnumerator StartNextRoundRoutine()
    {
        changingRound = true;

        HideActionButtons();
        HideAllEnemyTargetButtons();
        ClearAllHighlights();

        messageText.text = "Round 1 cleared. Round 2 starts.";

        yield return new WaitForSeconds(1f);

        SetRound(2);

        messageText.text = "Round 2 starts.";

        yield return new WaitForSeconds(0.5f);

        changingRound = false;
        StartTurn();
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

    private void HideAllEnemyTargetButtons()
    {
        for (int i = 0; i < round1Enemies.Length; i++)
        {
            round1Enemies[i].HideTargetButton();
        }

        for (int i = 0; i < round2Enemies.Length; i++)
        {
            round2Enemies[i].HideTargetButton();
        }
    }

    private void ClearAllHighlights()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetHighlight(false);
        }

        for (int i = 0; i < round1Enemies.Length; i++)
        {
            round1Enemies[i].SetHighlight(false);
        }

        for (int i = 0; i < round2Enemies.Length; i++)
        {
            round2Enemies[i].SetHighlight(false);
        }
    }

    public void ForceStartRound2()
    {
        if (battleEnded)
        {
            return;
        }

        StopAllCoroutines();

        changingRound = false;
        waitingForPlayerTarget = false;
        selectedAction = ActionType.None;

        HideActionButtons();
        HideAllEnemyTargetButtons();
        ClearAllHighlights();

        SetRound(2);

        messageText.text = "Round 2 starts.";

        StartTurn();
    }
}