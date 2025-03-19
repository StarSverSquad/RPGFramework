using RPGF.RPG;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[Obsolete]
public class BattleUsingService
{
    private BattleManager battleManager;
    private GameManager gameManager;

    public BattleUsingService(BattleManager battleManager, GameManager gameManager)
    {
        this.battleManager = battleManager;
        this.gameManager = gameManager;
    }

    [Obsolete]
    public IEnumerator UseAbility(RPGAbility ability, RPGEntity user, params RPGEntity[] targets) 
    {
        float minigameFactor = 1f;

        if (ability.Minigame != null)
        {
            battleManager.Minigame.InvokeMinigame(ability.Minigame);

            yield return new WaitWhile(() => battleManager.Minigame.MinigameIsPlay);

            minigameFactor = battleManager.Minigame.LastWinFactor;
        }

        if (targets.Any(i => i is RPGEnemy) && ability.VisualEffect != null)
        {
            EnemyModel model = BattleManager.Instance.EnemyModels.GetModel(targets.Where(i => i is RPGEnemy).First() as RPGEnemy);

            VisualAttackEffect effect;

            if (targets.Where(i => i is RPGEnemy).Count() == 1)
                effect = battleManager.Utility.SpawnAttackEffect(ability.VisualEffect);
            else 
                effect = battleManager.Utility.SpawnAttackEffect(ability.VisualEffect, model.AttackGlobalPoint);

            effect.Invoke();

            yield return new WaitWhile(() => effect.IsAnimating);

            yield return new WaitForSeconds(0.5f);

            GameObject.Destroy(effect.gameObject);
        }

        foreach (var target in targets)
        {
            if (target is RPGCharacter chr)
            {
                var turnData = BattleManager.Instance.data.TurnsData.First(i => i.Character == chr);

                if ((turnData.IsDead && !ability.ForDeath) || (!turnData.IsDead && !ability.ForAlive))
                {
                    continue;
                }
            }

            int oldHp = target.Heal;
            int oldMp = target.Mana;
            string[] oldStates = target.States.Select(x => x.Tag).ToArray();

            foreach (var effect in ability.Effects)
            {
                effect.Factor = minigameFactor;

                yield return BattleManager.Instance.StartCoroutine(effect.Invoke(user, target));
            }

            RPGEntityState[] states = target.States.Where(i => oldStates.All(y => i.Tag != y)).ToArray();
            
            
            if (ability.Formula >= 0)
                target.Heal += Mathf.RoundToInt(ability.Formula * minigameFactor);
            else
                target.GiveDamage(Mathf.RoundToInt((Mathf.Abs(ability.Formula) + user.Damage * 0.25f) * minigameFactor));

            int healDif = target.Heal - oldHp, manaDif = target.Mana - oldMp;

            if (target is RPGEnemy enemy)
            {
                if (!BattleManager.Instance.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = BattleManager.Instance.EnemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    battleManager.Utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);

                    model.Damage();
                }
                else if (healDif > 0)
                {
                    battleManager.Utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.white, Color.green);
                }
                

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.Utility.SpawnFallingText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        Color.white, states[i].Color);
                }


                if (enemy.Heal <= 0)
                {
                    BattleManager.Instance.BattleAudio.PlaySound(battleManager.data.EnemyDeath);
                    model.Death();
                }
                else if (healDif < 0)
                    battleManager.BattleAudio.PlaySound(battleManager.data.EnemyDamage);
                else
                    battleManager.BattleAudio.PlaySound(battleManager.data.Heal);
            }
            else if (target is RPGCharacter character)
            {
                var turnData = battleManager.data.TurnsData.First(i => i.Character == character);

                CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(character);

                if (healDif < 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);
                else if (healDif > 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.white, Color.green);
                else if (target.Heal == target.MaxHeal)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    "MAX", Color.green);
                else if (ability.Formula > 0 && healDif == 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    "0", Color.green);

                if (manaDif > 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.5f),
                                    manaDif.ToString(), Color.white, Color.cyan);
                else if (target.Mana == target.MaxMana)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.5f),
                                    "MAX", Color.cyan);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), states[i].Name,
                                                            Color.white, states[i].Color);
                }

                if (character.Heal <= 0)
                    character.Heal = 1;

                if (ability.WakeupCharacter && turnData.IsDead)
                {
                    box.SetDead(false);

                    turnData.IsDead = false;

                    if (character.Heal <= 0)
                        character.Heal = 1;
                }

                if (healDif < 0)
                    battleManager.BattleAudio.PlaySound(battleManager.data.Hurt);
                else
                    battleManager.BattleAudio.PlaySound(battleManager.data.Heal);
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    [Obsolete]
    public IEnumerator UseItem(RPGConsumed item, RPGEntity user, params RPGEntity[] targets)
    {
        if (targets.Any(i => i is RPGEnemy) && item.VisualEffect != null)
        {
            EnemyModel model = BattleManager.Instance.EnemyModels.GetModel(targets.Where(i => i is RPGEnemy).First() as RPGEnemy);

            VisualAttackEffect effect;

            if (targets.Where(i => i is RPGEnemy).Count() == 1)
                effect = battleManager.Utility.SpawnAttackEffect(item.VisualEffect);
            else
                effect = battleManager.Utility.SpawnAttackEffect(item.VisualEffect, model.AttackGlobalPoint);

            effect.Invoke();

            yield return new WaitWhile(() => effect.IsAnimating);

            yield return new WaitForSeconds(0.5f);

            GameObject.Destroy(effect.gameObject);
        }

        foreach (var target in targets)
        {
            if (target is RPGCharacter chr)
            {
                var turnData = battleManager.data.TurnsData.First(i => i.Character == chr);

                if ((turnData.IsDead && !item.ForDeath) || (!turnData.IsDead && !item.ForAlive))
                {
                    continue;
                }
            }

            int oldHp = target.Heal;
            int oldMp = target.Mana;
            string[] oldStates = target.States.Select(x => x.Tag).ToArray();

            foreach (var effect in item.Effects)
            {
                effect.Factor = 1f;

                yield return battleManager.StartCoroutine(effect.Invoke(user, target));
            }

            RPGEntityState[] states = target.States.Where(i => oldStates.All(y => i.Tag != y)).ToArray();
            int healDif = target.Heal - oldHp, manaDif = target.Mana - oldMp;

            if (target is RPGEnemy enemy)
            {
                if (!battleManager.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = BattleManager.Instance.EnemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    battleManager.Utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);

                    model.Damage();
                }
                    
                else if (healDif > 0)
                    battleManager.Utility.SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.white, Color.green);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.Utility.SpawnFallingText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        Color.white, states[i].Color);
                }


                if (enemy.Heal <= 0)
                {
                    BattleManager.Instance.BattleAudio.PlaySound(battleManager.data.EnemyDeath);
                    model.Death();
                }
                else if (healDif < 0)
                    battleManager.BattleAudio.PlaySound(battleManager.data.EnemyDamage);
                else
                    battleManager.BattleAudio.PlaySound(battleManager.data.Heal);
            }
            else if (target is RPGCharacter character)
            {
                var turnData = battleManager.data.TurnsData.First(i => i.Character == character);

                CharacterBox box = battleManager.UI.CharacterBox.GetBox(character);

                if (healDif < 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);
                else if (healDif > 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.white, Color.green);
                else
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                        "MAX", Color.green, Color.green);

                if (manaDif > 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.5f),
                                    manaDif.ToString(), Color.white, Color.cyan);
                else if (manaDif == 0)
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.5f),
                                    "MAX", Color.cyan, Color.cyan);

                for (int i = 0; i < states.Length; i++)
                {
                    battleManager.Utility.SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), states[i].Name,
                                                            Color.white, states[i].Color);
                }

                if (character.Heal <= 0)
                    character.Heal = 1;
                    

                if (item.WakeupCharacter && turnData.IsDead)
                {
                    box.SetDead(false);

                    turnData.IsDead = false;

                    if (character.Heal <= 0)
                        character.Heal = 1;
                }

                if (healDif < 0)
                    battleManager.BattleAudio.PlaySound(battleManager.data.Hurt);
                else
                    battleManager.BattleAudio.PlaySound(battleManager.data.Heal);
            }

            yield return new WaitForSeconds(.5f);
        }

        gameManager.Inventory.AddToItemCount(item, -1);
    }
}
