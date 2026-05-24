using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPGF.RPG;
using RPGF.Shared;
using RPGF.Battle.UI;
using RPGF.Core.Localization;
using RPGF.Domain.DI;
using RPGF.Core.Services;
using RPGF.Core.Battle.Enums;
using RPGF.Core.Battle;
using RPGF.Battle.Choice;

namespace RPGF.Battle
{
    public class BattlePipeline : ISupportDI
    {
        public enum ChoiceAction
        {
            Primary, Special, Act, Ability, Entity, Teammate, Enemy,
            Item, Defence
        }

        [Inject]
        private readonly BattleManager _battle;
        [Inject]
        private readonly SharedManager _shared;
        [Inject]
        private readonly DependencyInjection _di;
        [Inject]
        private readonly InvokeUsableEventService _invokeUsableEvent;

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

        public BattleData Data => _battle.Data;
        public BattleConfig Config => _battle.Config;
        public LocalizationService Localization => GlobalManager.Instance.Localization;
        public BattleChoiceManager Choice => _battle.Choice;
        public BattleUtility Utility => _battle.Utility;
        public BattleVisualTransmitionManager VisualTransmition => _battle.VisualTransmition;
        public BattleUIManager UI => _battle.UI;

        public bool MainIsWorking => main != null;

        public ChoiceAction CurrentChoiceAction => choiceActions.Last();

        public BattleTurnData CurrentTurnData => Data.TurnsData[CurrentTurnDataIndex];

        private bool AllKeysFalse => !loseKey && !winKey && !fleeKey && !breakKey;

        #endregion

        public BattlePipeline(BattleManager battle, SharedManager common)
        {
            _battle = battle;
            _shared = common;

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
        private void PreviewCharacter()
        {
            _battle.UI.CharacterBox.Boxes[CurrentTurnDataIndex].ChangeAct(TurnAction.None);

            CurrentTurnDataIndex--;

            UI.CharacterQuery.PreviewPosition();
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

            yield return BattleExit();

            main = null;
        }

        private IEnumerator BattleEnter()
        {
            foreach (var character in GlobalManager.Instance.Character.Characters)
            {
                if (character.ParticipateInBattle)
                    Data.TurnsData.Add(new BattleTurnData(character));
            }

            if (Data.BattleInfo.StopGlobalMusic)
                GlobalManager.Instance.GameAudio.PauseBGM(0.2f);

            if (Data.BattleInfo.StopGlobalMusic)
                GlobalManager.Instance.GameAudio.PauseBGS();

            VisualTransmition.InitializeEffect(Data.BattleInfo.BattleEnterEffect);

            yield return VisualTransmition.InvokePartOne();

            _battle.SetActive(true);

            yield return new WaitForFixedUpdate();

            _battle.UI.CharacterBox.Initialize(Data.TurnsData.Select(i => i.Character).ToArray());

            Choice.PrimaryChoice.SetActive(false);

            _battle.UI.Description.SetActive(false);

            _battle.UI.Concentration.SetConcentration(0);

            _battle.BattleField.transform.position = (Vector2)Camera.main.transform.position;

            _battle.Background.CreateBackground(Data.BattleInfo.Background);

            if (Data.BattleInfo.BattleMusic != null)
                _battle.BattleAudio.PlayMusic(Data.BattleInfo.BattleMusic, Data.BattleInfo.MusicVolume);

            yield return VisualTransmition.InvokePartTwo();

            VisualTransmition.DisposeEffect();

            if (Data.BattleInfo.ShowStartMessage)
            {
                string numeriticDifText = Data.BattleInfo.enemySquad.Enemies.Count > 1 ? Localization.GetLocale("SYS_BATTLE_ENCOUNTER_MULTI") : Localization.GetLocale("SYS_BATTLE_ENCOUNTER_SIGNLE");

                _shared.MessageDialog.Write(new MessageBoxInfo()
                {
                    text = $"* {Data.BattleInfo.enemySquad.Name} {numeriticDifText}!",
                    closeWindow = true
                });

                yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);
            }
        }

        private IEnumerator BattleExit()
        {
            VisualTransmition.InitializeEffect(Data.BattleInfo.BattleExitEffect);

            yield return _battle.StartCoroutine(VisualTransmition.InvokePartOne());

            foreach (var item in Data.TurnsData)
            {
                if (item.Character.Heal <= 0)
                    item.Character.Heal = 1;

                item.Character.RemoveNonBattleStates();
            }


            if (Data.BattleInfo.StopGlobalMusic)
                GlobalManager.Instance.GameAudio.ResumeBGM();

            if (Data.BattleInfo.StopGlobalMusic)
                GlobalManager.Instance.GameAudio.ResumeBGS();

            Data.Dispose();

            _battle.SetActive(false);

            _battle.UI.CharacterBox.Dispose();

            _battle.EnemyModels.Dispose();

            _battle.UI.CharacterQuery.Dispose();

            yield return _battle.StartCoroutine(VisualTransmition.InvokePartTwo());

            VisualTransmition.DisposeEffect();
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

            for (int i = 0; i < Data.TurnsData.Count; i++)
                _battle.UI.CharacterBox.Boxes[i].ChangeAct(TurnAction.None);

            while (CurrentTurnDataIndex < Data.TurnsData.Count)
            {
                BattleTurnData currentTurnData = Data.TurnsData[CurrentTurnDataIndex];

                if (ShouldSkipTurn(currentTurnData))
                    continue;

                isCancelChoice = false;

                if (choiceActions.Count == 0)
                    choiceActions.Add(ChoiceAction.Primary);

                if (choiceActions.Last() == ChoiceAction.Primary)
                    _battle.SpashWriter.WriteSpash();
                else
                    _battle.SpashWriter.Dispose();

                if (currentTurnData.ReservedConcentration != 0)
                {
                    Utility.AddConcetration(-currentTurnData.ReservedConcentration);
                    currentTurnData.ReservedConcentration = 0;
                }

                yield return HandleChoiceAction(currentTurnData);
            }

            _battle.SpashWriter.Dispose();

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
                        yield return new WaitWhile(() => _battle.EnemyModels.GetModel(deads[i]).IsAnyEffectPlaying);

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
                @event.Event.Invoke(_battle, _di);

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
                        state.Event.Invoke(_battle, _di);

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

                var model = _battle.EnemyModels.GetModel(enemy);

                if (enemy.Heal < oldHeal)
                {
                    BattleManager.BattleUtility.SpawnFallingText(model.DamageTextWorldPoint, (oldHeal - enemy.Heal).ToString(), Color.white, Color.red);
                }
                else if (enemy.Heal > oldHeal)
                {
                    BattleManager.BattleUtility.SpawnFallingText(model.DamageTextWorldPoint, (oldHeal - enemy.Heal).ToString(), Color.white, Color.green);
                }
            }
        }

        private IEnumerator EnemyTurn()
        {
            IsEnemyTurn = true;

            yield return new WaitForSeconds(0.5f);

            List<BattleTurnData> targets = new List<BattleTurnData>();

            foreach (var enemy in Data.Enemys)
            {
                if (enemy.States.Any(i => i.SkipTurn) || !enemy.Behaviours.Any())
                    continue;

                var behaviours = enemy.Behaviours[Random.Range(0, enemy.Behaviours.Count)];

                _battle.EnemyBehaviour.AddBehaviour(behaviours, enemy);
            }

            int targetedCharacterCount = Random.Range(1, Data.TurnsData.Where(i => !i.IsDead).Count() + 1);
            for (int i = 0; i < targetedCharacterCount; i++)
            {
                var turnData = Data.TurnsData[Random.Range(0, Data.TurnsData.Count)];

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

            if (_battle.EnemyBehaviour.BattleFieldRequired)
            {
                var field = _battle.BattleField.Create(_battle.Config.DefaultBattleField);
                field.Show();
            }

            _battle.EnemyBehaviour.Invoke();

            yield return new WaitWhile(() => _battle.EnemyBehaviour.IsWorking && !loseKey && !breakKey);

            if (loseKey || breakKey)
                _battle.EnemyBehaviour.Break();

            foreach (var item in targets)
            {
                item.IsTarget = false;
                _battle.UI.CharacterBox.GetBox(item.Character).MarkTarget(false);
            }

            _battle.Player.SetActive(false);
            _battle.BattleField.Dispose();
            _battle.Projectiles.Dispose();

            yield return new WaitForSeconds(0.3f);

            _battle.Player.Dispose();

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
                _battle.BattleAudio.PlaySound(Config.LoseTrack);

                _battle.UI.CharacterBox.SetActive(false);

                _shared.MessageDialog.Write(new MessageBoxInfo()
                {
                    text = $"* {Localization.GetLocale("SYS_BATTLE_LOSE")}",
                    closeWindow = true,
                });

                yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);
            }

            if (!Data.BattleInfo.CanLose)
                SceneManager.LoadScene(Config.GameOverSceneName);
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

                    lvlUpText += $"* {character.Character.Name} {Localization.GetLocale("SYS_LEVELUP")}\n";
                }
            }

            if (Data.BattleInfo.enemySquad.Money > 0)
            {
                int money = Data.BattleInfo.enemySquad.Money;

                if (Data.BattleInfo.enemySquad.MoneyConstDrop)
                    money = Mathf.RoundToInt(Random.Range(money * 0.65f, money * 1.35f));

                GlobalManager.Instance.GameData.Money += money;

                moneyText += $"* {Localization.GetLocale("SYS_BATTLE_YOU_GOT")} {money} {Localization.GetLocale("SYS_MONEY")}!\n";
            }

            bool first = true;
            foreach (var drop in Data.BattleInfo.enemySquad.EnemiesDrop)
            {
                if (Random.Range(0f, 1f) > drop.Chance)
                    continue;

                int count = Mathf.RoundToInt(Random.Range(drop.Count - drop.CountRange, drop.Count + drop.CountRange));

                GlobalManager.Instance.Inventory.AddToItemCount(drop.item, count);

                string countText = count > 1 ? $" {count}x" : "";

                if (first)
                {
                    dropText += $"* {Localization.GetLocale("SYS_BATTLE_YOU_GOT")} {drop.item.Name}{countText}";

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
                _battle.BattleAudio.PlaySound(Config.WinTrack);

                _shared.MessageDialog.Write(new MessageBoxInfo()
                {
                    text = $"* {Localization.GetLocale("SYS_BATTLE_WIN")}<!>",
                    closeWindow = true
                });

                yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);

                if (dropText != string.Empty || moneyText != string.Empty)
                {
                    _shared.MessageDialog.Write(new MessageBoxInfo()
                    {
                        text = moneyText + dropText,
                        closeWindow = true
                    });

                    yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);
                }

                if (lvlUpText != string.Empty)
                {
                    _shared.MessageDialog.Write(new MessageBoxInfo()
                    {
                        text = lvlUpText,
                        closeWindow = true
                    });

                    yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);
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
                case ChoiceAction.Defence:
                    yield return HandleDefenceChoice(turnData);
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
                    PreviewCharacter();

                    isCancelChoice = true;
                }
            }
            else
            {
                actionIndex = Choice.PrimaryCurrentIndex;

                switch (actionIndex)
                {
                    // Выбрана битва
                    case 0:
                        turnData.BattleAction = TurnAction.Attack;
                        choiceActions.Add(ChoiceAction.Enemy);
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
                        choiceActions.Add(ChoiceAction.Defence);
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
                if ((int)Choice.BattleChoice.Index == 0)
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

                turnData.EntityBuffer = null;

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
            _battle.Choice.InvokeChoiceCharacterOrEnemy();

            yield return new WaitWhile(() => _battle.Choice.IsChoicing);

            if (_battle.Choice.IsCanceled)
            {
                turnData.EntityBuffer = null;

                PreviewAction();
            }
            else
            {
                var result = _battle.Choice.BattleChoice.Index;
                if (result == 0)
                {
                    choiceActions.Add(ChoiceAction.Teammate);

                } 
                else
                {
                    choiceActions.Add(ChoiceAction.Enemy);
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
                PreviewAction();
            }
            else
            {
                turnData.EntityBuffer = (RPGCharacter)Choice.CurrentItem.Value;

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
                turnData.EntityBuffer = null;

                PreviewAction();
            }
            else
            {
                turnData.EntityBuffer = (RPGEnemy)Choice.CurrentItem.Value;

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
            if (GlobalManager.Instance.Inventory.Slots
                        .Where(i => i.Item.Usage == Usability.Battle ||
                                    i.Item.Usage == Usability.Any).Count() > 0)
            {
                _battle.Choice.InvokeChoiceItem();

                yield return new WaitWhile(() => _battle.Choice.IsChoicing);

                if (_battle.Choice.IsCanceled)
                {
                    turnData.Item = null;

                    PreviewAction();
                }
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
        private IEnumerator HandleDefenceChoice(BattleTurnData turnData)
        {
            _battle.Choice.InvokeChoiceDefence();

            yield return new WaitWhile(() => _battle.Choice.IsChoicing);

            if (_battle.Choice.IsCanceled)
                PreviewAction();
            else
            {
                if ((int)_battle.Choice.BattleChoice.Index == 0)
                {
                    Utility.AddConcetration(Config.AdditionConcentrationOnDefence);
                    turnData.ReservedConcentration = Config.AdditionConcentrationOnDefence;
                    turnData.BattleAction = TurnAction.Defence;
                }
                else
                {
                    turnData.BattleAction = TurnAction.Flee;
                }

                NextCharacter();
            }

            _battle.Choice.CleanUp();

            yield break;
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
            if (!Data.Enemys.Contains(turnData.EntityBuffer))
            {
                if (Data.Enemys.Count != 0)
                    turnData.EntityBuffer = Data.Enemys[0];
                else
                    yield break;
            }

            _battle.AttackQTE.Show();

            if (!_battle.AttackQTE.IsShowed)
                yield return new WaitForSeconds(1f);

            _battle.AttackQTE.Invoke();

            yield return new WaitWhile(() => _battle.AttackQTE.QTE.IsWorking);

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.BeforeHit, false, turnData.EntityBuffer.Tag));

            BattleAttackEffect effect = character.WeaponSlot == null ? Config.DefaultEffect : character.WeaponSlot.VisualEffect;

            _di.InjectInto(effect);

            if (effect.LocaleInCenter)
                effect = BattleManager.BattleUtility.SpawnAttackEffect(effect);
            else
            {
                Vector2 attackPos = _battle.EnemyModels.GetModel(turnData.EntityBuffer as RPGEnemy).AttackWorldPoint;

                effect = BattleManager.BattleUtility.SpawnAttackEffect(effect, attackPos);
            }

            effect.Play();

            yield return new WaitWhile(() => effect.IsPlaying);

            yield return new WaitForSeconds(0.5f);

            Object.Destroy(effect.gameObject);

            _battle.AttackQTE.Hide();

            BattleManager.BattleUtility.DamageEnemy(character, turnData.EntityBuffer as RPGEnemy, _battle.AttackQTE.QTE.DamageFactor);

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.AfterHit, false, turnData.EntityBuffer.Tag));

            yield return _battle.StartCoroutine(InvokeBattleEvent(RPGBattleEvent.InvokePeriod.OnLessEnemyHeal, false, turnData.EntityBuffer.Tag));
        }
        private IEnumerator HandleActAction(BattleTurnData turnData, RPGCharacter character)
        {
            if (turnData.InteractionAct.Event != null)
                turnData.InteractionAct.Event.Invoke(_battle, _di);

            if (turnData.InteractionAct.Minigame != null)
            {
                _battle.Minigame.InvokeMinigame(turnData.InteractionAct.Minigame);
                yield return new WaitWhile(() => _battle.Minigame.MinigameIsPlay);
            }

            yield return new WaitWhile(() => turnData.InteractionAct.Event.IsPlaying);
        }
        private IEnumerator HandleAbilityAction(BattleTurnData turnData, RPGCharacter character)
        {
            character.Mana -= turnData.Ability.ManaCost;

            if (turnData.Ability.StartEvent != null)
            {
                turnData.Ability.StartEvent.Invoke(_battle, _di);

                yield return new WaitWhile(() => turnData.Ability.StartEvent.IsPlaying);
            }

            switch (turnData.Ability.Direction)
            {
                case UsabilityDirection.AllTeam:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, Data.TurnsData.Select(i => i.Character).ToArray()));
                    break;
                case UsabilityDirection.Teammate:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, turnData.EntityBuffer));
                    break;
                case UsabilityDirection.AllEnemys:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, Data.Enemys.ToArray()));
                    break;
                case UsabilityDirection.Enemy:
                    yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(turnData.Ability, character, turnData.EntityBuffer));
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
                turnData.Ability.EndEvent.Invoke(_battle, _di);

                yield return new WaitWhile(() => turnData.Ability.EndEvent.IsPlaying);
            }
        }
        private IEnumerator HandleItemAction(BattleTurnData turnData, RPGCharacter character)
        {
            if (turnData.Item.Event != null)
            {
                yield return _invokeUsableEvent.AwaitInvokeEvent(turnData.Item);
            }

            if (turnData.IsConsumed && turnData.Item is RPGConsumed consumed)
            {
                if (consumed.WriteMessage)
                {
                    _shared.MessageDialog.Write(new MessageBoxInfo()
                    {
                        text = $"* {character.Name} использует {consumed.Name}!",
                        closeWindow = true,
                    });

                    yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);
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
                        yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, turnData.EntityBuffer));
                        break;
                    case UsabilityDirection.AllEnemys:
                        yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, Data.Enemys.ToArray()));
                        break;
                    case UsabilityDirection.Enemy:
                        yield return _battle.StartCoroutine(_battle.Utility.UseUsableTo(consumed, character, turnData.EntityBuffer));
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
            _battle.BattleAudio.PlaySound(Config.FleeSound);

            _battle.BattleAudio.PauseMusic();

            _shared.MessageDialog.Write(new MessageBoxInfo()
            {
                text = $"* {character.Name} пытаеться сбежать<\\:>.<\\:>.<\\:>.",
                closeWindow = true,
            });

            yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);

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

                _shared.MessageDialog.Write(new MessageBoxInfo()
                {
                    text = $"* Но сбежать не удалось",
                    closeWindow = true,
                });

                yield return new WaitWhile(() => _shared.MessageDialog.IsWriting);
            }
        }

        #endregion
    }
}