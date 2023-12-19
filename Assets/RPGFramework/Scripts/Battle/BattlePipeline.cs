﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePipeline : MonoBehaviour
{
    /// <summary>
    /// Типы возможных менюшек в битве
    /// </summary>
    public enum ChoiceAction
    {
        Primary, InterationOrAbility, Interaction, Ability, Entity, Teammate, Enemy,
        Item, Defence
    }

    public BattleData Data => BattleManager.Data;
    public BattleChoiceManager Choice => BattleManager.instance.choice;
    public BattleUtility Utility => BattleManager.instance.utility;

    public bool MainIsWorking => main != null;

    public bool IsPlayerTurn { get; private set; } = false;
    public bool IsEnemyTurn { get; private set; } = false;

    // Стек для выборов
    private List<ChoiceAction> choiceActions = new List<ChoiceAction>();
    public ChoiceAction CurrentChoiceAction => choiceActions.Last();

    public BattleCharacterInfo CurrentChoiceCharacter => Data.Characters[currentCharacterChoiceIndex];

    // Переключатели запоминающие что нужно ли возвращать BGM и BGS
    private bool restoreBGM = false;
    private bool restoreBGS = false;

    private int currentCharacterChoiceIndex = 0;

    private Coroutine main = null;

    public void InvokeLose()
    {
        StopCoroutine(main);
        main = null;

        StartCoroutine(Lose());
    }

    public void InvokeBreak()
    {

    }

    public void InvokeMainPipeline()
    {
        if (!MainIsWorking)
            main = StartCoroutine(MainPipeline());
    }

    private IEnumerator MainPipeline()
    {
        /// SETUP PART +

        // Добавление персонажей в битву
        foreach (var item in GameManager.Instance.characterManager.characters)
        {
            // Добавление только тех кто может учавствовать
            if (item.ParticipateInBattle) 
                Data.Characters.Add(new BattleCharacterInfo(item));
        }

        // Затухание музыки
        if (restoreBGM = GameManager.Instance.gameAudio.BGMIsPlaying)
            GameManager.Instance.gameAudio.PauseBGM(0.2f);

        // Затухание фотовых звуков
        if (restoreBGS = GameManager.Instance.gameAudio.BGSIsPlaying)
            GameManager.Instance.gameAudio.PauseBGS();

        // Затимнение
        GameManager.Instance.loadingScreen.ActivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        // Включение контейнера битвы
        BattleManager.instance.SetActive(true);

        // Ждём fixedupdate
        yield return new WaitForFixedUpdate();

        // Инициализация character box-ов
        BattleManager.instance.characterBox.Initialize(Data.Characters.ToArray());
        // Перемещение их наврех или вниз
        BattleManager.instance.characterBox.ChangePosition(!Data.BattleInfo.EnemyStart);

        // Отключение первичного выбора
        Choice.PrimaryChoice.SetActive(false);
        // Отключение окна с описанием
        BattleManager.instance.description.SetActive(false);

        // Обнавление полоски концентрации
        BattleManager.instance.concentrationBar.UpdateValue();

        // Установка заднего фона
        BattleManager.instance.background.CreateBackground(Data.BattleInfo.Background);

        // Включение музыки битвы
        if (Data.BattleInfo.BattleMusic != null)
            BattleManager.instance.battleAudio.PlayMusic(Data.BattleInfo.BattleMusic, Data.BattleInfo.MusicVolume);

        /// SETUP PART -

        // Появление экрана
        GameManager.Instance.loadingScreen.DeactivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        // Старт врага
        bool enemyStart = Data.BattleInfo.EnemyStart;
        while (true)
        {
            // Если старт врага
            if (enemyStart)
            {
                yield return StartCoroutine(EnemyTurn());
                enemyStart = false;
            }

            // Запуск хода игрока
            yield return StartCoroutine(PlayerTurn());

            // Завершение битвы если врагов не осталось
            if (Data.Enemys.Count <= 0)
            {
                // Запуск победной курутины
                yield return StartCoroutine(Win());
                break;
            }

            // Запуск хода врага
            yield return StartCoroutine(EnemyTurn());
        }

        /// END BATTLE

        // Затимнение
        GameManager.Instance.loadingScreen.ActivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        // Очистка данных
        Data.Dispose();
        BattleManager.Utility.CleanupBattle();

        // Оключение контейнера битвы
        BattleManager.instance.SetActive(false);

        // Востоновление музыки
        if (restoreBGM)
            GameManager.Instance.gameAudio.ResumeBGM();
        // Востоновление фоновых звуков
        if (restoreBGS)
            GameManager.Instance.gameAudio.ResumeBGS();

        // Появление экрана
        GameManager.Instance.loadingScreen.DeactivatePart1();

        yield return new WaitWhile(() => GameManager.Instance.loadingScreen.BgIsFade);

        // Обнуление
        main = null;

        /// END BATTLE
    }

    private IEnumerator PlayerTurn()
    {
        IsPlayerTurn = true;

        // Обработка состояний
        foreach (var character in Data.Characters)
        {
            if (character.States.Count == 0)
                break;

            foreach (var state in character.States)
            {
                if (state.rpg.Event != null)
                {
                    state.rpg.Event.Invoke(this);

                    yield return new WaitWhile(() => state.rpg.Event.IsPlaying);
                }
            }

            int oldHeal = character.Entity.Heal;

            character.UpdateAllStates();

            CharacterBox box = BattleManager.instance.characterBox.GetBox(character);

            if (character.Entity.Heal != oldHeal)
            {
                BattleManager.Utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1.4f), oldHeal - character.Entity.Heal);
            }
        }

        // Очистка ходов
        choiceActions.Clear();

        Data.Characters.ForEach(i => i.CleanUp());

        currentCharacterChoiceIndex = 0;

        // Установление позиции character box-сов
        BattleManager.instance.characterBox.ChangePosition(true);
        // Активация первичного выбора
        Choice.PrimaryChoice.SetActive(true);

        int actionIndex = 0;

        // Установление иконки действия
        for (int i = 0; i < Data.Characters.Count; i++)
            BattleManager.instance.characterBox.Boxes[i].ChangeAct(BattleCharacterAction.None);

        /// ОБЩИЙ ЦИКЛ ВЫБОРА
        while (currentCharacterChoiceIndex < Data.Characters.Count)
        {
            BattleCharacterInfo currentCharacter = Data.Characters[currentCharacterChoiceIndex];

            // Если персонаж пал или пропускает ход или не может ничего делать в битве, то его надо пропустить
            if (currentCharacter.IsDead || currentCharacter.States.Any(i => i.rpg.SkipTurn) || !((RPGCharacter)currentCharacter.Entity).CanMoveInBattle)
            {
                currentCharacter.BattleAction = BattleCharacterAction.None;
                currentCharacterChoiceIndex++;
                continue;
            }
                
            // Установка первичного действия
            if (choiceActions.Count == 0)
            {
                choiceActions.Add(ChoiceAction.Primary);
            }
                

            switch (choiceActions.Last())
            {
                // Для первичного действия
                case ChoiceAction.Primary:
                    // Вкличение показа персонажа
                    BattleManager.instance.characterPreview.SetActive(true);
                    // Установка текущего персонажа на показ
                    BattleManager.instance.characterPreview.SetData(currentCharacter.Entity as RPGCharacter);

                    currentCharacter.CleanUp();

                    // Включить первичный выбор
                    Choice.InvokePrimaryChoice(actionIndex);

                    yield return new WaitWhile(() => Choice.IsChoicing);

                    // Если отмена то откат к предыдущему персонажу, либо игнор
                    if (Choice.IsPrimaryCanceled)
                    {
                        currentCharacterChoiceIndex = currentCharacterChoiceIndex == 0 ? 0 : currentCharacterChoiceIndex - 1;
                        BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.None);
                    } 
                    else
                    {
                        actionIndex = Choice.PrimaryCurrentIndex;

                        switch (actionIndex)
                        {
                            // Выбрана битва
                            case 0:
                                currentCharacter.BattleAction = BattleCharacterAction.Fight;

                                choiceActions.Add(ChoiceAction.Enemy);
                                break;
                            // Выбрано дейтсвие
                            case 1:
                                currentCharacter.BattleAction = BattleCharacterAction.Act;

                                choiceActions.Add(ChoiceAction.InterationOrAbility);
                                break;
                            // Выбраны вещи
                            case 2:
                                currentCharacter.BattleAction = BattleCharacterAction.Item;

                                choiceActions.Add(ChoiceAction.Item);
                                break;
                            // Выбрана обарона
                            case 3:
                                currentCharacter.BattleAction = BattleCharacterAction.Defence;

                                choiceActions.Add(ChoiceAction.Defence);
                                break;
                            // Неизветсное действие
                            default:
                                Debug.LogWarning("Unknown battle action!");
                                break;
                        }
                    }

                    BattleManager.instance.characterPreview.SetActive(false);

                    break;
                // Выбор между взаимодействием и способностью 
                case ChoiceAction.InterationOrAbility:
                    // Запуск выбора
                    BattleManager.instance.choice.InvokeChoiceAct();

                    yield return new WaitWhile(() => BattleManager.instance.choice.IsChoicing);

                    // Если отмена то откат
                    if (BattleManager.instance.choice.IsCanceled)
                        choiceActions.Remove(choiceActions.Last());
                    else
                    {
                        if ((int)Choice.CurrentItem.value == 0)
                        {
                            choiceActions.Add(ChoiceAction.Enemy);
                        }
                        else
                        {
                            choiceActions.Add(ChoiceAction.Ability);
                        }
                    }

                    // Очистка меню выбора
                    BattleManager.instance.choice.CleanUp();
                    break;
                // Выбор взаимодействия
                case ChoiceAction.Interaction:
                    BattleManager.instance.choice.InvokeChoiceInteraction();

                    yield return new WaitWhile(() => BattleManager.instance.choice.IsChoicing);

                    // Если отмена то откат
                    if (BattleManager.instance.choice.IsCanceled)
                        choiceActions.Remove(choiceActions.Last());
                    else
                    {
                        currentCharacter.InteractionAct = (RPGEnemy.EnemyAct)Choice.CurrentItem.value;

                        BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Act);

                        // Переключение на следующего персонажа
                        currentCharacterChoiceIndex++;

                        // Обнуляет все менюшки выбора
                        choiceActions.Clear();
                    }

                    Choice.CleanUp();
                    break;
                // Выбор способности
                case ChoiceAction.Ability:
                    Choice.InvokeChoiceAbility();

                    yield return new WaitWhile(() => Choice.IsChoicing);

                    // Если отмена то откат
                    if (Choice.IsCanceled)
                        choiceActions.Remove(choiceActions.Last());
                    else
                    {
                        BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Act);
                    }

                    BattleManager.instance.choice.CleanUp();
                    break;
                // Выбор сущности (Враг или персонаж)
                case ChoiceAction.Entity:
                    // Запуск выбора
                    BattleManager.instance.choice.InvokeChoiceEntity();

                    yield return new WaitWhile(() => BattleManager.instance.choice.IsChoicing);

                    // Если отмена то откат
                    if (BattleManager.instance.choice.IsCanceled)
                    {
                        if (currentCharacter.BattleAction == BattleCharacterAction.Item)
                            currentCharacter.Item = null;

                        choiceActions.Remove(choiceActions.Last());
                    }
                    else
                    {
                        currentCharacter.EntityBuffer = (BattleEntityInfo)BattleManager.instance.choice.CurrentItem.value;

                        if (currentCharacter.BattleAction == BattleCharacterAction.Act)
                        {
                            if (choiceActions[choiceActions.Count - 2] == ChoiceAction.Ability)
                            {
                                BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Act);

                                // Переключение на следующего персонажа
                                currentCharacterChoiceIndex++;

                                // Обнуляет все менюшки выбора
                                choiceActions.Clear();
                            }
                            else
                            {
                                choiceActions.Add(ChoiceAction.Interaction);
                            }
                        }
                        else
                        {
                            // Утанавливаем соотвествующую иконку действия
                            BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(currentCharacter.BattleAction);

                            // Переключение на следующего персонажа
                            currentCharacterChoiceIndex++;

                            // Обнуляет все менюшки выбора
                            choiceActions.Clear();
                        }
                    }

                    // Очистка меню выбора
                    BattleManager.instance.choice.CleanUp();
                    break;
                // Выбор персонажа
                case ChoiceAction.Teammate:
                    Choice.InvokeChoiceTeammate();

                    yield return new WaitWhile(() => Choice.IsChoicing);

                    if (Choice.IsCanceled)
                    {
                        if (currentCharacter.BattleAction == BattleCharacterAction.Item)
                            currentCharacter.Item = null;

                        choiceActions.Remove(choiceActions.Last());
                    }
                    else
                    {
                        currentCharacter.CharacterBuffer = (BattleCharacterInfo)Choice.CurrentItem.value;

                        if (currentCharacter.BattleAction == BattleCharacterAction.Item)
                        {
                            BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Item);

                            // Переключение на следующего персонажа
                            currentCharacterChoiceIndex++;

                            // Обнуляет все менюшки выбора
                            choiceActions.Clear();
                        }
                    }

                    Choice.CleanUp();
                    break;
                // Выбор врага
                case ChoiceAction.Enemy:
                    // Запуск выбора
                    BattleManager.instance.choice.InvokeChoiceEnemy();

                    yield return new WaitWhile(() => BattleManager.instance.choice.IsChoicing);      
                    
                    // Если отмена то откат
                    if (BattleManager.instance.choice.IsCanceled)
                    {
                        if (currentCharacter.BattleAction == BattleCharacterAction.Item)
                            currentCharacter.Item = null;

                        choiceActions.Remove(choiceActions.Last());
                    }
                    else
                    {
                        currentCharacter.EnemyBuffer = (BattleEnemyInfo)BattleManager.instance.choice.CurrentItem.value;

                        // Если это атака
                        if (currentCharacter.BattleAction == BattleCharacterAction.Act)
                        {
                            if (choiceActions[choiceActions.Count - 2] == ChoiceAction.Ability)
                            {
                                BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Act);

                                // Переключение на следующего персонажа
                                currentCharacterChoiceIndex++;

                                // Обнуляет все менюшки выбора
                                choiceActions.Clear();
                            }
                            else
                            {
                                choiceActions.Add(ChoiceAction.Interaction);
                            }
                        }
                        else
                        {
                            // Утанавливаем соотвествующую иконку действия
                            BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(currentCharacter.BattleAction);

                            // Переключение на следующего персонажа
                            currentCharacterChoiceIndex++;

                            // Обнуляет все менюшки выбора
                            choiceActions.Clear();
                        }
                    }

                    // Очистка меню выбора
                    BattleManager.instance.choice.CleanUp();
                    break;
                // Выбор пердмета
                case ChoiceAction.Item:
                    // Заупуск выбра
                    BattleManager.instance.choice.InvokeChoiceItem();

                    yield return new WaitWhile(() => BattleManager.instance.choice.IsChoicing);

                    // Если отмена то откат 
                    if (BattleManager.instance.choice.IsCanceled)
                        choiceActions.Remove(choiceActions.Last());
                    else
                    {
                        // Запоминаем предмет
                        currentCharacter.Item = BattleManager.instance.choice.CurrentItem.value as RPGCollectable;

                        // Этот предмет является потребляемым?
                        currentCharacter.IsConsumed = currentCharacter.Item is RPGConsumed;

                        if (currentCharacter.Item is RPGConsumed consumed)
                        {
                            switch (consumed.Direction)
                            {
                                case RPGConsumed.ConsumingDirection.AllEnemys:
                                case RPGConsumed.ConsumingDirection.AllTeam:
                                    BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Item);

                                    currentCharacterChoiceIndex++;

                                    choiceActions.Clear();
                                    break;
                                case RPGConsumed.ConsumingDirection.Teammate:
                                    choiceActions.Add(ChoiceAction.Teammate);
                                    break;
                                case RPGConsumed.ConsumingDirection.Enemy:
                                    choiceActions.Add(ChoiceAction.Enemy);
                                    break;
                                case RPGConsumed.ConsumingDirection.Any:
                                    choiceActions.Add(ChoiceAction.Entity);
                                    break;
                            }
                        }
                        else
                        {
                            BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Item);

                            currentCharacterChoiceIndex++;

                            choiceActions.Clear();
                        }
                    }

                    // Очистка меню выбора
                    BattleManager.instance.choice.CleanUp();
                    break;
                // Выбор защиты или бегства
                case ChoiceAction.Defence:
                    // Запуск выбора
                    BattleManager.instance.choice.InvokeChoiceDefence();

                    yield return new WaitWhile(() => BattleManager.instance.choice.IsChoicing);

                    // Если отмена то откат
                    if (BattleManager.instance.choice.IsCanceled)
                        choiceActions.Remove(choiceActions.Last());
                    else
                    {
                        // Бество это или защита
                        currentCharacter.IsFlee = (int)BattleManager.instance.choice.CurrentItem.value == 1;
                        currentCharacter.IsDefence = (int)BattleManager.instance.choice.CurrentItem.value == 0;

                        // Утанавливаем соотвествующую иконку действия
                        BattleManager.instance.characterBox.Boxes[currentCharacterChoiceIndex].ChangeAct(BattleCharacterAction.Defence);

                        // Переключение на следующего персонажа
                        currentCharacterChoiceIndex++;

                        // Обнуляет все менюшки выбора
                        choiceActions.Clear();
                    }

                    // Очистка меню выбора
                    BattleManager.instance.choice.CleanUp();
                    break;
                default:
                    Debug.LogError("Unknown battle menu!");
                    break;
            }

            IsPlayerTurn = false;

            yield return null;
        }

        // Изменение позиции character box-сов
        BattleManager.instance.characterBox.ChangePosition(false);
        // Выключение первичного выбора
        Choice.PrimaryChoice.SetActive(false);

        /// ЦИКЛ ПОСЛЕДСВИЙ ВЫБОРА
        foreach (var characterInfo in Data.Characters)
        {
            RPGCharacter character = characterInfo.Entity as RPGCharacter;

            // Если персонаж пал или пропускает ход или не может ничего делать в битве, то его надо пропустить 
            if (characterInfo.IsDead || characterInfo.States.Any(i => i.rpg.SkipTurn) || !character.CanMoveInBattle)
                continue;

            yield return new WaitForSeconds(0.5f);

            switch (characterInfo.BattleAction)
            {
                // Если персонаж атакует
                case BattleCharacterAction.Fight:
                    {
                        // Запуск QTE атаки
                        BattleManager.instance.attackQTE.InvokeQTE((AttackQTEManager.Positions)Data.Characters.IndexOf(characterInfo));

                        yield return new WaitWhile(() => BattleManager.instance.attackQTE.QTE.IsWorking);

                        // Эффект удара
                        AttackEffect effect;

                        // Получение эффекта удара. Если его нет, то использовать стандартный
                        effect = character.WeaponSlot == null ? Data.DefaultEffect : character.WeaponSlot.Effect;

                        // Расчёт позици эффекта удара
                        Vector2 attackPos = BattleManager.instance.enemyModels.GetModel(characterInfo.EnemyBuffer).AttackGlobalPoint;

                        // Получение экземпляра эффекта удара
                        AttackEffect neffect = BattleManager.Utility.SpawnAttackEffect(effect, attackPos);

                        // Запуск анимации удара
                        neffect.Invoke();

                        yield return new WaitWhile(() => neffect.IsAnimating);

                        yield return new WaitForSeconds(1f);

                        // Удаление объекта эффекта удара
                        Destroy(neffect.gameObject);

                        // Убрать QTE
                        BattleManager.instance.attackQTE.DropQTE();

                        // Нанесение урона врагу
                        BattleManager.Utility.DamageEnemy(characterInfo, characterInfo.EnemyBuffer, BattleManager.instance.attackQTE.QTE.DamageFactor);
                    }
                    break;
                // Если персонаж действует
                case BattleCharacterAction.Act:
                    if (characterInfo.IsAbility)
                    {

                    }
                    else
                    {
                        if (characterInfo.InteractionAct.Name == "Check")
                        {

                        }
                        else
                        {

                        }
                    }
                    break;
                // Если персонаж использует предмет
                case BattleCharacterAction.Item:
                    {
                        characterInfo.Item.InvokeEvent();

                        if (characterInfo.Item.Event != null)
                            yield return new WaitWhile(() => characterInfo.Item.Event.IsPlaying);

                        if (characterInfo.IsConsumed && characterInfo.Item is RPGConsumed consumed)
                        {
                            if (consumed.WriteMessage)
                            {
                                CommonManager.instance.messageBox.Write(new MessageInfo()
                                {
                                    text = $"* {characterInfo.Entity.Name} использует {consumed.Name}!",
                                    closeWindow = true,
                                });

                                yield return new WaitWhile(() => CommonManager.instance.messageBox.IsWriting);
                                yield return new WaitForSeconds(.25f);
                            }
                            else
                                yield return new WaitForSeconds(.5f);

                            switch (consumed.Direction)
                            {
                                case RPGConsumed.ConsumingDirection.AllTeam:
                                    Utility.ConsumeItem(consumed, Data.Characters.ToArray());
                                    break;
                                case RPGConsumed.ConsumingDirection.Teammate:
                                    Utility.ConsumeItem(consumed, characterInfo.CharacterBuffer);
                                    break;
                                case RPGConsumed.ConsumingDirection.AllEnemys:
                                    Utility.ConsumeItem(consumed, Data.Enemys.ToArray());
                                    break;
                                case RPGConsumed.ConsumingDirection.Enemy:
                                    Utility.ConsumeItem(consumed, characterInfo.EnemyBuffer);
                                    break;
                                case RPGConsumed.ConsumingDirection.Any:
                                    Utility.ConsumeItem(consumed, characterInfo.EntityBuffer);
                                    break;
                                case RPGConsumed.ConsumingDirection.All:
                                    Utility.ConsumeItem(consumed, Data.Characters.ToArray());

                                    yield return new WaitForSeconds(.25f);

                                    Utility.ConsumeItem(consumed, Data.Enemys.ToArray());
                                    break;
                            }
                        }
                    }
                    break;
                // Если персонаж обораняется
                case BattleCharacterAction.Defence:
                    if (characterInfo.IsDefence)
                    {
                        BattleManager.Utility.AddConcetration(Data.AdditionConcentrationInDefence);
                    }
                    else if (characterInfo.IsFlee)
                    {
                        if (Data.BattleInfo.CanFlee)
                        {
                            /// TODO:
                        }
                        else
                        {
                            /// TODO:
                        }
                    }
                    break;
                case BattleCharacterAction.None:
                default:
                    break;
            }

            if (Data.Enemys.Any(i => i.Heal <= 0))
            {
                BattleEnemyInfo[] deads = Data.Enemys.Where(i => i.Heal <= 0).ToArray();

                for (int i = 0; i < deads.Length; i++)
                {
                    yield return new WaitWhile(() => BattleManager.instance.enemyModels.GetModel(deads[i]).IsAnimating);

                    BattleManager.Utility.RemoveEnemy(deads[i]);
                }
            }

            if (Data.Enemys.Count == 0)
                break;
        }
    }

    private IEnumerator EnemyTurn()
    {
        IsEnemyTurn = true;

        yield return new WaitForSeconds(0.5f);

        List<RPGAttackPattern> patterns = new List<RPGAttackPattern>();
        List<BattleCharacterInfo> targets = new List<BattleCharacterInfo>();

        foreach (var enemyinfo in Data.Enemys)
        {
            RPGEnemy enemy = enemyinfo.Entity as RPGEnemy;

            if (enemyinfo.States.Any(i => i.rpg.SkipTurn))
                continue;

            RPGAttackPattern pattern = enemy.Patterns[UnityEngine.Random.Range(0, enemy.Patterns.Count)];
            pattern.enemy = (RPGEnemy)enemyinfo.Entity;

            patterns.Add(pattern);
        }

        foreach (var pattern in patterns)
            BattleManager.instance.pattern.AddPattern(pattern);

        int chars = UnityEngine.Random.Range(1, Data.Characters.Where(i => !i.IsDead).Count() + 1);

        for (int i = 0; i < chars; i++)
        {
            BattleCharacterInfo characterInfo = Data.Characters[UnityEngine.Random.Range(0, Data.Characters.Count)];

            if (characterInfo.IsTarget || characterInfo.IsDead)
            {
                i--;
                continue;
            }

            characterInfo.IsTarget = true;

            BattleManager.instance.characterBox.GetBox(characterInfo).MarkTarget(true);

            targets.Add(characterInfo);
        }

        BattleManager.instance.player.SetActive(true);
        BattleManager.instance.battleField.SetActive(true);

        BattleManager.instance.pattern.Invoke(patterns.Count <= 1);

        yield return new WaitWhile(() => BattleManager.instance.pattern.IsAttack);

        foreach (var item in targets)
        {
            item.IsTarget = false;
            BattleManager.instance.characterBox.GetBox(item).MarkTarget(false);
        }

        BattleManager.instance.player.SetActive(false);
        BattleManager.instance.battleField.SetActive(false);

        IsPlayerTurn = false;
    }

    private IEnumerator Lose()
    {
        if (Data.BattleInfo.CanLose)
            SceneManager.LoadScene("GameOver");

        BattleManager.instance.battleAudio.StopMusic();
        BattleManager.instance.battleAudio.PlaySound(Data.Lose);

        BattleManager.instance.characterBox.SetActive(false);

        BattleManager.instance.player.SetActive(false);
        BattleManager.instance.battleField.SetActive(false);

        CommonManager.instance.messageBox.Write(new MessageInfo()
        {
            text = "* Ваша команда проебала!",
            closeWindow = true,
        });

        yield return new WaitWhile(() => CommonManager.instance.messageBox.IsWriting);

#if UNITY_EDITOR
        Debug.Break();
#endif

        Application.Quit();
    }

    private IEnumerator Break()
    {
        yield return null;
    }

    private IEnumerator Win()
    {
        BattleManager.instance.battleAudio.StopMusic();
        BattleManager.instance.battleAudio.PlaySound(Data.Win);

        BattleManager.instance.characterBox.SetActive(false);

        CommonManager.instance.messageBox.Write(new MessageInfo()
        {
            text = "* Ваша команда одержала победу<!>",
            closeWindow = true,
            wait = true
        });

        yield return new WaitWhile(() => CommonManager.instance.messageBox.IsWriting);
    }
}