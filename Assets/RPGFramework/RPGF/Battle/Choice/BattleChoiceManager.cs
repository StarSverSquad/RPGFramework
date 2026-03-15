using RPGF.Core;
using RPGF.RPG;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPGF.Core.Battle.Enums;
using RPGF.Core.Battle;

namespace RPGF.Battle.Choice
{
    public class BattleChoiceManager : RPGFrameworkBehaviour
    {
        [SerializeField]
        private EnemySelector _enemySelector;
        [SerializeField]
        private BattleUIEnemyInformation _enemyInformation;

        public BattleData Data => Battle.Data;
        public BattleConfig Config => Battle.Config;
        public BattleAudioManager BattleAudio => Battle.BattleAudio;
        public BattlePipeline Pipeline => Battle.Pipeline;

        [SerializeField]
        private BattleChoice battleChoice;
        public BattleChoice BattleChoice => battleChoice;

        [SerializeField]
        private PrimaryBattleChoiceUI primaryChoice;
        public PrimaryBattleChoiceUI PrimaryChoice => primaryChoice;

        public bool IsChoicing => battleChoice.IsChoiceCoroutineWorking || primaryChoice.IsChoicing;

        public bool IsCanceled => battleChoice.State == Core.Choice.ChoiceState.Canceled;
        public bool IsPrimaryCanceled => primaryChoice.IsCanceled;

        public BattleChoiceItem CurrentItem => battleChoice.Current;

        public int PrimaryCurrentIndex => primaryChoice.Choice;

        public override void Initialize()
        {
            Local.DI.InjectInto(primaryChoice);
            primaryChoice.Initialize();

            Local.DI.InjectInto(battleChoice);
            battleChoice.Initialize();

            _enemySelector.Initialize();
            _enemyInformation.Initialize();
        }

        private void Start()
        {
            battleChoice.OnConfirmEvent += Choice_OnSuccess;
            battleChoice.OnConfirmLockedEvent += Choice_OnDeny;
            battleChoice.OnSelectionChangedEvent += Choice_OnSellectionChanged;
            battleChoice.OnCancelEvent += Choice_OnCanceled;
            battleChoice.OnEndedEvent += Choice_OnEndChoice;
            battleChoice.OnStartedEvent += Choice_OnStartChoice;

            primaryChoice.OnSellectionChanged += PrimaryChoice_OnSellectionChanged;
            primaryChoice.OnSuccess += PrimaryChoice_OnSuccess;
            primaryChoice.OnCanceled += PrimaryChoice_OnCanceled;
        }

        private void OnDestroy()
        {
            battleChoice.OnConfirmEvent -= Choice_OnSuccess;
            battleChoice.OnConfirmLockedEvent -= Choice_OnDeny;
            battleChoice.OnSelectionChangedEvent -= Choice_OnSellectionChanged;
            battleChoice.OnCancelEvent -= Choice_OnCanceled;
            battleChoice.OnEndedEvent -= Choice_OnEndChoice;
            battleChoice.OnStartedEvent -= Choice_OnStartChoice;

            primaryChoice.OnSellectionChanged -= PrimaryChoice_OnSellectionChanged;
            primaryChoice.OnSuccess -= PrimaryChoice_OnSuccess;
            primaryChoice.OnCanceled -= PrimaryChoice_OnCanceled;

            _enemySelector.Dispose();
        }

        #region Primary choice callbacks

        private void PrimaryChoice_OnCanceled()
        {
            BattleAudio.PlaySound(Config.CancelSound);
        }

        private void PrimaryChoice_OnSuccess()
        {
            BattleAudio.PlaySound(Config.SellectSound);
        }

        private void PrimaryChoice_OnSellectionChanged()
        {
            BattleAudio.PlaySound(Config.HoverSound);
        }

        #endregion

        #region Choice callbacks

        private void Choice_OnStartChoice()
        {
            if (Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Ability ||
                Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Act ||
                Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Item)
            {
                ShowDescriptionFor(CurrentItem);
            }
        }

        private void Choice_OnEndChoice()
        {
            BattleManager.Instance.UI.Description.SetActive(false);

            _enemySelector.Dispose();
            _enemyInformation.HideInformation();
        }

        private void Choice_OnCanceled()
        {
            BattleAudio.PlaySound(Config.CancelSound);

            _enemySelector.Dispose();
            _enemyInformation.HideInformation();
        }

        private void Choice_OnSellectionChanged(BattleChoiceItem current, int currentIndex, int prevIndex)
        {
            BattleAudio.PlaySound(Config.HoverSound);

            if (Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Ability ||
                Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Act ||
                Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Item)
            {
                ShowDescriptionFor(current);
            }

            if (Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Enemy ||
                Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Entity)
            {
                if (current.Value is RPGEnemy enemy)
                {
                    _enemySelector.Select(Battle.EnemyModels.GetModel(enemy));
                    _enemyInformation.ShowInformation(enemy);
                }
                else
                {
                    _enemySelector.Dispose();
                    _enemyInformation.HideInformation();
                }
            }
        }

        private void Choice_OnDeny(int index)
        {
            BattleAudio.PlaySound(Config.DenySound);
        }

        private void Choice_OnSuccess(BattleChoiceItem result)
        {
            BattleAudio.PlaySound(Config.SellectSound);
        }

        #endregion

        #region Choice invokers

        public void InvokePrimaryChoice(int startIndex = 0)
        {
            primaryChoice.InvokeChoice(startIndex);
        }

        public void InvokeChoiceCharacterOrEnemy()
        {
            BattleChoiceItem[] items = new BattleChoiceItem[]
            {
                new BattleChoiceItem
                {
                    Label = "Союзник",
                },
                new BattleChoiceItem
                {
                    Label = "Враг",
                }
            };

            battleChoice.Invoke(items);
        }

        public void InvokeChoiceEnemy()
        {
            BattleTurnData current = Battle.Pipeline.CurrentTurnData;

            List<BattleChoiceItem> choiceElements = Data.Enemys.Select(enemy =>
            {
                bool locked = false;

                if (current.BattleAction == TurnAction.Act)
                {
                    locked = !enemy.Acts.Any();
                }

                return new BattleChoiceItem()
                {
                    Label = enemy.Name,
                    Value = enemy,
                    IsLocked = locked
                };
            }).ToList();

            battleChoice.Invoke(choiceElements);
        }

        public void InvokeChoiceTeammate()
        {
            BattleTurnData current = Battle.Pipeline.CurrentTurnData;
            RPGConsumed consumed = current.Item as RPGConsumed;

            var choices = Data.TurnsData.Select(turnData =>
            {
                BattleChoiceItem info = new()
                {
                    Label = turnData.Character.Name,
                    Value = turnData.Character,
                    IsLocked = false
                };

                if (current.BattleAction == TurnAction.Item)
                {
                    info.IsLocked = !(current.IsConsumed &&
                        (turnData.IsDead && consumed.ForDeath) || (!turnData.IsDead && consumed.ForAlive));
                }
                else if (current.BattleAction == TurnAction.Act)
                {
                    info.IsLocked = turnData.IsDead;
                }

                return info;
            }).ToList();

            battleChoice.Invoke(choices);
        }

        public void InvokeChoiceAbility()
        {
            var choices = Pipeline.CurrentTurnData.Character.Abilities.Select(item =>
            {
                return new BattleChoiceItem()
                {
                    Label = item.Name,
                    Description = item.Description + "\n" + (item.ManaCost > 0 ? $"[<color=#0081FF>Мана: {item.ManaCost}</color>] " : "") + (item.ConcentrationCost > 0 ? $"[<color=#06C100>Конц.: {item.ConcentrationCost}</color>]" : ""),
                    Value = item,
                    IsLocked = Pipeline.CurrentTurnData.Character.Mana < item.ManaCost || Data.Concentration < item.ConcentrationCost,
                    Icon = item.Icon
                };
            }).ToArray();

            battleChoice.Invoke(choices);
        }

        public void InvokeChoiceAct()
        {
            List<BattleChoiceItem> choices = new()
            {
                new BattleChoiceItem()
                {
                    Label = "Действие"
                },
                new BattleChoiceItem()
                {
                    Label = "Способность",
                    IsLocked = Pipeline.CurrentTurnData.Character.Abilities.Count == 0
                }
            };

            battleChoice.Invoke(choices);
        }

        public void InvokeChoiceDefence()
        {
            List<BattleChoiceItem> choices = new()
            {
                new BattleChoiceItem()
                {
                    Label = "Защита"
                }
            };

            if (Data.BattleInfo.CanFlee)
            {
                choices.Add(new()
                {
                    Label = "Бегство"
                });
            }

            battleChoice.Invoke(choices);
        }

        public void InvokeChoiceItem()
        {
            List<BattleChoiceItem> choices = new();

            foreach (var slot in GlobalManager.Instance.Inventory.Slots)
            {
                if (slot.Item.Usage == Usability.Noway ||
                    slot.Item.Usage == Usability.Explorer)
                    continue;

                int alreadyUsing = 0;
                if (slot.Item is RPGConsumed)
                {
                    foreach (var item in Data.TurnsData)
                        alreadyUsing += item.Item == slot.Item ? 1 : 0;
                }

                BattleChoiceItem element = new()
                {
                    Label = GetLocale($"{slot.Item.Tag}_name", slot.Item.Name),
                    Description = GetLocale($"{slot.Item.Tag}_description", slot.Item.Description),
                    Icon = slot.Item.Icon,
                    CounterText = slot.Count - alreadyUsing == 1 ? "" : $"{slot.Count - alreadyUsing}x",
                    Value = slot.Item,
                    IsLocked = alreadyUsing >= slot.Count
                };

                choices.Add(element);
            }

            battleChoice.Invoke(choices);
        }

        public void InvokeChoiceBattle()
        {
            List<BattleChoiceItem> choices = new()
            {
                new BattleChoiceItem()
                {
                    Label = "Атака"
                }
            };

            if (Data.BattleInfo.CanFlee)
            {
                choices.Add(new()
                {
                    Label = "Защита"
                });
            }

            battleChoice.Invoke(choices);
        }

        public void InvokeChoiceInteraction()
        {
            List<BattleChoiceItem> choiceElements = new();

            BattleTurnData currentCharacter = Battle.Pipeline.CurrentTurnData;

            foreach (var act in (currentCharacter.EntityBuffer as RPGEnemy).Acts)
            {
                BattleChoiceItem elementInfo = new()
                {
                    Label = act.Name,
                    Description = act.Description,
                    Value = act,
                    IsLocked = false
                };

                foreach (var character in Data.TurnsData)
                {
                    if (character.EntityBuffer == currentCharacter.EntityBuffer
                        && character.InteractionAct.Name == act.Name && act.OnlyOne)
                    {
                        elementInfo.IsLocked = true;
                        break;
                    }
                }

                choiceElements.Add(elementInfo);
            }

            battleChoice.Invoke();
        }

        #endregion

        public void CleanUp() => battleChoice.Dispose();

        private void ShowDescriptionFor(BattleChoiceItem element)
        {
            BattleManager.Instance.UI.Description.SetActive(true);
            BattleManager.Instance.UI.Description.Text = element.Description;
        }
    }
}