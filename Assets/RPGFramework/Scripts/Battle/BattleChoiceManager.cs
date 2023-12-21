using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleChoiceManager : MonoBehaviour
{
    public BattleData Data => BattleManager.Data;
    public BattleAudioManager BattleAudio => BattleManager.instance.battleAudio;
    public BattlePipeline Pipeline => BattleManager.instance.pipeline;

    [SerializeField]
    private BattleChoiceUI battleChoice;
    public BattleChoiceUI BattleChoice => battleChoice;

    [SerializeField]
    private PrimaryBattleChoiceUI primaryChoice;
    public PrimaryBattleChoiceUI PrimaryChoice => primaryChoice;

    public bool IsChoicing => battleChoice.IsChoicing || primaryChoice.IsChoicing;

    public bool IsCanceled => battleChoice.IsCanceled;
    public bool IsPrimaryCanceled => primaryChoice.IsCanceled;

    public IndependenceChoiceUI.ElementInfo CurrentItem => battleChoice.CurrentItem;

    public int PrimaryCurrentIndex => primaryChoice.Choice;

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
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Interaction ||
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Item)
        {
            ShowDescriptionFor(CurrentItem);
        }
    }

    private void Choice_OnEndChoice()
    {
        BattleManager.instance.description.SetActive(false);
    }

    private void Choice_OnCanceled()
    {
        BattleAudio.PlaySound(Data.Cancel);
    }

    private void Choice_OnSellectionChanged()
    {
        BattleAudio.PlaySound(Data.Hover);

        if (Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Ability ||
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Interaction ||
            Pipeline.CurrentChoiceAction == BattlePipeline.ChoiceAction.Item)
        {
            ShowDescriptionFor(CurrentItem);
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
        battleChoice.AppendTittle("Персонаж", IndependenceChoiceUI.Aling.center);

        List<IndependenceChoiceUI.ElementInfo> choices = new List<IndependenceChoiceUI.ElementInfo>();

        foreach (var item in Data.Characters)
        {
            choices.Add(new IndependenceChoiceUI.ElementInfo()
            {
                name = item.Entity.Name,
                value = item
            });
        }

        battleChoice.AppendElements(choices.ToArray());


        battleChoice.AppendTittle("Противник", IndependenceChoiceUI.Aling.center);

        List<IndependenceChoiceUI.ElementInfo> choices0 = new List<IndependenceChoiceUI.ElementInfo>();

        foreach (var item in Data.Enemys)
        {
            choices0.Add(new IndependenceChoiceUI.ElementInfo()
            {
                name = item.Entity.Name,
                value = item
            });
        }

        battleChoice.AppendElements(choices0.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceEnemy()
    {
        List<IndependenceChoiceUI.ElementInfo> choiceElements = new List<IndependenceChoiceUI.ElementInfo>();

        foreach (var enemy in Data.Enemys)
        {
            choiceElements.Add(new IndependenceChoiceUI.ElementInfo()
            {
                name = enemy.Entity.Name,
                value = enemy
            });
        }

        battleChoice.AppendElements(choiceElements.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceTeammate()
    {
        List<IndependenceChoiceUI.ElementInfo> choices = new List<IndependenceChoiceUI.ElementInfo>();

        BattleCharacterInfo current = BattleManager.instance.pipeline.CurrentChoicingCharacter;

        RPGConsumed consumed = current.Item as RPGConsumed;
        
        foreach (var item in Data.Characters)
        {
            IndependenceChoiceUI.ElementInfo info = new IndependenceChoiceUI.ElementInfo()
            {
                name = item.Entity.Name,
                value = item,
                locked = false
            };

            if (current.BattleAction == BattleCharacterAction.Item)
            {
                info.locked = !current.IsConsumed
                           && (item.IsDead != consumed.ForDeath || item.IsDead == consumed.ForAlive);
            }

            choices.Add(info);
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceAbility()
    {
        List<IndependenceChoiceUI.ElementInfo> choices = new List<IndependenceChoiceUI.ElementInfo>();

        foreach (var item in Pipeline.CurrentChoicingCharacter.Character.Abilities)
        {
            choices.Add(new IndependenceChoiceUI.ElementInfo()
            {
                name = item.Name,
                description = item.Destription + "\n" + (item.ManaCost > 0 ? $"[<color=#0081FF>Мана: {item.ManaCost}</color>] " : "") + (item.ConcentrationCost > 0 ? $"[<color=#06C100>Конц.: {item.ConcentrationCost}</color>]" : ""),
                value = item,
                locked = Pipeline.CurrentChoicingCharacter.Character.Mana < item.ManaCost || Data.Concentration < item.ConcentrationCost
            });
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceAct()
    {
        List<IndependenceChoiceUI.ElementInfo> choices = new List<IndependenceChoiceUI.ElementInfo>()
        {
            new IndependenceChoiceUI.ElementInfo()
            {
                name = "Взаимодействие",
                value = 0
            },
            new IndependenceChoiceUI.ElementInfo()
            {
                name = "Способность",
                value = 1,
                locked = Pipeline.CurrentChoicingCharacter.Character.Abilities.Count == 0
            }
        };

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceDefence()
    {
        List<IndependenceChoiceUI.ElementInfo> choices = new List<IndependenceChoiceUI.ElementInfo>()
        {
            new IndependenceChoiceUI.ElementInfo()
            {
                name = "Защита",
                value = 0
            },
            new IndependenceChoiceUI.ElementInfo()
            {
                name = "Бегство",
                value = 1,
                locked = !Data.BattleInfo.CanFlee
            }
        };

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceItem()
    {
        List<IndependenceChoiceUI.ElementInfo> choices = new List<IndependenceChoiceUI.ElementInfo>();

        foreach (var slot in GameManager.Instance.inventory.Slots)
        {
            if (slot.Item.Usage == RPGCollectable.Usability.Noway ||
                slot.Item.Usage == RPGCollectable.Usability.Explorer)
                continue;

            if (slot.Item is RPGConsumed)
            {
                int alreadyUsing = 0;
                foreach (var item in Data.Characters)
                    alreadyUsing += item.Item == slot.Item ? 1 : 0;

                if (alreadyUsing >= slot.Count)
                    continue;
            }

            IndependenceChoiceUI.ElementInfo element = new IndependenceChoiceUI.ElementInfo()
            {
                name = slot.Item.Name,
                description = slot.Item.Description,
                icon = slot.Item.Icon,
                counterText = slot.Count == 1 ? "" : $"{slot.Count}x",
                value = slot.Item
            };

            choices.Add(element);
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceInteraction()
    {
        List<IndependenceChoiceUI.ElementInfo> choiceElements = new List<IndependenceChoiceUI.ElementInfo>
        {
            new IndependenceChoiceUI.ElementInfo()
            {
                name = "Проверить",
                description = "Проверить текущего врага",
                value = new RPGEnemy.EnemyAct()
                {
                    Name = "Check"
                }
            }
        };

        foreach (var enemy in Data.Enemys)
        {
            foreach (var act in enemy.Enemy.Acts)
            {

                IndependenceChoiceUI.ElementInfo elementInfo = new IndependenceChoiceUI.ElementInfo()
                {
                    name = act.Name,
                    description = act.Description,
                    value = act,
                    locked = false
                };

                foreach (var character in Data.Characters)
                {
                    if (character.EnemyBuffer == enemy 
                        && character.InteractionAct.Name == act.Name && act.OnlyOne)
                    {
                        elementInfo.locked = true;
                        break;
                    }
                }
                    

                choiceElements.Add(elementInfo);
            }
        }

        battleChoice.AppendElements(choiceElements.ToArray());

        battleChoice.InvokeChoice();
    }

    #endregion

    public void CleanUp() => battleChoice.CleanUp();

    private void ShowDescriptionFor(IndependenceChoiceUI.ElementInfo element)
    {
        BattleManager.instance.description.SetActive(true);
        BattleManager.instance.description.SetText(element.description);
    }
}