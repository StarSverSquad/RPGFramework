using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPGF.RPG;
using static BattleTurnData;

public class BattlePipeline
{
    public enum ChoiceAction
    {
        Primary, Special, Act, Ability, Entity, Teammate, Enemy,
        Item, Flee, Battle
    }

    private readonly BattleManager _battle;
    private readonly CommonManager _common;

    #region PROPS

    public bool IsWin { get; private set; }
    public bool IsLose { get; private set; }
    public bool IsFlee { get; private set; }

    public bool IsPlayerTurn { get; private set; } = false;
    public bool IsEnemyTurn { get; private set; } = false;

    public int TurnCounter { get; private set; }
    public int CurrentTurnDataIndex { get; private set; }

    #endregion

    private List<ChoiceAction> choiceActions = new();

    private bool loseKey, winKey, fleeKey, breakKey;
    private bool isCancelChoice;
    private bool isFlee;
    private int actionIndex;

    private Coroutine main = null;

    #region READONLY PROPS

    public BattleData Data => _battle.data;
    public BattleChoiceManager Choice => _battle.Choice;
    public BattleUtility Utility => _battle.Utility;
    public BattleVisualTransmitionManager VisualTransmition => _battle.VisualTransmition;
    public BattleUIManager UI => _battle.UI;

    public bool MainIsWorking => main != null;

    public ChoiceAction CurrentChoiceAction => choiceActions.Last();

    public BattleTurnData CurrentTurnData => Data.TurnsData[CurrentTurnDataIndex];

    private bool AllKeysFalse => !loseKey && !winKey && !fleeKey && !breakKey;

    #endregion

    public BattlePipeline(BattleManager battle, CommonManager common)
    {
        _battle = battle;
        _common = common;

        TurnCounter = 0;
        CurrentTurnDataIndex = 0;
    }

    #region API

    public void InvokeMainPipeline()
    {
        if (!MainIsWorking)
            main = _battle.StartCoroutine(MainPipeline());
    }

    public void InvokeLose()
    {
        loseKey = true;
    }

    public void InvokeWin()
    {
        winKey = true;
    }

    public void InvokeFlee()
    {
        fleeKey = true;
    }

    public void InvokeBreak()
    {
        breakKey = true;
    }

    #endregion

    private void NextCharacter()
    {
        _battle.UI.CharacterBox.Boxes[CurrentTurnDataIndex].ChangeAct(Data.TurnsData[CurrentTurnDataIndex].BattleAction);

        UI.CharacterQuery.NextPosition();

        CurrentTurnDataIndex++;

        choiceActions.Clear();
    }
    private void PreviewAction()
    {
        choiceActions.Remove(choiceActions.Last());
    }

    private bool ShouldSkipTurn(BattleTurnData turnData)
    {
        if (turnData.IsDead || turnData.Character.States.Any(i => i.SkipTurn) || !turnData.Character.CanMoveInBattle)
        {
            if (isCancelChoice && CurrentTurnDataIndex != 0)
            {
                CurrentTurnDataIndex--;

                UI.CharacterQuery.PreviewPosition();
            }
            else
            {
                turnData.BattleAction = TurnAction.None;
                CurrentTurnDataIndex++;

                UI.CharacterQuery.NextPosition();

                isCancelChoice = false;

            }
            return true;
        }

        return false;
    }

    private IEnumerator MainPipeline()
    {
        loseKey = false; winKey = false; fleeKey = false; breakKey = false;

        IsFlee = false; IsWin = false; IsLose = false;

        TurnCounter = 0;

        yield return BattleEnter();

        yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnBattleStart);

        if (Data.BattleInfo.EnemyStart)
        {
            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnEnemyTurn, true);

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.EveryEnemyTurn);

            yield return EnemyTurn();

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessCharacterHeal);

            TurnCounter++;
        }

        while (true)
        {
            if (Data.Enemys.Count == 0)
                InvokeWin();

            if (Data.TurnsData.All(i => i.Character.Heal <= 0))
                InvokeLose();

            if (!AllKeysFalse)
                break;

            yield return UpdateEntitysStates();

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnPlayerTurn, true);

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.EveryPlayerTurn);

            if (!AllKeysFalse)
                break;

            yield return PlayerTurn();

            if (Data.Enemys.Count == 0)
                InvokeWin();

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnEnemyTurn, true);

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.EveryEnemyTurn);

            if (!AllKeysFalse)
                break;

            yield return EnemyTurn();

            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessCharacterHeal);

            TurnCounter++;
        }

        UI.Concentration.Hide();
        UI.CharacterSide.Hide();
        UI.PlayerTurnSide.Hide();
        UI.CharacterBox.Hide();

        if (fleeKey)
        {
            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnFlee);

            yield return Flee();
        }

        if (winKey)
        {
            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnWin);

            yield return Win();
        }

        if (loseKey)
        {
            yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLose);

            yield return Lose();
        }

        yield return InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnBattleEnd);

        main = null;

        yield return BattleExit();
    }

    private IEnumerator BattleEnter()
    {
        foreach (var character in GameManager.Instance.Character.Characters)
        {
            if (character.ParticipateInBattle)
                Data.TurnsData.Add(new BattleTurnData(character));
        }

        if (Data.BattleInfo.StopGlobalMusic)
            GameManager.Instance.GameAudio.PauseBGM(0.2f);

        if (Data.BattleInfo.StopGlobalMusic)
            GameManager.Instance.GameAudio.PauseBGS();

        VisualTransmition.InitializeCustomEffect(Data.BattleInfo.BattleEnterEffect);

        yield return _battle.StartCoroutine(VisualTransmition.InvokePartOne());

        _battle.SetActive(true);

        yield return new WaitForFixedUpdate();

        _battle.UI.CharacterBox.Initialize(Data.TurnsData.Select(i => i.Character).ToArray());

        Choice.PrimaryChoice.SetActive(false);

        _battle.UI.Description.SetActive(false);

        _battle.UI.Concentration.SetConcentration(0);

        _battle.Background.CreateBackground(Data.BattleInfo.Background);

        if (Data.BattleInfo.BattleMusic != null)
            _battle.BattleAudio.PlayMusic(Data.BattleInfo.BattleMusic, Data.BattleInfo.MusicVolume);

        yield return _battle.StartCoroutine(VisualTransmition.InvokePartTwo());

        VisualTransmition.DisposeCustomEffect();

        if (Data.BattleInfo.ShowStartMessage)
        {
            string rusMultiText = Data.BattleInfo.enemySquad.Enemies.Count > 1 ? "вступают в битву" : "вступает в битву";

            _common.MessageBox.Write(new MessageInfo()
            {
                text = $"* {Data.BattleInfo.enemySquad.Name} {rusMultiText}!",
                closeWindow = true
            });

            yield return new WaitWhile(() => _common.MessageBox.IsWriting);
        }
    }

    private IEnumerator BattleExit()
    {
        VisualTransmition.InitializeCustomEffect(Data.BattleInfo.BattleExitEffect);

        yield return _battle.StartCoroutine(VisualTransmition.InvokePartOne());

        foreach (var item in Data.TurnsData)
        {
            if (item.Character.Heal <= 0)
                item.Character.Heal = 1;

            item.Character.RemoveNonBattleStates();
        }


        if (Data.BattleInfo.StopGlobalMusic)
            GameManager.Instance.GameAudio.ResumeBGM();

        if (Data.BattleInfo.StopGlobalMusic)
            GameManager.Instance.GameAudio.ResumeBGS();

        Data.Dispose();

        _battle.SetActive(false);

        _battle.UI.CharacterBox.Dispose();

        _battle.EnemyModels.Dispose();

        yield return _battle.StartCoroutine(VisualTransmition.InvokePartTwo());

        VisualTransmition.DisposeCustomEffect();
    }

    private IEnumerator PlayerTurn()
    {
        IsPlayerTurn = true;

        choiceActions.Clear();

        Data.TurnsData.ForEach(i => i.CleanUp());

        CurrentTurnDataIndex = 0;

        UI.CharacterSide.Show();
        UI.PlayerTurnSide.Show();
        UI.Concentration.Show();
        UI.CharacterQuery.Show();

        Choice.PrimaryChoice.SetActive(true);

        actionIndex = 0;
        isCancelChoice = false;

        // Установление иконки действия
        for (int i = 0; i < Data.TurnsData.Count; i++)
            _battle.UI.CharacterBox.Boxes[i].ChangeAct(TurnAction.None);

        while (CurrentTurnDataIndex < Data.TurnsData.Count)
        {
            BattleTurnData currenTurnData = Data.TurnsData[CurrentTurnDataIndex];

            if (ShouldSkipTurn(currenTurnData))
                continue;

            isCancelChoice = false;

            if (choiceActions.Count == 0)
                choiceActions.Add(ChoiceAction.Primary);

            if (currenTurnData.ReservedConcentration != 0)
            {
                Utility.AddConcetration(-currenTurnData.ReservedConcentration);
                currenTurnData.ReservedConcentration = 0;
            }

            yield return HandleChoiceAction(currenTurnData);
        }

        UI.CharacterSide.Hide();
        UI.PlayerTurnSide.Hide();
        UI.CharacterQuery.Hide();
        UI.Concentration.NearWindow();

        yield return new WaitForSeconds(.5f);

        UI.CharacterBox.Show();

        yield return new WaitForSeconds(UI.CharacterBox.TraslateContainerTime);

        Choice.PrimaryChoice.SetActive(false);

        /// ЦИКЛ ПОСЛЕДСВИЙ ВЫБОРА
        for (int charIndex = 0; charIndex < Data.TurnsData.Count; charIndex++)
        {
            var turnData = Data.TurnsData[charIndex];

            isFlee = false;

            RPGCharacter currentCharacter = turnData.Character;

            // Если персонаж пал или пропускает ход или не может ничего делать в битве, то его надо пропустить 
            if (turnData.IsDead || currentCharacter.States.Any(i => i.SkipTurn) || !currentCharacter.CanMoveInBattle)
                continue;

            UI.CharacterBox.FocusBox(currentCharacter);

            yield return new WaitForSeconds(UI.CharacterBox.TraslateBoxTime);

            turnData.ReservedConcentration = 0;

            yield return HandleBattleAction(turnData, currentCharacter);

            UI.CharacterBox.UnfocusBox(currentCharacter);

            if (Data.Enemys.Any(i => i.Heal <= 0))
            {
                RPGEnemy[] deads = Data.Enemys.Where(i => i.Heal <= 0).ToArray();

                for (int i = 0; i < deads.Length; i++)
                {
                    yield return new WaitWhile(() => _battle.EnemyModels.GetModel(deads[i]).IsAnimatingEffect);

                    BattleManager.BattleUtility.RemoveEnemy(deads[i]);
                }
            }

            if (Data.Enemys.Count == 0 || isFlee)
                break;
        }

        _battle.AttackQTE.Hide();

        IsPlayerTurn = false;
    }

    private IEnumerator InvokeBattleEvent(RPGBattleEvent.InvokePeriod period, bool byTurn = false, string byEntity = "")
    {
        List<RPGBattleEvent> events = Data.BattleInfo.Events;

        if (byTurn)
            events = events.Where(i => i.Turn == TurnCounter).ToList();

        if (!string.IsNullOrEmpty(byEntity))
        {
            events = events.Where(i => i.EntityTag == byEntity).ToList();
        }

        events = events.Where(i => i.Period == period).ToList();

        if (period == RPGBattleEvent.InvokePeriod.OnLessCharacterHeal)
        {
            events = events.Where(i => Data.TurnsData.Select(i => i.Character).Any(ch => ch.Heal <= i.Heal && i.EntityTag == ch.Tag)).ToList();
        }
        else if (period == RPGBattleEvent.InvokePeriod.OnLessEnemyHeal)
        {
            events = events.Where(i => Data.Enemys.Any(enem => enem.Heal <= i.Heal && i.EntityTag == enem.Tag)).ToList();
        }

        foreach (var @event in events)
        {
            @event.Event.Invoke(_battle);

            yield return new WaitWhile(() => @event.Event.IsPlaying);
        }

        yield break;
    }

    private IEnumerator UpdateEntitysStates()
    {
        foreach (var turnData in Data.TurnsData)
        {
            if (turnData.Character.States.Length == 0)
                continue;

            foreach (var state in turnData.Character.States)
            {
                if (state.Event != null)
                {
                    state.Event.Invoke(_battle);

                    yield return new WaitWhile(() => state.Event.IsPlaying);
                }
            }

            int oldHeal = turnData.Character.Heal;

            turnData.Character.UpdateAllStates();

            CharacterBox box = _battle.UI.CharacterBox.GetBox(turnData.Character);

            if (turnData.Character.Heal < oldHeal)
            {
                BattleManager.BattleUtility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.4f), (oldHeal - turnData.Character.Heal).ToString(), Color.white, Color.red);
            }
            else if (turnData.Character.Heal > oldHeal)
            {
                BattleManager.BattleUtility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.4f), (oldHeal - turnData.Character.Heal).ToString(), Color.white, Color.green);
            } 

        }

        foreach (var enemy in Data.Enemys)
        {
            if (enemy.States.Length == 0)
                continue;

            int oldHeal = enemy.Heal;

            enemy.UpdateAllStates();

            EnemyModel model = _battle.EnemyModels.GetModel(enemy);

            if (enemy.Heal < oldHeal)
            {
                BattleManager.BattleUtility.SpawnFallingText(model.DamageTextGlobalPoint, (oldHeal - enemy.Heal).ToString(), Color.white, Color.red);
            }
            else if (enemy.Heal > oldHeal)
            {
                BattleManager.BattleUtility.SpawnFallingText(model.DamageTextGlobalPoint, (oldHeal - enemy.Heal).ToString(), Color.white, Color.green);
            }
        }
    }

    private IEnumerator EnemyTurn()
    {
        IsEnemyTurn = true;

        yield return new WaitForSeconds(0.5f);

        List<BattleAttackPatternBase> patterns = new List<BattleAttackPatternBase>();
        List<BattleTurnData> targets = new List<BattleTurnData>();

        foreach (var enemy in Data.Enemys)
        {
            if (enemy.States.Any(i => i.SkipTurn))
                continue;

            BattleAttackPatternBase pattern = enemy.Patterns[Random.Range(0, enemy.Patterns.Count)];
            pattern.enemy = enemy;

            patterns.Add(pattern);
        }

        foreach (var pattern in patterns)
            _battle.Pattern.AddPattern(pattern);

        int chars = Random.Range(1, Data.TurnsData.Where(i => !i.IsDead).Count() + 1);

        for (int i = 0; i < chars; i++)
        {
            BattleTurnData turnData = Data.TurnsData[Random.Range(0, Data.TurnsData.Count)];

            if (turnData.IsTarget || turnData.IsDead)
            {
                i--;
                continue;
            }

            turnData.IsTarget = true;

            _battle.UI.CharacterBox.GetBox(turnData.Character).MarkTarget(true);

            targets.Add(turnData);
        }

        _battle.Player.SetActive(true);
        _battle.BattleField.SetActive(true);

        _battle.Pattern.Invoke(patterns.Count <= 1);

        yield return new WaitWhile(() => _battle.Pattern.IsAttack && !loseKey && !breakKey);

        if (loseKey || breakKey)
            _battle.Pattern.Break();

        foreach (var item in targets)
        {
            item.IsTarget = false;
            _battle.UI.CharacterBox.GetBox(item.Character).MarkTarget(false);
        }

        _battle.Player.SetActive(false);
        _battle.BattleField.SetActive(false);

        UI.CharacterBox.Hide();

        IsEnemyTurn = false;
    }

    private IEnumerator Lose()
    {
        IsLose = true;

        UI.Concentration.Hide();
        UI.CharacterSide.Hide();
        UI.PlayerTurnSide.Hide();
        UI.CharacterBox.Hide();

        PlayerPrefs.SetFloat("DeadX", _battle.Player.transform.position.x - Camera.main.transform.position.x);
        PlayerPrefs.SetFloat("DeadY", _battle.Player.transform.position.y - Camera.main.transform.position.y);
        PlayerPrefs.Save();

        _battle.BattleAudio.StopMusic();

        if (Data.BattleInfo.ShowDeadMessage)
        {
            _battle.BattleAudio.PlaySound(Data.Lose);

            _battle.UI.CharacterBox.SetActive(false);

            _common.MessageBox.Write(new MessageInfo()
            {
                text = "* Ваша команда проебала!",
                closeWindow = true,
            });

            yield return new WaitWhile(() => _common.MessageBox.IsWriting);
        }

        if (!Data.BattleInfo.CanLose)
            SceneManager.LoadScene(Data.GameOverSceneName);
    }

    private IEnumerator Win()
    {
        IsWin = true;

        _battle.BattleAudio.StopMusic();

        #region ДРОП

        string lvlUpText = string.Empty;
        string moneyText = string.Empty;
        string dropText = string.Empty;

        foreach (var character in Data.TurnsData)
        {
            int addExp = Data.BattleInfo.enemySquad.Expireance;

            if (!Data.BattleInfo.enemySquad.ExpConstDrop)
                addExp = Mathf.RoundToInt(Random.Range(addExp * 0.75f, addExp * 1.25f));

            character.Character.Expireance += addExp;

            if (character.Character.LevelUpCanExecute())
            {
                character.Character.LevelUp();

                lvlUpText += $"* {character.Character.Name} становиться сильнее!\n";
            }
        }

        if (Data.BattleInfo.enemySquad.Money > 0)
        {
            int money = Data.BattleInfo.enemySquad.Money;

            if (Data.BattleInfo.enemySquad.MoneyConstDrop)
                money = Mathf.RoundToInt(Random.Range(money * 0.65f, money * 1.35f));

            GameManager.Instance.GameData.Money += money;

            moneyText += $"* Вы получили {money} {GameManager.ILocalization.GetLocale("Base_Money")}!\n";
        }

        bool first = true;
        foreach (var drop in Data.BattleInfo.enemySquad.EnemiesDrop)
        {
            if (Random.Range(0f, 1f) > drop.Chance)
                continue;

            int count = Mathf.RoundToInt(Random.Range(drop.Count - drop.CountRange, drop.Count + drop.CountRange));

            GameManager.Instance.Inventory.AddToItemCount(drop.item, count);

            string countText = count > 1 ? $" {count}x" : "";

            if (first)
            {
                dropText += $"* Вы получили {drop.item.Name}{countText}";

                first = false;
            }
            else
                dropText += $", {drop.item.Name}{countText}";
        }

        if (!first)
            dropText += "!\n";

        #endregion

        if (Data.BattleInfo.ShowEndMessage)
        {
            _battle.BattleAudio.PlaySound(Data.Win);

            _common.MessageBox.Write(new MessageInfo()
            {
                text = "* Ваша команда одержала победу<!>",
                closeWindow = true
            });

            yield return new WaitWhile(() => _common.MessageBox.IsWriting);

            if (dropText != string.Empty || moneyText != string.Empty)
            {
                _common.MessageBox.Write(new MessageInfo()
                {
                    text = moneyText + dropText,
                    closeWindow = true
                });

                yield return new WaitWhile(() => _common.MessageBox.IsWriting);
            }

            if (lvlUpText != string.Empty)
            {
                _common.MessageBox.Write(new MessageInfo()
                {
                    text = lvlUpText,
                    closeWindow = true
                });

                yield return new WaitWhile(() => _common.MessageBox.IsWriting);
            }
        }

        _battle.UI.CharacterBox.SetActive(false);

    }

    private IEnumerator Flee()
    {
        _battle.BattleAudio.StopMusic();

        IsFlee = true;

        yield break;
    }

    #region HANDLE CHOICES

    private IEnumerator HandleChoiceAction(BattleTurnData turnData)
    {
        switch (choiceActions.Last())
        {
            case ChoiceAction.Primary:
                yield return HandlePrimaryChoice(turnData);
                break;
            case ChoiceAction.Special:
                yield return HandleSpecialChoice(turnData);
                break;
            case ChoiceAction.Act:
                yield return HandleActChoice(turnData);
                break;
            case ChoiceAction.Ability:
                yield return HandleAbilityChoice(turnData);
                break;
            case ChoiceAction.Entity:
                yield return HandleEntityChoice(turnData);
                break;
            case ChoiceAction.Teammate:
                yield return HandleTeammateChoice(turnData);
                break;
            case ChoiceAction.Enemy:
                yield return HandleEnemyChoice(turnData);
                break;
            case ChoiceAction.Item:
                yield return HandleItemChoice(turnData);
                break;
            case ChoiceAction.Flee:
                yield return HandleFleeChoice(turnData);
                break;
            case ChoiceAction.Battle:
                yield return HandleBattleChoice(turnData);
                break;
        }
    }

    private IEnumerator HandlePrimaryChoice(BattleTurnData turnData)
    {
        UI.CharacterSide.Setup(turnData.Character);

        turnData.CleanUp();

        Choice.InvokePrimaryChoice(actionIndex);

        yield return new WaitWhile(() => Choice.IsChoicing);

        if (Choice.IsPrimaryCanceled)
        {
            if (CurrentTurnDataIndex > 0)
            {
                CurrentTurnDataIndex--;

                UI.CharacterQuery.PreviewPosition();
                isCancelChoice = true;
            }
            _battle.UI.CharacterBox.Boxes[CurrentTurnDataIndex].ChangeAct(TurnAction.None);
        }
        else
        {
            actionIndex = Choice.PrimaryCurrentIndex;

            switch (actionIndex)
            {
                // Выбрана битва
                case 0:
                    choiceActions.Add(ChoiceAction.Battle);
                    break;
                // Выбрано дейтсвие
                case 1:
                    choiceActions.Add(ChoiceAction.Special);
                    break;
                // Выбраны вещи
                case 2:
                    turnData.BattleAction = TurnAction.Item;

                    choiceActions.Add(ChoiceAction.Item);
                    break;
                // Выбрана побег
                case 3:
                    choiceActions.Add(ChoiceAction.Flee);
                    break;
                // Неизветсное действие
                default:
                    Debug.LogWarning("Unknown battle action!");
                    break;
            }
        }
    }
    private IEnumerator HandleSpecialChoice(BattleTurnData turnData)
    {
        _battle.Choice.InvokeChoiceAct();

        yield return new WaitWhile(() => _battle.Choice.IsChoicing);

        if (_battle.Choice.IsCanceled)
            PreviewAction();
        else
        {
            if ((int)Choice.CurrentItem.Value == 0)
            {
                turnData.BattleAction = TurnAction.Act;
                choiceActions.Add(ChoiceAction.Enemy);
            }
            else
            {
                turnData.BattleAction = TurnAction.Ability;
                choiceActions.Add(ChoiceAction.Ability);
            }
        }

        _battle.Choice.CleanUp();
    }
    private IEnumerator HandleActChoice(BattleTurnData turnData)
    {
        _battle.Choice.InvokeChoiceInteraction();

        yield return new WaitWhile(() => _battle.Choice.IsChoicing);

        if (_battle.Choice.IsCanceled)
        {
            turnData.InteractionAct = RPGEnemy.EnemyAct.NullAct;

            turnData.EnemyBuffer = null;

            PreviewAction();
        }
        else
        {
            turnData.InteractionAct = (RPGEnemy.EnemyAct)Choice.CurrentItem.Value;

            NextCharacter();
        }

        Choice.CleanUp();
    }
    private IEnumerator HandleAbilityChoice(BattleTurnData turnData)
    {
        Choice.InvokeChoiceAbility();

        yield return new WaitWhile(() => Choice.IsChoicing);

        if (Choice.IsCanceled)
        {
            turnData.Ability = null;

            PreviewAction();
        }
        else
        {
            turnData.Ability = (RPGAbility)Choice.CurrentItem.Value;

            switch (turnData.Ability.Direction)
            {
                case UsabilityDirection.All:
                case UsabilityDirection.AllEnemys:
                case UsabilityDirection.AllTeam:
                    turnData.ReservedConcentration = -turnData.Ability.ConcentrationCost;
                    Utility.AddConcetration(-turnData.Ability.ConcentrationCost);

                    NextCharacter();
                    break;
                case UsabilityDirection.Teammate:
                    choiceActions.Add(ChoiceAction.Teammate);
                    break;
                case UsabilityDirection.Enemy:
                    choiceActions.Add(ChoiceAction.Enemy);
                    break;
                case UsabilityDirection.Any:
                    choiceActions.Add(ChoiceAction.Entity);
                    break;
            }
        }

        _battle.Choice.CleanUp();
    }
    private IEnumerator HandleEntityChoice(BattleTurnData turnData)
    {
        _battle.Choice.InvokeChoiceEntity();

        yield return new WaitWhile(() => _battle.Choice.IsChoicing);

        if (_battle.Choice.IsCanceled)
        {
            if (turnData.BattleAction == TurnAction.Item)
                turnData.Item = null;

            PreviewAction();
        }
        else
        {
            turnData.EntityBuffer = (RPGEntity)_battle.Choice.CurrentItem.Value;

            switch (turnData.BattleAction)
            {
                case TurnAction.Act:
                    choiceActions.Add(ChoiceAction.Act);

                    break;
                case TurnAction.Ability:
                    turnData.ReservedConcentration = -turnData.Ability.ConcentrationCost;
                    Utility.AddConcetration(-turnData.Ability.ConcentrationCost);

                    NextCharacter();
                    break;
                default:
                    NextCharacter();
                    break;
            }
        }

        _battle.Choice.CleanUp();
    }
    private IEnumerator HandleTeammateChoice(BattleTurnData turnData)
    {
        Choice.InvokeChoiceTeammate();

        yield return new WaitWhile(() => Choice.IsChoicing);

        if (Choice.IsCanceled)
        {
            if (turnData.BattleAction == TurnAction.Item)
                turnData.Item = null;

            PreviewAction();
        }
        else
        {
            turnData.CharacterBuffer = (RPGCharacter)Choice.CurrentItem.Value;

            switch (turnData.BattleAction)
            {
                case TurnAction.Ability:
                    turnData.ReservedConcentration = -turnData.Ability.ConcentrationCost;
                    Utility.AddConcetration(-turnData.Ability.ConcentrationCost);
                    break;
            }

            NextCharacter();
        }

        Choice.CleanUp();
    }
    private IEnumerator HandleEnemyChoice(BattleTurnData turnData)
    {
        _battle.Choice.InvokeChoiceEnemy();

        yield return new WaitWhile(() => _battle.Choice.IsChoicing);

        if (_battle.Choice.IsCanceled)
        {
            if (turnData.BattleAction == TurnAction.Item)
                turnData.Item = null;

            turnData.EnemyBuffer = null;

            PreviewAction();
        }
        else
        {
            turnData.EnemyBuffer = (RPGEnemy)Choice.CurrentItem.Value;

            switch (turnData.BattleAction)
            {
                case TurnAction.Act:
                    choiceActions.Add(ChoiceAction.Act);

                    break;
                case TurnAction.Ability:
                    turnData.ReservedConcentration = -turnData.Ability.ConcentrationCost;
                    Utility.AddConcetration(-turnData.Ability.ConcentrationCost);

                    NextCharacter();
                    break;
                default:
                    NextCharacter();
                    break;
            }
        }

        _battle.Choice.CleanUp();
    }
    private IEnumerator HandleItemChoice(BattleTurnData turnData)
    {
        if (GameManager.Instance.Inventory.Slots
                    .Where(i => i.Item.Usage == Usability.Battle ||
                                i.Item.Usage == Usability.Any).Count() > 0)
        {
            _battle.Choice.InvokeChoiceItem();

            yield return new WaitWhile(() => _battle.Choice.IsChoicing);

            if (_battle.Choice.IsCanceled)
                PreviewAction();
            else
            {
                turnData.Item = _battle.Choice.CurrentItem.Value as RPGCollectable;
                turnData.IsConsumed = turnData.Item is RPGConsumed;

                if (turnData.Item is RPGConsumed consumed)
                {
                    switch (consumed.Direction)
                    {
                        case UsabilityDirection.AllEnemys:
                        case UsabilityDirection.AllTeam:
                        case UsabilityDirection.All:
                            NextCharacter();
                            break;
                        case UsabilityDirection.Teammate:
                            choiceActions.Add(ChoiceAction.Teammate);
                            break;
                        case UsabilityDirection.Enemy:
                            choiceActions.Add(ChoiceAction.Enemy);
                            break;
                        case UsabilityDirection.Any:
                            choiceActions.Add(ChoiceAction.Entity);
                            break;
                    }
                }
                else
                    NextCharacter();
            }

            _battle.Choice.CleanUp();
        }
        else
            PreviewAction();
    }
    private IEnumerator HandleFleeChoice(BattleTurnData turnData)
    {
        turnData.BattleAction = TurnAction.Flee;
        NextCharacter();

        yield break;
    }
    private IEnumerator HandleBattleChoice(BattleTurnData turnData)
    {
        _battle.Choice.InvokeChoiceBattle();

        yield return new WaitWhile(() => _battle.Choice.IsChoicing);

        if (_battle.Choice.IsCanceled)
            PreviewAction();
        else
        {
            int result = (int)_battle.Choice.CurrentItem.Value;

            if (result == 0)
            {
                turnData.BattleAction = TurnAction.Attack;

                choiceActions.Add(ChoiceAction.Enemy);
            }
            else if (result == 1)
            {
                Utility.AddConcetration(Data.AdditionConcentrationOnDefence);
                turnData.ReservedConcentration = Data.AdditionConcentrationOnDefence;
                turnData.BattleAction = TurnAction.Defence;

                NextCharacter();
            }
        }

        _battle.Choice.CleanUp();
    }

    #endregion

    #region HANDLE BATTLE ACTS

    private IEnumerator HandleBattleAction(BattleTurnData turnData, RPGCharacter character)
    {
        switch (turnData.BattleAction)
        {
            case TurnAction.Attack:
                yield return HandleAttackAction(turnData, character);
                break;
            case TurnAction.Act:
                yield return HandleActAction(turnData, character);
                break;
            case TurnAction.Ability:
                yield return HandleAbilityAction(turnData, character);
                break;
            case TurnAction.Item:
                yield return HandleItemAction(turnData, character);
                break;
            case TurnAction.Flee:
                yield return HandleFleeAction(turnData, character);
                break;
            case TurnAction.Defence:
            case TurnAction.None:
            default:
                break;
        }
    }

    private IEnumerator HandleAttackAction(BattleTurnData turnData, RPGCharacter character)
    {
        if (!Data.Enemys.Contains(turnData.EnemyBuffer))
        {
            if (Data.Enemys.Count != 0)
                turnData.EnemyBuffer = Data.Enemys[0];
            else 
                yield break;
        }

        _battle.AttackQTE.Show();

        if (!_battle.AttackQTE.IsShowed)
            yield return new WaitForSeconds(1f);

        _battle.AttackQTE.Invoke();

        yield return new WaitWhile(() => _battle.AttackQTE.QTE.IsWorking);

        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.BeforeHit, false, turnData.EnemyBuffer.Tag));

        BattleAttackEffect effect = character.WeaponSlot == null ? Data.DefaultEffect : character.WeaponSlot.VisualEffect;

        if (effect.LocaleInCenter)
            effect = BattleManager.BattleUtility.SpawnAttackEffect(effect);
        else
        {
            Vector2 attackPos = _battle.EnemyModels.GetModel(turnData.EnemyBuffer).AttackGlobalPoint;

            effect = BattleManager.BattleUtility.SpawnAttackEffect(effect, attackPos);
        }

        effect.Invoke();

        yield return new WaitWhile(() => effect.IsAnimating);

        yield return new WaitForSeconds(0.5f);

        Object.Destroy(effect.gameObject);

        _battle.AttackQTE.Hide();

        BattleManager.BattleUtility.DamageEnemy(character, turnData.EnemyBuffer, _battle.AttackQTE.QTE.DamageFactor);

        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.AfterHit, false, turnData.EnemyBuffer.Tag));

        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessEnemyHeal, false, turnData.EnemyBuffer.Tag));
    }
    private IEnumerator HandleActAction(BattleTurnData turnData, RPGCharacter character)
    {
        if (turnData.InteractionAct.Name == "Check")
        {
            _common.MessageBox.Write(new MessageInfo()
            {
                text = $"АТАКА: {turnData.EnemyBuffer.Damage}, ЗАЩИТА: {turnData.EnemyBuffer.Defence}<\\:>\n" +
                       $"{turnData.EnemyBuffer.Description}",
                closeWindow = true,
                wait = true
            });

            yield return new WaitWhile(() => _common.MessageBox.IsWriting);
        }
        else
        {
            turnData.InteractionAct.Event.Invoke(_battle);

            yield return new WaitWhile(() => turnData.InteractionAct.Event.IsPlaying);
        }
    }
    private IEnumerator HandleAbilityAction(BattleTurnData turnData, RPGCharacter character)
    {
        character.Mana -= turnData.Ability.ManaCost;

        if (turnData.Ability.StartEvent != null)
        {
            turnData.Ability.StartEvent.Invoke(_battle);

            yield return new WaitWhile(() => turnData.Ability.StartEvent.IsPlaying);
        }

        switch (turnData.Ability.Direction)
        {
            case UsabilityDirection.AllTeam:
                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, Data.TurnsData.Select(i => i.Character).ToArray()));
                break;
            case UsabilityDirection.Teammate:
                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, turnData.CharacterBuffer));
                break;
            case UsabilityDirection.AllEnemys:
                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, Data.Enemys.ToArray()));
                break;
            case UsabilityDirection.Enemy:
                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, turnData.EnemyBuffer));
                break;
            case UsabilityDirection.Any:
                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, turnData.EntityBuffer));
                break;
            case UsabilityDirection.All:
                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, Data.TurnsData.Select(i => i.Character).ToArray()));

                yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, Data.Enemys.ToArray()));
                break;
        }

        if (turnData.Ability.EndEvent != null)
        {
            turnData.Ability.EndEvent.Invoke(_battle);

            yield return new WaitWhile(() => turnData.Ability.EndEvent.IsPlaying);
        }
    }
    private IEnumerator HandleItemAction(BattleTurnData turnData, RPGCharacter character)
    {
        if (turnData.Item.Event != null)
        {
            turnData.Item.InvokeEvent();

            yield return new WaitWhile(() => turnData.Item.Event.IsPlaying);
        }

        if (turnData.IsConsumed && turnData.Item is RPGConsumed consumed)
        {
            if (consumed.WriteMessage)
            {
                _common.MessageBox.Write(new MessageInfo()
                {
                    text = $"* {character.Name} использует {consumed.Name}!",
                    closeWindow = true,
                });

                yield return new WaitWhile(() => _common.MessageBox.IsWriting);
                yield return new WaitForSeconds(.25f);
            }
            else
                yield return new WaitForSeconds(.5f);

            switch (consumed.Direction)
            {
                case UsabilityDirection.AllTeam:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, Data.TurnsData.Select(i => i.Character).ToArray()));
                    break;
                case UsabilityDirection.Teammate:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, turnData.CharacterBuffer));
                    break;
                case UsabilityDirection.AllEnemys:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, Data.Enemys.ToArray()));
                    break;
                case UsabilityDirection.Enemy:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, turnData.EnemyBuffer));
                    break;
                case UsabilityDirection.Any:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, turnData.EntityBuffer));
                    break;
                case UsabilityDirection.All:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, Data.TurnsData.Select(i => i.Character).ToArray()));

                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, Data.Enemys.ToArray()));
                    break;
            }
        }
    }
    private IEnumerator HandleFleeAction(BattleTurnData turnData, RPGCharacter character)
    {
        _battle.BattleAudio.PlaySound(Data.Flee);

        _battle.BattleAudio.PauseMusic();

        _common.MessageBox.Write(new MessageInfo()
        {
            text = $"* {character.Name} пытаеться сбежать<\\:>.<\\:>.<\\:>.",
            closeWindow = true,
        });

        yield return new WaitWhile(() => _common.MessageBox.IsWriting);

        yield return new WaitForSeconds(.5f);

        int totalEnemysAgility, totalCharactersAgility;

        totalCharactersAgility = Data.TurnsData.Select(i => i.Character.Agility).Sum();
        totalEnemysAgility = Data.Enemys.Select(i => i.Agility).Sum();

        int randNum = Random.Range(0, totalCharactersAgility + totalEnemysAgility);

        if (randNum - totalCharactersAgility < 0)
        {
            isFlee = true;

            InvokeFlee();
        }
        else
        {
            _battle.BattleAudio.UnPauseMusic();

            _common.MessageBox.Write(new MessageInfo()
            {
                text = $"* Но сбежать не удалось",
                closeWindow = true,
            });

            yield return new WaitWhile(() => _common.MessageBox.IsWriting);
        }
    }

    #endregion
}