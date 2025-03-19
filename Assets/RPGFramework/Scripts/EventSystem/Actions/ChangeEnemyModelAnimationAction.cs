using RPGF.RPG;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ChangeEnemyModelAnimationAction : GraphActionBase
{
    public string EnemyTag;
    public string AnimatorTag;
    public string Trigger;

    public ChangeEnemyModelAnimationAction() : base("ChangeEnemyModelAnimation")
    {
        EnemyTag = string.Empty;
        AnimatorTag = string.Empty;
        Trigger = string.Empty;
    }

    public override IEnumerator ActionCoroutine()
    {
        try
        {
            if (BattleManager.IsBattle)
            {
                RPGEnemy enemy = BattleManager.Instance.data.Enemys.First(i => i.Tag == EnemyTag);

                if (enemy == null)
                    throw new ApplicationException("Enemy not found!");

                EnemyModel model = BattleManager.Instance.EnemyModels.GetModel(enemy);

                if (model == null)
                    throw new ApplicationException("Enemy model not found!");

                model.GetAnimator(AnimatorTag).SetTrigger(Trigger);
            }
        }
        catch (ApplicationException error)
        {
            Debug.LogException(error);
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Сменить анимацию врага";
    }
}