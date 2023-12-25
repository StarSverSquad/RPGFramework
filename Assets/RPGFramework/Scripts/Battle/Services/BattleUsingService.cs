using System.Collections;
using System.Collections.Generic;
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

            List<RPGEntityState> states = new List<RPGEntityState>();
            int healDif = 0, manaDif = 0;

            if (ability.Damage > 0)
            {
                healDif = target.Entity.GiveDamage(ability.Damage + Mathf.RoundToInt(user.Entity.Damage * 0.25f));
            }

            foreach (var effect in ability.Effects)
            {
                yield return battleManager.pipeline.StartCoroutine(effect.BattleInvoke(user, target));

                if (effect.InfoIsExists("AddState"))
                    states.Add(effect["AddState"] as RPGEntityState);

                if (effect.InfoIsExists("Heal"))
                    healDif += (int)effect["Heal"];

                if (effect.InfoIsExists("Mana"))
                    manaDif += (int)effect["Mana"];
            }

            if (target is BattleEnemyInfo enemy)
            {
                if (!battleManager.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = BattleManager.Instance.enemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    battleManager.utility.SpawnDamageText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.red, Color.white);

                    model.Damage();
                }
                    
                else if (healDif > 0)
                    battleManager.utility.SpawnDamageText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.green, Color.white);

                for (int i = 0; i < states.Count; i++)
                {
                    battleManager.utility.SpawnDamageText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        states[i].Color, Color.white);
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

                CharacterBox box = BattleManager.Instance.characterBox.GetBox(character);

                if (healDif < 0)
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.red, Color.white);
                else if (healDif > 0)
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.green, Color.white);

                if (manaDif > 0)
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1.4f),
                                    manaDif.ToString(), Color.cyan, Color.white);

                for (int i = 0; i < states.Count; i++)
                {
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), states[i].Name,
                                                            states[i].Color, Color.white);
                }

                if (character.Heal <= 0)
                    battleManager.utility.FallCharacter(character);

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

            List<RPGEntityState> states = new List<RPGEntityState>();
            int healDif = 0, manaDif = 0;

            foreach (var effect in item.Effects)
            {
                yield return battleManager.pipeline.StartCoroutine(effect.BattleInvoke(user, target));

                if (effect.InfoIsExists("AddState"))
                    states.Add(effect["AddState"] as RPGEntityState);

                if (effect.InfoIsExists("Heal"))
                    healDif += (int)effect["Heal"];

                if (effect.InfoIsExists("Mana"))
                    manaDif += (int)effect["Mana"];
            }

            if (target is BattleEnemyInfo enemy)
            {
                if (!battleManager.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = BattleManager.Instance.enemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    battleManager.utility.SpawnDamageText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.red, Color.white);

                    model.Damage();
                }
                    
                else if (healDif > 0)
                    battleManager.utility.SpawnDamageText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.green, Color.white);

                for (int i = 0; i < states.Count; i++)
                {
                    battleManager.utility.SpawnDamageText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        states[i].Color, Color.white);
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

                CharacterBox box = BattleManager.Instance.characterBox.GetBox(character);

                if (healDif < 0)
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.red, Color.white);
                else if (healDif > 0)
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.green, Color.white);

                if (manaDif > 0)
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1.4f),
                                    manaDif.ToString(), Color.cyan, Color.white);

                for (int i = 0; i < states.Count; i++)
                {
                    battleManager.utility.SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), states[i].Name,
                                                            states[i].Color, Color.white);
                }

                if (character.Heal <= 0)
                    battleManager.utility.FallCharacter(character);

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

        gameManager.inventory.AddToItemCount(item, -1);
    }
}
