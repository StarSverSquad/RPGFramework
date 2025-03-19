using RPGF.Choice;
using RPGF.RPG;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleTurnData;

public class BattleChoiceManager : RPGFrameworkBehaviour
{
    [SerializeField]
    private EnemySelector _enemySelector;

    public BattleData Data => BattleManager.Data;
    public BattleAudioManager BattleAudio => BattleManager.Instance.BattleAudio;
    public BattlePipeline Pipeline => BattleManager.Instance.Pipeline;

    [SerializeField]
    private ScrollableChoiceUI battleChoice;
    public ScrollableChoiceUI BattleChoice => battleChoice;

    [SerializeField]
    private PrimaryBattleChoiceUI primaryChoice;
    public PrimaryBattleChoiceUI PrimaryChoice => primaryChoice;

    public bool IsChoicing => battleChoice.IsChoicing || primaryChoice.IsChoicing;

    public bool IsCanceled => battleChoice.IsCanceled;
    public bool IsPrimaryCanceled => primaryChoice.IsCanceled;

    public ChoiceUI.Element CurrentItem => battleChoice.CurrentUIElement;

    public int PrimaryCurrentIndex => primaryChoice.Choice;

    public override void Initialize()
    {
        _enemySelector.Initialize();
    }

    private void Start()
    {
        battleChoice.OnSuccess += Choice_OnSuccess;
        battleChoice.OnDeny += Choice_OnDeny;
        battleChoice.OnSellectionChanged += Choice_OnSellectionChanged;
        battleChoice.OnCanceled += Choice_OnCanceled;
        battleChoice.OnEnd += Choice_OnEndChoice;
        battleChoice.OnStart += Choice_OnStartChoice;

        primaryChoice.OnSellectionChanged += PrimaryChoice_OnSellectionChanged;
        primaryChoice.OnSuccess += PrimaryChoice_OnSuccess;
        primaryChoice.OnCanceled += PrimaryChoice_OnCanceled;
    }

    private void OnDestroy()
    {
        battleChoice.OnSuccess -= Choice_OnSuccess;
        battleChoice.OnDeny -= Choice_OnDeny;
        battleChoice.OnSellectionChanged -= Choice_OnSellectionChanged;
        battleChoice.OnCanceled -= Choice_OnCanceled;
        battleChoice.OnEnd -= Choice_OnEndChoice;
        battleChoice.OnStart -= Choice_OnStartChoice;

        primaryChoice.OnSellectionChanged -= PrimaryChoice_OnSellectionChanged;
        primaryChoice.OnSuccess -= PrimaryChoice_OnSuccess;
        primaryChoice.OnCanceled -= PrimaryChoice_OnCanceled;

        _enemySelector.Dispose();
    }

    #region Primary choice callbacks

    private void PrimaryChoice_OnCanceled()
    {
        BattleAudio.PlaySound(Data.Cancel);
    }

    private void PrimaryChoice_OnSuccess()
    {
        BattleAudio.PlaySound(Data.Sellect);
    }

    private void PrimaryChoice_OnSellectionChanged()
    {
        BattleAudio.PlaySound(Data.Hover);
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
    }

    private void Choice_OnCanceled()
    {
        BattleAudio.PlaySound(Data.Cancel);

        _enemySelector.Dispose();
    }

    private void Choice_OnSellectionChanged()
    {
        BattleAudio.PlaySound(Data.Hover);

        if (Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Ability ||
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Act ||
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Item)
        {
            ShowDescriptionFor(CurrentItem);
        }

        if (Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Enemy ||
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Entity)
        {
            if (battleChoice.CurrentUIElement.Value is RPGEnemy enemy)
            {
                _enemySelector.Select(Battle.EnemyModels.GetModel(enemy));
            }
            else
            {
                _enemySelector.Dispose();
            }
        }
    }

    private void Choice_OnDeny()
    {
        BattleAudio.PlaySound(Data.Deny);
    }

    private void Choice_OnSuccess()
    {
        BattleAudio.PlaySound(Data.Sellect);
    }

    #endregion

    #region Choice invokers

    public void InvokePrimaryChoice(int startIndex = 0)
    {
        primaryChoice.InvokeChoice(startIndex);
    }

    public void InvokeChoiceEntity()
    {
        battleChoice.AppendTitle("Персонаж", TMPro.TextAlignmentOptions.Center);

        var choices = Data.TurnsData.Select(item => new ChoiceUI.Element
        {
            Name = item.Character.Name,
            Value = item.Character
        }).ToList();

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.AppendTitle("Противник", TMPro.TextAlignmentOptions.Center);

        var enemyChoices = Data.Enemys.Select(item => new ChoiceUI.Element
        {
            Name = item.Name,
            Value = item
        }).ToList();

        battleChoice.AppendElements(enemyChoices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceEnemy()
    {
        List<ChoiceUI.Element> choiceElements = Data.Enemys.Select(enemy =>
        {
            return new ChoiceUI.Element()
            {
                Name = enemy.Name,
                Value = enemy
            };
        }).ToList();

        battleChoice.AppendElements(choiceElements.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceTeammate()
    {
        BattleTurnData current = Battle.Pipeline.CurrentTurnData;
        RPGConsumed consumed = current.Item as RPGConsumed;

        var choices = Data.TurnsData.Select(turnData =>
        {
            ChoiceUI.Element info = new()
            {
                Name = turnData.Character.Name,
                Value = turnData.Character,
                locked = false
            };

            if (current.BattleAction == TurnAction.Item)
            {
                info.locked = !(current.IsConsumed &&
                    (turnData.IsDead && consumed.ForDeath) || (!turnData.IsDead && consumed.ForAlive));
            }
            else if (current.BattleAction == TurnAction.Act)
            {
                info.locked = turnData.IsDead;
            }

            return info;
        }).ToList();

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceAbility()
    {
        var choices = Pipeline.CurrentTurnData.Character.Abilities.Select(item =>
        {
            return new ChoiceUI.Element()
            {
                Name = item.Name,
                Description = item.Description + "\n" + (item.ManaCost > 0 ? $"[<color=#0081FF>Мана: {item.ManaCost}</color>] " : "") + (item.ConcentrationCost > 0 ? $"[<color=#06C100>Конц.: {item.ConcentrationCost}</color>]" : ""),
                Value = item,
                locked = Pipeline.CurrentTurnData.Character.Mana < item.ManaCost || Data.Concentration < item.ConcentrationCost,
                Icon = item.Icon
            };
        }).ToArray();

        battleChoice.AppendElements(choices);

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceAct()
    {
        List<ChoiceUI.Element> choices = new List<ChoiceUI.Element>()
        {
            new ChoiceUI.Element()
            {
                Name = "Действие",
                Value = 0
            },
            new ChoiceUI.Element()
            {
                Name = "Способность",
                Value = 1,
                locked = Pipeline.CurrentTurnData.Character.Abilities.Count == 0
            }
        };

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceDefence()
    {
        List<ChoiceUI.Element> choices = new List<ChoiceUI.Element>()
        {
            new ChoiceUI.Element()
            {
                Name = "Защита",
                Value = 0
            }
        };

        if (Data.BattleInfo.CanFlee)
        {
            choices.Add(new ChoiceUI.Element()
            {
                Name = "Бегство",
                Value = 1
            });
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceItem()
    {
        List<ChoiceUI.Element> choices = new List<ChoiceUI.Element>();

        foreach (var slot in GameManager.Instance.Inventory.Slots)
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

            ChoiceUI.Element element = new ChoiceUI.Element()
            {
                Name = slot.Item.Name,
                Description = slot.Item.Description,
                Icon = slot.Item.Icon,
                CounterText = slot.Count - alreadyUsing == 1 ? "" : $"{slot.Count - alreadyUsing}x",
                Value = slot.Item,
                locked = alreadyUsing >= slot.Count
            };

            choices.Add(element);
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceBattle()
    {
        List<ChoiceUI.Element> choices = new List<ChoiceUI.Element>()
        {
            new ChoiceUI.Element()
            {
                Name = "Атака",
                Value = 0
            }
        };

        if (Data.BattleInfo.CanFlee)
        {
            choices.Add(new ChoiceUI.Element()
            {
                Name = "Защита",
                Value = 1
            });
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceInteraction()
    {
        List<ChoiceUI.Element> choiceElements = new List<ChoiceUI.Element>
        {
            new ChoiceUI.Element()
            {
                Name = "Проверить",
                Description = "Проверить текущего врага",
                Value = new RPGEnemy.EnemyAct()
                {
                    Name = "Check"
                }
            }
        };

        BattleTurnData currentCharacter = Battle.Pipeline.CurrentTurnData;

        foreach (var act in currentCharacter.EnemyBuffer.Acts)
        {

            ChoiceUI.Element elementInfo = new ChoiceUI.Element()
            {
                Name = act.Name,
                Description = act.Description,
                Value = act,
                locked = false
            };

            foreach (var character in Data.TurnsData)
            {
                if (character.EnemyBuffer == currentCharacter.EnemyBuffer
                    && character.InteractionAct.Name == act.Name && act.OnlyOne)
                {
                    elementInfo.locked = true;
                    break;
                }
            }


            choiceElements.Add(elementInfo);
        }

        battleChoice.AppendElements(choiceElements.ToArray());

        battleChoice.InvokeChoice();
    }

    #endregion

    public void CleanUp() => battleChoice.Dispose();

    private void ShowDescriptionFor(ChoiceUI.Element element)
    {
        BattleManager.Instance.UI.Description.SetActive(true);
        BattleManager.Instance.UI.Description.Text = element.Description;
    }
}