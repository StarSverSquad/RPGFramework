using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleChoiceManager : MonoBehaviour
{
    public BattleData Data => BattleManager.Data;
    public BattleAudioManager BattleAudio => BattleManager.Instance.battleAudio;
    public BattlePipeline Pipeline => BattleManager.Instance.pipeline;

    [SerializeField]
    private BattleChoiceUI battleChoice;
    public BattleChoiceUI BattleChoice => battleChoice;

    [SerializeField]
    private PrimaryBattleChoiceUI primaryChoice;
    public PrimaryBattleChoiceUI PrimaryChoice => primaryChoice;

    public bool IsChoicing => battleChoice.IsChoicing || primaryChoice.IsChoicing;

    public bool IsCanceled => battleChoice.IsCanceled;
    public bool IsPrimaryCanceled => primaryChoice.IsCanceled;

    public CommonChoiceUI.ElementInfo CurrentItem => battleChoice.CurrentItem;

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
        //BattleManager.Instance.description.SetActive(false);
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
        battleChoice.AppendTittle("Персонаж", CommonChoiceUI.Aling.center);

        List<CommonChoiceUI.ElementInfo> choices = new List<CommonChoiceUI.ElementInfo>();

        foreach (var item in Data.Characters)
        {
            choices.Add(new CommonChoiceUI.ElementInfo()
            {
                name = item.Entity.Name,
                value = item
            });
        }

        battleChoice.AppendElements(choices.ToArray());


        battleChoice.AppendTittle("Противник", CommonChoiceUI.Aling.center);

        List<CommonChoiceUI.ElementInfo> choices0 = new List<CommonChoiceUI.ElementInfo>();

        foreach (var item in Data.Enemys)
        {
            choices0.Add(new    CommonChoiceUI.ElementInfo()
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
        List<CommonChoiceUI.ElementInfo> choiceElements = new List<CommonChoiceUI.ElementInfo>();

        foreach (var enemy in Data.Enemys)
        {
            choiceElements.Add(new CommonChoiceUI.ElementInfo()
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
        List<CommonChoiceUI.ElementInfo> choices = new List<CommonChoiceUI.ElementInfo>();

        BattleCharacterInfo current = BattleManager.Instance.pipeline.CurrentChoicingCharacter;

        RPGConsumed consumed = current.Item as RPGConsumed;
        
        foreach (var character in Data.Characters)
        {
            CommonChoiceUI.ElementInfo info = new CommonChoiceUI.ElementInfo()
            {
                name = character.Entity.Name,
                value = character,
                locked = false
            };

            if (current.BattleAction == BattleCharacterAction.Item)
            {
                info.locked = !current.IsConsumed &&
                    (character.IsDead && !consumed.ForDeath) || (!character.IsDead == !consumed.ForAlive);
            }
            else if (current.BattleAction == BattleCharacterAction.Act)
            {
                info.locked = character.IsDead;
            }

            choices.Add(info);
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceAbility()
    {
        List<CommonChoiceUI.ElementInfo> choices = new List<CommonChoiceUI.ElementInfo>();

        foreach (var item in Pipeline.CurrentChoicingCharacter.Character.Abilities)
        {
            choices.Add(new CommonChoiceUI.ElementInfo()
            {
                name = item.Name,
                description = item.Destription + "\n" + (item.ManaCost > 0 ? $"[<color=#0081FF>Мана: {item.ManaCost}</color>] " : "") + (item.ConcentrationCost > 0 ? $"[<color=#06C100>Конц.: {item.ConcentrationCost}</color>]" : ""),
                value = item,
                locked = Pipeline.CurrentChoicingCharacter.Character.Mana < item.ManaCost || Data.Concentration < item.ConcentrationCost,
                icon = item.icon
            });
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceAct()
    {
        List<CommonChoiceUI.ElementInfo> choices = new List<CommonChoiceUI.ElementInfo>()
        {
            new CommonChoiceUI.ElementInfo()
            {
                name = "Взаимодействие",
                value = 0
            },
            new CommonChoiceUI.ElementInfo()
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
        List<CommonChoiceUI.ElementInfo> choices = new List<CommonChoiceUI.ElementInfo>()
        {
            new CommonChoiceUI.ElementInfo()
            {
                name = "Защита",
                value = 0
            }
        };

        if (Data.BattleInfo.CanFlee)
        {
            choices.Add(new CommonChoiceUI.ElementInfo()
            {
                name = "Бегство",
                value = 1
            });
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceItem()
    {
        List<CommonChoiceUI.ElementInfo> choices = new List<CommonChoiceUI.ElementInfo>();

        foreach (var slot in GameManager.Instance.Inventory.Slots)
        {
            if (slot.Item.Usage == RPGCollectable.Usability.Noway ||
                slot.Item.Usage == RPGCollectable.Usability.Explorer)
                continue;

            int alreadyUsing = 0;
            if (slot.Item is RPGConsumed)
            {
                foreach (var item in Data.Characters)
                    alreadyUsing += item.Item == slot.Item ? 1 : 0;
            }

            CommonChoiceUI.ElementInfo element = new CommonChoiceUI.ElementInfo()
            {
                name = slot.Item.Name,
                description = slot.Item.Description,
                icon = slot.Item.Icon,
                counterText = slot.Count - alreadyUsing == 1 ? "" : $"{slot.Count - alreadyUsing}x",
                value = slot.Item,
                locked = alreadyUsing >= slot.Count
            };

            choices.Add(element);
        }

        battleChoice.AppendElements(choices.ToArray());

        battleChoice.InvokeChoice();
    }

    public void InvokeChoiceInteraction()
    {
        List<CommonChoiceUI.ElementInfo> choiceElements = new List<CommonChoiceUI.ElementInfo>
        {
            new CommonChoiceUI.ElementInfo()
            {
                name = "Проверить",
                description = "Проверить текущего врага",
                value = new RPGEnemy.EnemyAct()
                {
                    Name = "Check"
                }
            }
        };

        BattleCharacterInfo currentCharacter = BattleManager.Instance.pipeline.CurrentChoicingCharacter;

        foreach (var act in currentCharacter.EnemyBuffer.Enemy.Acts)
        {

            CommonChoiceUI.ElementInfo elementInfo = new CommonChoiceUI.ElementInfo()
            {
                name = act.Name,
                description = act.Description,
                value = act,
                locked = false
            };

            foreach (var character in Data.Characters)
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

    public void CleanUp() => battleChoice.CleanUp();

    private void ShowDescriptionFor(CommonChoiceUI.ElementInfo element)
    {
        //BattleManager.Instance.description.SetActive(true);
        //BattleManager.Instance.description.SetText(element.description);
    }
}