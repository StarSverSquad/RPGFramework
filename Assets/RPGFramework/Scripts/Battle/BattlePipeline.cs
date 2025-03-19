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

    private IEnumerator MainPipeline()
    {
        loseKey = false; winKey = false; fleeKey = false; breakKey = false;

        IsFlee = false; IsWin = false; IsLose = false;

        TurnCounter = 0;

        yield return _battle.StartCoroutine(BattleEnter());

        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnBattleStart));

        if (Data.BattleInfo.EnemyStart)
        {
            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnEnemyTurn, true));

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.EveryEnemyTurn));

            yield return _battle.StartCoroutine(EnemyTurn());

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessCharacterHeal));

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

            yield return _battle.StartCoroutine(UpdateEntitysStates());

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnPlayerTurn, true));

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.EveryPlayerTurn));

            if (!AllKeysFalse)
                break;

            yield return _battle.StartCoroutine(PlayerTurn());

            if (Data.Enemys.Count == 0)
                InvokeWin();

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnEnemyTurn, true));

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.EveryEnemyTurn));

            if (!AllKeysFalse)
                break;

            yield return _battle.StartCoroutine(EnemyTurn());

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessCharacterHeal));

            TurnCounter++;
        }

        UI.Concentration.Hide();
        UI.CharacterSide.Hide();
        UI.PlayerTurnSide.Hide();
        UI.CharacterBox.Hide();

        if (fleeKey)
        {
            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnFlee));

            yield return _battle.StartCoroutine(Flee());
        }

        if (winKey)
        {
            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnWin));

            yield return _battle.StartCoroutine(Win());
        }

        if (loseKey)
        {
            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLose));

            yield return _battle.StartCoroutine(Lose());
        }

        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnBattleEnd));

        main = null;

        yield return _battle.StartCoroutine(BattleExit());
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

        // Очистка ходов
        choiceActions.Clear();

        Data.TurnsData.ForEach(i => i.CleanUp());

        CurrentTurnDataIndex = 0;

        UI.CharacterSide.Show();
        UI.PlayerTurnSide.Show();
        UI.Concentration.Show();
        UI.CharacterQuery.Show();

        // Активация первичного выбора
        Choice.PrimaryChoice.SetActive(true);

        int actionIndex = 0;
        bool isCancelChoice = false;

        // Установление иконки действия
        for (int i = 0; i < Data.TurnsData.Count; i++)
            _battle.UI.CharacterBox.Boxes[i].ChangeAct(TurnAction.None);

        /// ОБЩИЙ ЦИКЛ ВЫБОРА
        while (CurrentTurnDataIndex < Data.TurnsData.Count)
        {
            BattleTurnData currenTurnData = Data.TurnsData[CurrentTurnDataIndex];

            // Если персонаж пал или пропускает ход или не может ничего делать в битве, то его надо пропустить
            if (currenTurnData.IsDead || currenTurnData.Character.States.Any(i => i.SkipTurn) || !currenTurnData.Character.CanMoveInBattle)
            {
                if (isCancelChoice && CurrentTurnDataIndex != 0)
                {
                    CurrentTurnDataIndex--;

                    //if (currentTurnDataIndex > 0)
                        UI.CharacterQuery.PreviewPosition();
                }
                else
                {
                    currenTurnData.BattleAction = TurnAction.None;
                    CurrentTurnDataIndex++;

                    UI.CharacterQuery.NextPosition();

                    isCancelChoice = false;

                }
                continue;
            }
            isCancelChoice = false;

            // Установка первичного действия
            if (choiceActions.Count == 0)
            {
                choiceActions.Add(ChoiceAction.Primary);
            }
             
            if (currenTurnData.ReservedConcentration != 0)
            {
                Utility.AddConcetration(-currenTurnData.ReservedConcentration);
                currenTurnData.ReservedConcentration = 0;
            }

            switch (choiceActions.Last())
            {
                // Для первичного действия
                case ChoiceAction.Primary:

                    UI.CharacterSide.Setup(currenTurnData.Character);

                    currenTurnData.CleanUp();

                    // Включить первичный выбор
                    Choice.InvokePrimaryChoice(actionIndex);

                    yield return new WaitWhile(() => Choice.IsChoicing);

                    // Если отмена то откат к предыдущему персонажу, либо игнор
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
                                //currenTurnData.BattleAction = TurnAction.Attack;

                                choiceActions.Add(ChoiceAction.Battle);
                                break;
                            // Выбрано дейтсвие
                            case 1:
                                //currenTurnData.BattleAction = TurnAction.Special;

                                choiceActions.Add(ChoiceAction.Special);
                                break;
                            // Выбраны вещи
                            case 2:
                                currenTurnData.BattleAction = TurnAction.Item;

                                choiceActions.Add(ChoiceAction.Item);
                                break;
                            // Выбрана обарона
                            case 3:
                                choiceActions.Add(ChoiceAction.Flee);
                                break;
                            // Неизветсное действие
                            default:
                                Debug.LogWarning("Unknown battle action!");
                                break;
                        }
                    }

                    //Battle.characterPreview.SetActive(false);

                    break;
                // Выбор между взаимодействием и способностью 
                case ChoiceAction.Special:
                    // Запуск выбора
                    _battle.Choice.InvokeChoiceAct();

                    yield return new WaitWhile(() => _battle.Choice.IsChoicing);

                    // Если отмена то откат
                    if (_battle.Choice.IsCanceled)
                        PreviewAction();
                    else
                    {
                        if ((int)Choice.CurrentItem.Value == 0)
                        {
                            currenTurnData.BattleAction = TurnAction.Act;
                            choiceActions.Add(ChoiceAction.Enemy);
                        }
                        else
                        {
                            currenTurnData.BattleAction = TurnAction.Ability;
                            choiceActions.Add(ChoiceAction.Ability);
                        }
                    }

                    // Очистка меню выбора
                    _battle.Choice.CleanUp();
                    break;
                // Выбор взаимодействия
                case ChoiceAction.Act:
                    _battle.Choice.InvokeChoiceInteraction();

                    yield return new WaitWhile(() => _battle.Choice.IsChoicing);

                    // Если отмена то откат
                    if (_battle.Choice.IsCanceled)
                    {
                        currenTurnData.InteractionAct = RPGEnemy.EnemyAct.NullAct;

                        currenTurnData.EnemyBuffer = null;

                        PreviewAction();
                    }
                    else
                    {
                        currenTurnData.InteractionAct = (RPGEnemy.EnemyAct)Choice.CurrentItem.Value;

                        NextCharacter();
                    }

                    Choice.CleanUp();
                    break;
                // Выбор способности
                case ChoiceAction.Ability:
                    Choice.InvokeChoiceAbility();

                    yield return new WaitWhile(() => Choice.IsChoicing);

                    // Если отмена то откат
                    if (Choice.IsCanceled)
                    {
                        currenTurnData.Ability = null;

                        PreviewAction();
                    }
                    else
                    {
                        currenTurnData.Ability = (RPGAbility)Choice.CurrentItem.Value;

                        switch (currenTurnData.Ability.Direction)
                        {
                            case UsabilityDirection.All:
                            case UsabilityDirection.AllEnemys:
                            case UsabilityDirection.AllTeam:
                                currenTurnData.ReservedConcentration = -currenTurnData.Ability.ConcentrationCost;
                                Utility.AddConcetration(-currenTurnData.Ability.ConcentrationCost);

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
                    break;
                // Выбор сущности (Враг или персонаж)
                case ChoiceAction.Entity:
                    // Запуск выбора
                    _battle.Choice.InvokeChoiceEntity();

                    yield return new WaitWhile(() => _battle.Choice.IsChoicing);

                    // Если отмена то откат
                    if (_battle.Choice.IsCanceled)
                    {
                        if (currenTurnData.BattleAction == TurnAction.Item)
                            currenTurnData.Item = null;

                        PreviewAction();
                    }
                    else
                    {
                        currenTurnData.EntityBuffer = (RPGEntity)_battle.Choice.CurrentItem.Value;

                        switch (currenTurnData.BattleAction)
                        {
                            case TurnAction.Act:
                                choiceActions.Add(ChoiceAction.Act);

                                break;
                            case TurnAction.Ability:
                                currenTurnData.ReservedConcentration = -currenTurnData.Ability.ConcentrationCost;
                                Utility.AddConcetration(-currenTurnData.Ability.ConcentrationCost);

                                NextCharacter();
                                break;
                            default:
                                NextCharacter();
                                break;
                        }
                    }

                    // Очистка меню выбора
                    _battle.Choice.CleanUp();
                    break;
                // Выбор персонажа
                case ChoiceAction.Teammate:
                    Choice.InvokeChoiceTeammate();

                    yield return new WaitWhile(() => Choice.IsChoicing);

                    if (Choice.IsCanceled)
                    {
                        if (currenTurnData.BattleAction == TurnAction.Item)
                            currenTurnData.Item = null;

                        PreviewAction();
                    }
                    else
                    {
                        currenTurnData.CharacterBuffer = (RPGCharacter)Choice.CurrentItem.Value;

                        switch (currenTurnData.BattleAction)
                        {
                            case TurnAction.Ability:
                                currenTurnData.ReservedConcentration = -currenTurnData.Ability.ConcentrationCost;
                                Utility.AddConcetration(-currenTurnData.Ability.ConcentrationCost);
                                break;
                        }

                        NextCharacter();
                    }

                    Choice.CleanUp();
                    break;
                // Выбор врага
                case ChoiceAction.Enemy:
                    // Запуск выбора
                    _battle.Choice.InvokeChoiceEnemy();

                    yield return new WaitWhile(() => _battle.Choice.IsChoicing);      
                    
                    // Если отмена то откат
                    if (_battle.Choice.IsCanceled)
                    {
                        if (currenTurnData.BattleAction == TurnAction.Item)
                            currenTurnData.Item = null;

                        currenTurnData.EnemyBuffer = null;

                        PreviewAction();
                    }
                    else
                    {
                        currenTurnData.EnemyBuffer = (RPGEnemy)Choice.CurrentItem.Value;

                        switch (currenTurnData.BattleAction)
                        {
                            case TurnAction.Act:
                                choiceActions.Add(ChoiceAction.Act);

                                break;
                            case TurnAction.Ability:
                                currenTurnData.ReservedConcentration = -currenTurnData.Ability.ConcentrationCost;
                                Utility.AddConcetration(-currenTurnData.Ability.ConcentrationCost);

                                NextCharacter();
                                break;
                            default:
                                NextCharacter();
                                break;
                        }
                    }

                    // Очистка меню выбора
                    _battle.Choice.CleanUp();
                    break;
                // Выбор пердмета
                case ChoiceAction.Item:
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
                            // Запоминаем предмет
                            currenTurnData.Item = _battle.Choice.CurrentItem.Value as RPGCollectable;

                            // Этот предмет является потребляемым?
                            currenTurnData.IsConsumed = currenTurnData.Item is RPGConsumed;

                            if (currenTurnData.Item is RPGConsumed consumed)
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

                    break;
                // Выбор бегства
                case ChoiceAction.Flee:
                    currenTurnData.BattleAction = TurnAction.Flee;
                    NextCharacter();
                    break;
                // Выбор атаки или защиты
                case ChoiceAction.Battle:
                    _battle.Choice.InvokeChoiceBattle();

                    yield return new WaitWhile(() => _battle.Choice.IsChoicing);

                    if (_battle.Choice.IsCanceled)
                        PreviewAction();
                    else
                    {
                        int result = (int)_battle.Choice.CurrentItem.Value;

                        if (result == 0)
                        {
                            currenTurnData.BattleAction = TurnAction.Attack;

                            choiceActions.Add(ChoiceAction.Enemy);
                        }
                        else if (result == 1)
                        {
                            Utility.AddConcetration(Data.AdditionConcentrationOnDefence);
                            currenTurnData.ReservedConcentration = Data.AdditionConcentrationOnDefence;
                            currenTurnData.BattleAction = TurnAction.Defence;

                            NextCharacter();
                        }
                    }

                    _battle.Choice.CleanUp();
                break;
            }

            yield return null;
        }

        UI.CharacterSide.Hide();
        UI.PlayerTurnSide.Hide();
        UI.CharacterQuery.Hide();
        UI.Concentration.NearWindow();

        yield return new WaitForSeconds(.5f);

        UI.CharacterBox.Show();

        yield return new WaitForSeconds(UI.CharacterBox.TraslateContainerTime);

        // Выключение первичного выбора
        Choice.PrimaryChoice.SetActive(false);

        /// ЦИКЛ ПОСЛЕДСВИЙ ВЫБОРА
        for (int charIndex = 0; charIndex < Data.TurnsData.Count; charIndex++)
        {
            var turnData = Data.TurnsData[charIndex];

            bool flee = false;

            RPGCharacter currentCharacter = turnData.Character;

            // Если персонаж пал или пропускает ход или не может ничего делать в битве, то его надо пропустить 
            if (turnData.IsDead || currentCharacter.States.Any(i => i.SkipTurn) || !currentCharacter.CanMoveInBattle)
                continue;

            UI.CharacterBox.FocusBox(currentCharacter);

            yield return new WaitForSeconds(UI.CharacterBox.TraslateBoxTime);

            turnData.ReservedConcentration = 0;

            switch (turnData.BattleAction)
            {
                case TurnAction.Attack:
                    {
                        if (!Data.Enemys.Contains(turnData.EnemyBuffer))
                        {
                            if (Data.Enemys.Count != 0)
                                turnData.EnemyBuffer = Data.Enemys[0];
                            else
                                continue;
                        }

                        // Запуск QTE атаки
                        _battle.AttackQTE.Show();

                        if (!_battle.AttackQTE.IsShowed)
                            yield return new WaitForSeconds(1f);

                        _battle.AttackQTE.Invoke();

                        yield return new WaitWhile(() => _battle.AttackQTE.QTE.IsWorking);

                        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.BeforeHit, false, turnData.EnemyBuffer.Tag));

                        BattleAttackEffect effect = currentCharacter.WeaponSlot == null ? Data.DefaultEffect : currentCharacter.WeaponSlot.VisualEffect;

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

                        // Убрать QTE
                        _battle.AttackQTE.Hide();

                        // Нанесение урона врагу
                        BattleManager.BattleUtility.DamageEnemy(currentCharacter, turnData.EnemyBuffer, _battle.AttackQTE.QTE.DamageFactor);

                        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.AfterHit, false, turnData.EnemyBuffer.Tag));

                        yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessEnemyHeal, false, turnData.EnemyBuffer.Tag));
                    }
                    break;
                case TurnAction.Act:
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
                    break;
                case TurnAction.Ability:
                    currentCharacter.Mana -= turnData.Ability.ManaCost;

                    if (turnData.Ability.StartEvent != null)
                    {
                        turnData.Ability.StartEvent.Invoke(_battle);

                        yield return new WaitWhile(() => turnData.Ability.StartEvent.IsPlaying);
                    }

                    switch (turnData.Ability.Direction)
                    {
                        case UsabilityDirection.AllTeam:
                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, Data.TurnsData.Select(i => i.Character).ToArray()));
                            break;
                        case UsabilityDirection.Teammate:
                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, turnData.CharacterBuffer));
                            break;
                        case UsabilityDirection.AllEnemys:
                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, Data.Enemys.ToArray()));
                            break;
                        case UsabilityDirection.Enemy:
                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, turnData.EnemyBuffer));
                            break;
                        case UsabilityDirection.Any:
                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, turnData.EntityBuffer));
                            break;
                        case UsabilityDirection.All:
                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, Data.TurnsData.Select(i => i.Character).ToArray()));

                            yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, currentCharacter, Data.Enemys.ToArray()));
                            break;
                    }

                    if (turnData.Ability.EndEvent != null)
                    {
                        turnData.Ability.EndEvent.Invoke(_battle);

                        yield return new WaitWhile(() => turnData.Ability.EndEvent.IsPlaying);
                    }
                    break;
                case TurnAction.Item:
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
                                    text = $"* {currentCharacter.Name} использует {consumed.Name}!",
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
                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, Data.TurnsData.Select(i => i.Character).ToArray()));
                                    break;
                                case UsabilityDirection.Teammate:
                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, turnData.CharacterBuffer));
                                    break;
                                case UsabilityDirection.AllEnemys:
                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, Data.Enemys.ToArray()));
                                    break;
                                case UsabilityDirection.Enemy:
                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, turnData.EnemyBuffer));
                                    break;
                                case UsabilityDirection.Any:
                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, turnData.EntityBuffer));
                                    break;
                                case UsabilityDirection.All:
                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, Data.TurnsData.Select(i => i.Character).ToArray()));

                                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, currentCharacter, Data.Enemys.ToArray()));
                                    break;
                            }
                        }
                    }
                    break;
                case TurnAction.Flee:
                    _battle.BattleAudio.PlaySound(Data.Flee);

                    _battle.BattleAudio.PauseMusic();

                    _common.MessageBox.Write(new MessageInfo()
                    {
                        text = $"* {currentCharacter.Name} пытаеться сбежать<\\:>.<\\:>.<\\:>.",
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
                        flee = true;

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
                    break;
                case TurnAction.Defence:
                case TurnAction.None:
                default:
                    break;
            }

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

            if (Data.Enemys.Count == 0 || flee)
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
}