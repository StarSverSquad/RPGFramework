﻿using System.Collections;
using System.Linq;
using UnityEngine;

public class BattleUsingService
{
    private BattleManager battleManager;
    private GameManager gameManager;

    public BattleUsingService(BattleManager battleManager, GameManager gameManager)
    {
        this.battleManager = battleManager;
        this.gameManager = gameManager;
    }

    public IEnumerator UseAbility(RPGAbility ability, BattleEntityInfo user, params BattleEntityInfo[] targets) 
    {
        float minigameFactor = 1f;

        if (targets.Any(i => i is BattleEnemyInfo) && ability.VisualEffect != null)
        {
            EnemyModel model = BattleManager.Instance.enemyModels.GetModel(targets.Where(i => i is BattleEnemyInfo).First() as BattleEnemyInfo);

            VisualAttackEffect effect;

            if (targets.Where(i => i is BattleEnemyInfo).Count() == 1)
                effect = battleManager.utility.SpawnAttackEffect(ability.VisualEffect);
            else 
                effect = battleManager.utility.SpawnAttackEffect(ability.VisualEffect, model.AttackGlobalPoint);

            effect.Invoke();

            yield return new WaitWhile(() => effect.IsAnimating);

            yield return new WaitForSeconds(0.5f);

            Object.Destroy(effect.gameObject);
        }

        foreach (BattleEntityInfo target in targets)
        {
            if (target is BattleCharacterInfo chr)
            {
                if ((chr.IsDead && !ability.ForDeath) || (!chr.IsDead && !ability.ForAlive))
                {
                    continue;
                }
            }

            int oldHp = target.Heal;
            int oldMp = target.Mana;
            string[] oldStates = target.States.Select(x => x.Tag).ToArray();

            foreach (var effect in ability.Effects)
                yield return battleManager.pipeline.StartCoroutine(effect.Invoke(user, target));

            RPGEntityState[] states = target.States.Where(i => oldStates.All(y => i.Tag != y)).ToArray();
            int healDif = target.Heal - oldHp, manaDif = target.Mana - oldMp;

            Debug.Log(ability.Formula);
            
            if (ability.Formula >= 0)
                target.Heal += Mathf.RoundToInt(ability.Formula * minigameFactor);
            else
                healDif -= target.Entity.GiveDamage(Mathf.RoundToInt((Mathf.Abs(ability.Formula) + user.Entity.Damage * 0.25f) * minigameFactor));

            if (target is BattleEnemyInfo enemy)
            {
                if (!battleManager.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = BattleManager.Instance.enemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    battleManager.utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);

                    model.Damage();
                }
                    
                else if (healDif > 0)
                    battleManager.utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.white, Color.green);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.utility.SpawnFallingText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        Color.white, states[i].Color);
                }


                if (enemy.Heal <= 0)
                {
                    BattleManager.Instance.battleAudio.PlaySound(battleManager.data.EnemyDeath);
                    model.Death();
                }
                else if (healDif < 0)
                    battleManager.battleAudio.PlaySound(battleManager.data.EnemyDamage);
                else
                    battleManager.battleAudio.PlaySound(battleManager.data.Heal);
            }
            else if (target is BattleCharacterInfo character)
            {

                CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(character);

                if (healDif < 0)
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);
                else if (healDif > 0)
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.white, Color.green);

                if (manaDif > 0)
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.4f),
                                    manaDif.ToString(), Color.white, Color.cyan);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), states[i].Name,
                                                            Color.white, states[i].Color);
                }

                if (character.Heal <= 0)
                    character.Heal = 1;

                if (ability.WakeupCharacter && character.IsDead)
                {
                    box.SetDead(false);

                    character.IsDead = false;

                    if (character.Heal <= 0)
                        character.Heal = 1;
                }

                if (healDif < 0)
                    battleManager.battleAudio.PlaySound(battleManager.data.Hurt);
                else
                    battleManager.battleAudio.PlaySound(battleManager.data.Heal);
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    public IEnumerator UseItem(RPGConsumed item, BattleEntityInfo user, params BattleEntityInfo[] targets)
    {
        if (targets.Any(i => i is BattleEnemyInfo) && item.VisualEffect != null)
        {
            EnemyModel model = BattleManager.Instance.enemyModels.GetModel(targets.Where(i => i is BattleEnemyInfo).First() as BattleEnemyInfo);

            VisualAttackEffect effect;

            if (targets.Where(i => i is BattleEnemyInfo).Count() == 1)
                effect = battleManager.utility.SpawnAttackEffect(item.VisualEffect);
            else
                effect = battleManager.utility.SpawnAttackEffect(item.VisualEffect, model.AttackGlobalPoint);

            effect.Invoke();

            yield return new WaitWhile(() => effect.IsAnimating);

            yield return new WaitForSeconds(0.5f);

            Object.Destroy(effect.gameObject);
        }

        foreach (BattleEntityInfo target in targets)
        {
            if (target is BattleCharacterInfo chr)
            {
                if ((chr.IsDead && !item.ForDeath) || (!chr.IsDead && !item.ForAlive))
                {
                    continue;
                }
            }

            int oldHp = target.Heal;
            int oldMp = target.Mana;
            string[] oldStates = target.States.Select(x => x.Tag).ToArray();

            foreach (var effect in item.Effects)
                yield return battleManager.pipeline.StartCoroutine(effect.Invoke(user, target));

            RPGEntityState[] states = target.States.Where(i => oldStates.All(y => i.Tag != y)).ToArray();
            int healDif = target.Heal - oldHp, manaDif = target.Mana - oldMp;

            if (target is BattleEnemyInfo enemy)
            {
                if (!battleManager.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = BattleManager.Instance.enemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    battleManager.utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);

                    model.Damage();
                }
                    
                else if (healDif > 0)
                    battleManager.utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.white, Color.green);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.utility.SpawnFallingText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        Color.white, states[i].Color);
                }


                if (enemy.Heal <= 0)
                {
                    BattleManager.Instance.battleAudio.PlaySound(battleManager.data.EnemyDeath);
                    model.Death();
                }
                else if (healDif < 0)
                    battleManager.battleAudio.PlaySound(battleManager.data.EnemyDamage);
                else
                    battleManager.battleAudio.PlaySound(battleManager.data.Heal);
            }
            else if (target is BattleCharacterInfo character)
            {

                CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(character);

                if (healDif < 0)
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);
                else if (healDif > 0)
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.white, Color.green);

                if (manaDif > 0)
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.4f),
                                    manaDif.ToString(), Color.white, Color.cyan);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), states[i].Name,
                                                            Color.white, states[i].Color);
                }

                if (character.Heal <= 0)
                    character.Heal = 1;
                    

                if (item.WakeupCharacter && character.IsDead)
                {
                    box.SetDead(false);

                    character.IsDead = false;

                    if (character.Heal <= 0)
                        character.Heal = 1;
                }

                if (healDif < 0)
                    battleManager.battleAudio.PlaySound(battleManager.data.Hurt);
                else
                    battleManager.battleAudio.PlaySound(battleManager.data.Heal);
            }

            yield return new WaitForSeconds(.5f);
        }

        gameManager.Inventory.AddToItemCount(item, -1);
    }
}
