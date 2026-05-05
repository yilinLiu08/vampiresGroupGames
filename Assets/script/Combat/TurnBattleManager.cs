using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnBattleManager : MonoBehaviour
{
    public static TurnBattleManager Instance { get; private set; }

    public enum ActionType
    {
        None,
        Attack,
        Skill,
        Item
    }

    public enum BattleLevel
    {
        Level1,
        Level2,
        Level3
    }

    [Header("Battle Level")]
    public BattleLevel battleLevel = BattleLevel.Level1;

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
    public Button inventoryButton;
    public TextMeshProUGUI attackButtonText;
    public TextMeshProUGUI inventoryButtonText;

    [Header("Inventory UI")]
    public GameObject inventoryPanel;

    [Header("Button Labels")]
    public string attackLabel = "Attack";
    public string cancelAttackLabel = "Cancel";
    public string inventoryLabel = "Inventory";
    public string cancelInventoryLabel = "Cancel";

    [Header("Settings")]
    public float enemyActionDelay = 1f;
    public float itemUseDelay = 0.8f;

    [Header("Scene Change")]
    public SceneChange loseSceneChange;
    public SceneChange winSceneChange;
    public string resultSceneName = "Story";
    public float resultSceneDelay = 1.2f;

    private int currentTurnIndex = 0;
    private int currentRound = 1;
    private int turnCycle = 0;

    private BattleUnit currentUnit;
    private ActionType selectedAction = ActionType.None;

    private bool waitingForPlayerTarget = false;
    private bool waitingForInventoryItem = false;
    private bool itemUseLocked = false;

    private bool battleEnded = false;
    private bool changingRound = false;
    private bool loadingResultScene = false;
    private bool currentTurnGaveTeamShield = false;

    private float roundSkillBoostMultiplier = 1f;
    private int roundSkillBoostExpireCycle = -1;
    private Fruit roundSkillBoostSourceFruit;

    private bool[] consumedTurnSlots;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (attackButtonText == null && attackButton != null)
        {
            attackButtonText = attackButton.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        if (inventoryButtonText == null && inventoryButton != null)
        {
            inventoryButtonText = inventoryButton.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        attackButton.onClick.RemoveAllListeners();
        attackButton.onClick.AddListener(OnClickAttack);

        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(OnClickSkill);

        if (inventoryButton != null)
        {
            inventoryButton.onClick.RemoveAllListeners();
            inventoryButton.onClick.AddListener(OnClickInventory);
        }

        HideActionButtons();
        HideInventoryPanel();
        HideAllEnemyTargetButtons();
        ClearAllHighlights();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        SetRound(1);
        StartTurn();
    }

    public void ShowFruitMessage(string text)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = text;
    }

    void SetAttackButtonNormal()
    {
        if (attackButtonText == null)
        {
            return;
        }

        attackButtonText.text = attackLabel;
    }

    void SetAttackButtonCancel()
    {
        if (attackButtonText == null)
        {
            return;
        }

        attackButtonText.text = cancelAttackLabel;
    }

    void SetInventoryButtonNormal()
    {
        if (inventoryButtonText == null)
        {
            return;
        }

        inventoryButtonText.text = inventoryLabel;
    }

    void SetInventoryButtonCancel()
    {
        if (inventoryButtonText == null)
        {
            return;
        }

        inventoryButtonText.text = cancelInventoryLabel;
    }

    void ShowInventoryPanel()
    {
        if (inventoryPanel == null)
        {
            return;
        }

        inventoryPanel.SetActive(true);
    }

    void HideInventoryPanel()
    {
        if (inventoryPanel == null)
        {
            return;
        }

        inventoryPanel.SetActive(false);
    }

    bool CurrentUnitHasEnoughMana()
    {
        if (currentUnit == null)
        {
            return false;
        }

        return currentUnit.currentMana >= currentUnit.skillManaCost;
    }

    int GetBattleLevelNumber()
    {
        if (battleLevel == BattleLevel.Level1)
        {
            return 1;
        }

        if (battleLevel == BattleLevel.Level2)
        {
            return 2;
        }

        return 3;
    }

    void RefreshRoundSkillBoostStatusUI()
    {
        bool active = IsRoundSkillBoostActive();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null)
            {
                continue;
            }

            if (active && roundSkillBoostSourceFruit != null)
            {
                players[i].SetExternalPersistentEffect(
                    roundSkillBoostSourceFruit.itemName,
                    "Attack skills x" + roundSkillBoostMultiplier.ToString("0.0") + " for the rest of this round.",
                    roundSkillBoostSourceFruit.icon
                );
            }
            else
            {
                players[i].ClearExternalPersistentEffect();
            }
        }
    }

    public void ResolveCoinFlipFruit(Fruit fruit)
    {
        if (fruit == null)
        {
            return;
        }

        bool heads = Random.value < 0.5f;

        if (heads)
        {
            roundSkillBoostMultiplier = Mathf.Max(1f, fruit.coinFlipSkillMultiplier);
            roundSkillBoostExpireCycle = turnCycle + 1;
            roundSkillBoostSourceFruit = fruit;

            RefreshRoundSkillBoostStatusUI();

            ShowFruitMessage("Heads! All player attack skills are x" + roundSkillBoostMultiplier.ToString("0.0") + " for the rest of this round.");
            return;
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsDead())
            {
                continue;
            }

            players[i].TakeDirectDamage(fruit.coinFlipTeamDamage);
        }

        ShowFruitMessage("Tails! All allies lose " + fruit.coinFlipTeamDamage + " HP.");
        CheckBattleResult();
    }

    private bool IsRoundSkillBoostActive()
    {
        return turnCycle < roundSkillBoostExpireCycle;
    }

    private int GetModifiedSkillDamage(BattleUnit unit)
    {
        int damage = unit.skillDamage;

        if (!unit.isPlayer)
        {
            return damage;
        }

        if (!IsRoundSkillBoostActive())
        {
            return damage;
        }

        return Mathf.RoundToInt(damage * roundSkillBoostMultiplier);
    }

    private void ResetConsumedTurnSlots()
    {
        if (turnOrder == null)
        {
            return;
        }

        consumedTurnSlots = new bool[turnOrder.Length];
    }

    private void MarkTurnSlotConsumed(int index)
    {
        if (consumedTurnSlots == null)
        {
            return;
        }

        if (index < 0 || index >= consumedTurnSlots.Length)
        {
            return;
        }

        consumedTurnSlots[index] = true;
    }

    private BattleUnit GetNextAlivePlayerReplacement(int startIndex)
    {
        if (turnOrder == null)
        {
            return null;
        }

        for (int i = startIndex; i < turnOrder.Length; i++)
        {
            if (consumedTurnSlots != null && consumedTurnSlots[i])
            {
                continue;
            }

            BattleUnit unit = turnOrder[i];

            if (!unit.isPlayer)
            {
                continue;
            }

            if (!unit.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (unit.IsDead())
            {
                continue;
            }

            MarkTurnSlotConsumed(i);
            return unit;
        }

        return null;
    }

    private void SetRound(int round)
    {
        currentRound = round;
        currentTurnIndex = 0;
        turnCycle = 0;
        roundSkillBoostMultiplier = 1f;
        roundSkillBoostExpireCycle = -1;
        roundSkillBoostSourceFruit = null;
        currentTurnGaveTeamShield = false;
        itemUseLocked = false;

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

        ResetConsumedTurnSlots();
        RefreshRoundSkillBoostStatusUI();
        HideAllEnemyTargetButtons();
        HideInventoryPanel();
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
        RefreshRoundSkillBoostStatusUI();

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
        waitingForInventoryItem = false;
        itemUseLocked = false;
        currentTurnGaveTeamShield = false;

        HideAllEnemyTargetButtons();
        HideActionButtons();
        HideInventoryPanel();
        ClearAllHighlights();
        currentUnit.SetHighlight(true);
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        StartCoroutine(BeginTurnRoutine());
    }

    private IEnumerator BeginTurnRoutine()
    {
        yield return StartCoroutine(HandlePoisonRoutine());

        if (battleEnded)
        {
            yield break;
        }

        if (changingRound)
        {
            yield break;
        }

        if (currentUnit == null)
        {
            yield break;
        }

        if (currentUnit.IsDead())
        {
            StartTurn();
            yield break;
        }

        if (currentUnit.ConsumeClotTurn())
        {
            messageText.text = currentUnit.unitName + " is frozen and cannot move.";

            yield return new WaitForSeconds(0.8f);

            CheckBattleResult();

            if (!battleEnded && !changingRound)
            {
                StartTurn();
            }

            yield break;
        }

        if (currentUnit.isPlayer)
        {
            StartPlayerTurn();
            yield break;
        }

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator HandlePoisonRoutine()
    {
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

            if (!enemy.HasPoison())
            {
                continue;
            }

            messageText.text = enemy.unitName + " takes " + enemy.GetPoisonDamage() + " poison damage.";

            yield return new WaitForSeconds(0.3f);

            enemy.TickPoison();

            yield return new WaitForSeconds(0.4f);

            CheckBattleResult();

            if (battleEnded || changingRound)
            {
                yield break;
            }
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
                turnCycle++;
                ResetConsumedTurnSlots();
                RefreshRoundSkillBoostStatusUI();
            }

            int slotIndex = currentTurnIndex;
            BattleUnit unit = turnOrder[slotIndex];

            currentTurnIndex++;
            checkedCount++;

            if (consumedTurnSlots != null && consumedTurnSlots[slotIndex])
            {
                continue;
            }

            MarkTurnSlotConsumed(slotIndex);

            if (!unit.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (!unit.IsDead())
            {
                return unit;
            }

            if (unit.isPlayer)
            {
                BattleUnit replacementPlayer = GetNextAlivePlayerReplacement(slotIndex + 1);

                if (replacementPlayer != null)
                {
                    return replacementPlayer;
                }
            }
        }

        return null;
    }

    private void StartPlayerTurn()
    {
        ShowActionButtons();
        HideAllEnemyTargetButtons();
        HideInventoryPanel();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        messageText.text = currentUnit.unitName + " turn. Skill: " + currentUnit.GetSkillDescription();
    }

    void ReturnToPlayerActionState(string customMessage)
    {
        waitingForPlayerTarget = false;
        waitingForInventoryItem = false;
        selectedAction = ActionType.None;

        HideAllEnemyTargetButtons();
        HideInventoryPanel();
        ClearAllHighlights();

        if (currentUnit != null)
        {
            currentUnit.SetHighlight(true);
        }

        ShowActionButtons();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        if (messageText == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(customMessage))
        {
            messageText.text = customMessage;
            return;
        }

        if (currentUnit != null)
        {
            messageText.text = currentUnit.unitName + " turn. Skill: " + currentUnit.GetSkillDescription();
        }
    }

    void CancelAttackSelection()
    {
        ReturnToPlayerActionState("");
    }

    void CancelSkillSelection()
    {
        ReturnToPlayerActionState("");
    }

    void CancelInventorySelection()
    {
        ReturnToPlayerActionState("");
    }

    private IEnumerator EnemyTurnRoutine()
    {
        HideActionButtons();
        HideInventoryPanel();
        HideAllEnemyTargetButtons();

        messageText.text = currentUnit.unitName + " is thinking...";

        yield return new WaitForSeconds(enemyActionDelay);

        if (currentUnit.TryFailFromNausea())
        {
            messageText.text = currentUnit.unitName + " feels nauseous and misses the turn.";

            yield return new WaitForSeconds(0.8f);

            CheckBattleResult();

            if (!battleEnded && !changingRound)
            {
                StartTurn();
            }

            yield break;
        }

        if (currentUnit.enemyActionType == BattleUnit.EnemyActionType.Healer)
        {
            yield return StartCoroutine(EnemyHealRoutine());
            yield break;
        }

        if (currentUnit.enemyActionType == BattleUnit.EnemyActionType.MagicAoE)
        {
            yield return StartCoroutine(EnemyMagicAoERoutine());
            yield break;
        }

        yield return StartCoroutine(EnemyNormalAttackRoutine());
    }

    private IEnumerator EnemyNormalAttackRoutine()
    {
        int targetIndex = GetRandomAlivePlayerIndex();

        if (targetIndex < 0)
        {
            yield break;
        }

        BattleUnit target = players[targetIndex];

        ClearAllHighlights();
        currentUnit.SetHighlight(true);
        target.SetHighlight(true);

        messageText.text = currentUnit.unitName + " attacks " + target.unitName + ".";
        currentUnit.PlayAttackAnimation();
        currentUnit.PlayAttackSFX();

        yield return new WaitForSeconds(0.4f);

        currentUnit.SpawnAttackEffectAtIndex(targetIndex);
        target.TakeDamage(currentUnit.GetAttackDamage());

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    private IEnumerator EnemyHealRoutine()
    {
        ClearAllHighlights();
        currentUnit.SetHighlight(true);

        messageText.text = currentUnit.unitName + " heals all enemies.";

        currentUnit.PlayAttackAnimation();
        currentUnit.PlaySkillSFX();

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (!currentEnemies[i].gameObject.activeInHierarchy)
            {
                continue;
            }

            if (currentEnemies[i].IsDead())
            {
                continue;
            }

            currentUnit.SpawnHealEffectOn(currentEnemies[i]);
            currentEnemies[i].Heal(currentUnit.enemyHealAmount);
        }

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    private IEnumerator EnemyMagicAoERoutine()
    {
        ClearAllHighlights();
        currentUnit.SetHighlight(true);

        messageText.text = currentUnit.unitName + " casts magic on all players.";

        currentUnit.PlayAttackAnimation();
        currentUnit.PlayAttackSFX();

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsDead())
            {
                continue;
            }

            players[i].SetHighlight(true);
            currentUnit.SpawnAttackEffectAtIndex(i);
            players[i].TakeDamage(currentUnit.enemyMagicDamage);
        }

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    public void OnClickAttack()
    {
        if (waitingForInventoryItem)
        {
            return;
        }

        if (waitingForPlayerTarget)
        {
            if (selectedAction == ActionType.Attack)
            {
                CancelAttackSelection();
            }

            return;
        }

        selectedAction = ActionType.Attack;
        waitingForPlayerTarget = true;
        waitingForInventoryItem = false;

        ShowAvailableEnemyTargets();
        HideInventoryPanel();
        SetAttackButtonCancel();
        SetInventoryButtonNormal();

        if (skillButton != null)
        {
            skillButton.interactable = false;
        }

        if (inventoryButton != null)
        {
            inventoryButton.interactable = false;
        }

        messageText.text = "Choose an enemy to attack.";
    }

    public void OnClickSkill()
    {
        if (waitingForInventoryItem)
        {
            return;
        }

        if (waitingForPlayerTarget)
        {
            if (selectedAction == ActionType.Skill)
            {
                CancelSkillSelection();
            }

            return;
        }

        selectedAction = ActionType.Skill;

        if (currentUnit.skillType == BattleUnit.SkillType.Damage)
        {
            if (!CurrentUnitHasEnoughMana())
            {
                ReturnToPlayerActionState(currentUnit.unitName + " does not have enough mana.");
                return;
            }

            waitingForPlayerTarget = true;
            waitingForInventoryItem = false;
            ShowAvailableEnemyTargets();
            HideInventoryPanel();

            if (attackButton != null)
            {
                attackButton.interactable = false;
            }

            if (inventoryButton != null)
            {
                inventoryButton.interactable = false;
            }

            if (skillButton != null)
            {
                skillButton.interactable = true;
            }

            messageText.text = currentUnit.unitName + " skill: " + currentUnit.GetSkillDescription();
            return;
        }

        if (!currentUnit.TryUseMana())
        {
            ReturnToPlayerActionState(currentUnit.unitName + " does not have enough mana.");
            return;
        }

        waitingForPlayerTarget = false;
        waitingForInventoryItem = false;
        HideAllEnemyTargetButtons();
        HideActionButtons();
        HideInventoryPanel();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

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

    public void OnClickInventory()
    {
        if (battleEnded)
        {
            return;
        }

        if (changingRound)
        {
            return;
        }

        if (itemUseLocked)
        {
            return;
        }

        if (currentUnit == null)
        {
            return;
        }

        if (!currentUnit.isPlayer)
        {
            return;
        }

        if (waitingForPlayerTarget)
        {
            return;
        }

        if (waitingForInventoryItem)
        {
            CancelInventorySelection();
            return;
        }

        selectedAction = ActionType.Item;
        waitingForInventoryItem = true;
        waitingForPlayerTarget = false;

        HideAllEnemyTargetButtons();
        ShowInventoryPanel();
        SetAttackButtonNormal();
        SetInventoryButtonCancel();

        if (attackButton != null)
        {
            attackButton.interactable = false;
        }

        if (skillButton != null)
        {
            skillButton.interactable = false;
        }

        if (inventoryButton != null)
        {
            inventoryButton.interactable = true;
        }

        messageText.text = "Choose an item and drag it to a target.";
    }

    public bool CanUseInventoryItem()
    {
        if (battleEnded)
        {
            return false;
        }

        if (changingRound)
        {
            return false;
        }

        if (itemUseLocked)
        {
            return false;
        }

        if (currentUnit == null)
        {
            return false;
        }

        if (!currentUnit.isPlayer)
        {
            return false;
        }

        if (!waitingForInventoryItem)
        {
            return false;
        }

        return selectedAction == ActionType.Item;
    }

    public void NotifyInventoryItemUsed()
    {
        if (!CanUseInventoryItem())
        {
            return;
        }

        itemUseLocked = true;
        selectedAction = ActionType.None;
        waitingForInventoryItem = false;
        waitingForPlayerTarget = false;

        HideInventoryPanel();
        HideActionButtons();
        HideAllEnemyTargetButtons();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        StartCoroutine(FinishInventoryItemRoutine());
    }

    private IEnumerator FinishInventoryItemRoutine()
    {
        yield return new WaitForSeconds(itemUseDelay);

        FinishCurrentUnitTurn();
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

        bool isSkillAttack = selectedAction == ActionType.Skill;

        if (isSkillAttack)
        {
            if (!currentUnit.TryUseMana())
            {
                ReturnToPlayerActionState(currentUnit.unitName + " does not have enough mana.");
                return;
            }
        }

        ClearAllHighlights();
        currentUnit.SetHighlight(true);
        target.SetHighlight(true);

        int damage = 0;

        if (selectedAction == ActionType.Attack)
        {
            damage = currentUnit.GetAttackDamage();
            messageText.text = currentUnit.unitName + " attacks " + target.unitName + ".";
        }

        if (selectedAction == ActionType.Skill)
        {
            damage = GetModifiedSkillDamage(currentUnit);
            messageText.text = currentUnit.unitName + " uses skill on " + target.unitName + ".";
        }

        waitingForPlayerTarget = false;
        selectedAction = ActionType.None;

        HideActionButtons();
        HideAllEnemyTargetButtons();
        HideInventoryPanel();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        StartCoroutine(PlayerAttackRoutine(target, damage, isSkillAttack));
    }

    private IEnumerator PlayerAttackRoutine(BattleUnit target, int damage, bool isSkillAttack)
    {
        currentUnit.PlayAttackAnimation();

        if (isSkillAttack)
        {
            currentUnit.PlaySkillSFX();
        }
        else
        {
            currentUnit.PlayAttackSFX();
        }

        yield return new WaitForSeconds(0.4f);

        currentUnit.SpawnAttackEffectOn(target);
        target.TakeDamage(damage);

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    private IEnumerator PlayerTeamHealRoutine()
    {
        currentUnit.PlayAttackAnimation();
        currentUnit.PlaySkillSFX();

        messageText.text = currentUnit.unitName + " uses skill: heal all allies.";

        yield return new WaitForSeconds(0.4f);

        BattleUnit[] allyTeam = GetAlliesOf(currentUnit);

        for (int i = 0; i < allyTeam.Length; i++)
        {
            if (!allyTeam[i].IsDead())
            {
                currentUnit.SpawnHealEffectAtIndex(i);
                allyTeam[i].Heal(currentUnit.healAmount);
            }
        }

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    private IEnumerator PlayerTeamShieldRoutine()
    {
        currentTurnGaveTeamShield = true;

        currentUnit.PlayAttackAnimation();
        currentUnit.PlaySkillSFX();

        messageText.text = currentUnit.unitName + " uses skill: all allies ignore damage until their next turn ends.";

        BattleUnit[] allyTeam = GetAlliesOf(currentUnit);

        for (int i = 0; i < allyTeam.Length; i++)
        {
            if (!allyTeam[i].IsDead())
            {
                allyTeam[i].GiveShield();
            }
        }

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    private IEnumerator PlayerAoERoutine()
    {
        int damage = GetModifiedSkillDamage(currentUnit);

        currentUnit.PlayAttackAnimation();
        currentUnit.PlaySkillSFX();

        messageText.text = currentUnit.unitName + " uses skill: deal damage to all enemies.";

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (!currentEnemies[i].IsDead() && currentEnemies[i].gameObject.activeInHierarchy)
            {
                currentUnit.SpawnAttackEffectOn(currentEnemies[i]);
                currentEnemies[i].TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(0.8f);

        FinishCurrentUnitTurn();
    }

    private void FinishCurrentUnitTurn()
    {
        currentUnit.AdvanceBuffTurn(currentTurnGaveTeamShield);
        currentTurnGaveTeamShield = false;

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

    private int GetRandomAlivePlayerIndex()
    {
        List<int> alivePlayerIndexes = new List<int>();

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsDead())
            {
                alivePlayerIndexes.Add(i);
            }
        }

        if (alivePlayerIndexes.Count == 0)
        {
            return -1;
        }

        int randomIndex = Random.Range(0, alivePlayerIndexes.Count);
        return alivePlayerIndexes[randomIndex];
    }

    private IEnumerator LoadLoseSceneRoutine()
    {
        loadingResultScene = true;

        yield return new WaitForSeconds(resultSceneDelay);

        int fightNumber = GetBattleLevelNumber();

        if (loseSceneChange != null)
        {
            loseSceneChange.LoadBattleResultScene(fightNumber, false, resultSceneName);
            yield break;
        }

        SceneChange sceneChange = FindObjectOfType<SceneChange>();

        if (sceneChange != null)
        {
            sceneChange.LoadBattleResultScene(fightNumber, false, resultSceneName);
            yield break;
        }

        PlayerPrefs.SetInt("LastFightNumber", fightNumber);
        PlayerPrefs.SetInt("LastFightWon", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(resultSceneName);
    }

    private IEnumerator LoadWinSceneRoutine()
    {
        loadingResultScene = true;

        yield return new WaitForSeconds(resultSceneDelay);

        int fightNumber = GetBattleLevelNumber();

        if (winSceneChange != null)
        {
            winSceneChange.LoadBattleResultScene(fightNumber, true, resultSceneName);
            yield break;
        }

        SceneChange sceneChange = FindObjectOfType<SceneChange>();

        if (sceneChange != null)
        {
            sceneChange.LoadBattleResultScene(fightNumber, true, resultSceneName);
            yield break;
        }

        PlayerPrefs.SetInt("LastFightNumber", fightNumber);
        PlayerPrefs.SetInt("LastFightWon", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(resultSceneName);
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
            HideInventoryPanel();
            ClearAllHighlights();
            SetAttackButtonNormal();
            SetInventoryButtonNormal();
            messageText.text = "All players are defeated.";

            if (!loadingResultScene)
            {
                StartCoroutine(LoadLoseSceneRoutine());
            }

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
            HideInventoryPanel();
            ClearAllHighlights();
            SetAttackButtonNormal();
            SetInventoryButtonNormal();
            messageText.text = "All enemies are defeated.";

            if (!loadingResultScene)
            {
                StartCoroutine(LoadWinSceneRoutine());
            }
        }
    }

    private IEnumerator StartNextRoundRoutine()
    {
        changingRound = true;

        HideActionButtons();
        HideAllEnemyTargetButtons();
        HideInventoryPanel();
        ClearAllHighlights();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

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

        attackButton.interactable = true;
        skillButton.interactable = true;

        if (inventoryButton != null)
        {
            inventoryButton.gameObject.SetActive(true);
            inventoryButton.interactable = true;
        }
    }

    private void HideActionButtons()
    {
        attackButton.gameObject.SetActive(false);
        skillButton.gameObject.SetActive(false);

        if (inventoryButton != null)
        {
            inventoryButton.gameObject.SetActive(false);
        }

        SetAttackButtonNormal();
        SetInventoryButtonNormal();
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
        waitingForInventoryItem = false;
        itemUseLocked = false;
        selectedAction = ActionType.None;
        currentTurnGaveTeamShield = false;

        HideActionButtons();
        HideAllEnemyTargetButtons();
        HideInventoryPanel();
        ClearAllHighlights();
        SetAttackButtonNormal();
        SetInventoryButtonNormal();

        SetRound(2);

        messageText.text = "Round 2 starts.";

        StartTurn();
    }

    public bool IsChoosingAttackTarget()
    {
        if (battleEnded)
        {
            return false;
        }

        if (changingRound)
        {
            return false;
        }

        if (!waitingForPlayerTarget)
        {
            return false;
        }

        return selectedAction == ActionType.Attack || selectedAction == ActionType.Skill;
    }
}