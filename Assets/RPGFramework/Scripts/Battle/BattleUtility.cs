using RPGF.Battle.Pattern;
using RPGF.RPG;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BattleUtility
{
    private BattleManager _battle { get; set; }

    public BattleData Data => _battle.data;

    public BattleUtility(BattleManager battle)
    {
        _battle = battle;
    }

    #region BATTLE CONTROL

    public void StartBattle(RPGBattleInfo info)
    {
        Data.BattleInfo = info;

        foreach (var item in info.enemySquad.Enemies)
            AddEnemy(item.Enemy, item.ScreenPosition);

        BattleManager.Instance.Pipeline.InvokeMainPipeline();
    }
    public void StopBattle()
    {
        BattleManager.Instance.Pipeline.InvokeBreak();
    }

    #endregion

    #region ENEMY CONTROL

    public void AddEnemy(RPGEnemy enemy, Vector2 position)
    {
        var entity = Object.Instantiate(enemy);
        entity.InitializeEntity();

        Data.Enemys.Add(entity);

        _battle.EnemyModels.AddModel(Data.Enemys.Last(), position);
    }
    public void RemoveEnemy(RPGEnemy enemy)
    {
        if (!Data.Enemys.Contains(enemy))
            return;

        BattleManager.Instance.EnemyModels.DeleteModel(enemy);

        Data.Enemys.Remove(enemy);
    }
    public void DamageEnemy(RPGCharacter who, RPGEnemy enemy, float damageFactor = 1f)
    {
        EnemyModel model = BattleManager.Instance.EnemyModels.GetModel(enemy);

        int dmg = enemy.GiveDamage(who, damageFactor, true);

        enemy.Heal -= dmg;

        if (dmg > 0)
        {
            if (enemy.Heal <= 0)
            {
                model.Death();
                BattleManager.Instance.BattleAudio.PlaySound(Data.EnemyDeath);
            }
            else
            {
                model.Damage();
                BattleManager.Instance.BattleAudio.PlaySound(Data.EnemyDamage);
            }

            SpawnFallingText(model.DamageTextGlobalPoint, dmg.ToString(), Color.white, Color.red);
        }
        else
        {
            BattleManager.Instance.BattleAudio.PlaySound(Data.Miss);
            SpawnFallingText(model.DamageTextGlobalPoint, "ПРОМАХ");
        }
    }

    #endregion

    #region FALLING TEXT CONTROL

    public void SpawnFallingText(Vector2 position, string text)
    {
        SpawnFallingText(position, text, Color.white, Color.white);
    }
    public void SpawnFallingText(Vector2 position, string text, Color color)
    {
        SpawnFallingText(position, text, color, color);
    }
    public void SpawnFallingText(Vector2 position, string text, Color colorStart, Color colorEnd)
    {
        GameObject obj = Object.Instantiate(Data.DmgText.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);
        obj.transform.position = position;

        FallingText dmg = obj.GetComponent<FallingText>();

        dmg.Invoke(text, colorStart, colorEnd);
    }

    #endregion

    #region BATTLE EFFECT CONTROL

    public BattleAttackEffect SpawnAttackEffect(BattleAttackEffect instance, Vector2 position)
    {
        GameObject obj = Object.Instantiate(
            instance.gameObject, 
            position, 
            Quaternion.identity, 
            Data.BattleCanvas.transform);

        return obj.GetComponent<BattleAttackEffect>();
    }
    public BattleAttackEffect SpawnAttackEffect(BattleAttackEffect instance)
    {
        return SpawnAttackEffect(
            instance, 
            (Vector2)Data.BattleCanvas.transform.position + new Vector2(0, 0.5f)
            );
    }

    #endregion

    public void DamageCharacterByBullet(BattleTurnData data, PatternBulletBase bullet)
    {
        CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(data.Character);

        float defenceAspect = data.BattleAction == BattleTurnData.TurnAction.Defence ? .5f : 1f;

        int realDamage = data.Character.GiveDamage(Mathf.RoundToInt(bullet.enemy.Damage * bullet.DamageModifier * defenceAspect));

        BattleManager.Instance.Shaker.Shake(2);

        float stateTextOffset = 2f;
        foreach (var state in bullet.States)
        {
            data.Character.AddState(state);

            SpawnFallingText(
                (Vector2)box.transform.position + new Vector2(0, stateTextOffset), 
                state.Name,
                Color.white, 
                state.Color);

            stateTextOffset += 0.5f;
        }    

        if (data.Character.Heal <= 0)
        {
            FallCharacter(data);

            if (BattleManager.Instance.Pipeline.IsEnemyTurn 
                && Data.TurnsData.Where(i => i.IsTarget == true).Count() == 0)
            {
                Data.TurnsData.Where(i => i.IsDead == false).ToList().ForEach(item =>
                {
                    item.IsTarget = true;
                    BattleManager.Instance.UI.CharacterBox.GetBox(item.Character).MarkTarget(true);
                });
            }

            if (Data.TurnsData.Where(i => i.IsDead == false).Count() == 0)
            {
                BattleManager.Instance.Pipeline.InvokeLose();
            }
        }
        else
        {
            if (realDamage > 0)
                SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f), realDamage.ToString(), Color.white, Color.red);
            else
                SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f), "ПРОМАХ");
        }
            
    }

    public void FallCharacter(BattleTurnData data)
    {
        CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(data.Character);

        SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.4f), "ПАЛ");

        box.SetDead(true);
        box.MarkTarget(false);

        data.IsDead = true;
        data.IsTarget = false;

        data.Character.RemoveAllStates();

        data.Character.Heal = 0;
    }

    public void AddConcetration(int value)
    {
        Data.Concentration = Mathf.Clamp(Data.Concentration + value, 0, Data.MaxConcentration);

        _battle.UI.Concentration.SetConcentration(Data.Concentration);
    }

    /// <summary>
    /// Нужен для спользования RPGUsable в рамках битвы.
    /// Только для RPGAbility или RPGConsumed
    /// </summary>
    /// <remarks>Название ГОВНО. Метод каловый, требует дороботки!!!</remarks>
    /// <returns>Курутина с полной обработкой</returns>
    public IEnumerator UseUsableTo(RPGUsable usable, RPGEntity user, params RPGEntity[] targets)
    {
        RPGAbility ability = usable is RPGAbility ? usable as RPGAbility : null;
        RPGConsumed consumed = usable is RPGConsumed ? usable as RPGConsumed : null;

        if (ability == null && consumed == null)
            yield break;

        float minigameFactor = 1f;

        // Если способность запускает миниигру, если она есть
        if (ability != null && ability.Minigame != null)
        {
            _battle.Minigame.InvokeMinigame(ability.Minigame);

            yield return new WaitWhile(() => _battle.Minigame.MinigameIsPlay);
            minigameFactor = _battle.Minigame.LastWinFactor;
        }

        // Запускает еффекты битвы на врагах
        if (targets.Any(i => i is RPGEnemy) && usable.VisualEffect != null)
        {
            EnemyModel model = _battle.EnemyModels.GetModel(targets.Where(i => i is RPGEnemy).First() as RPGEnemy);

            BattleAttackEffect effect;

            if (targets.Where(i => i is RPGEnemy).Count() == 1)
                effect = SpawnAttackEffect(usable.VisualEffect);
            else
                effect = SpawnAttackEffect(usable.VisualEffect, model.AttackGlobalPoint);

            effect.Invoke();

            yield return new WaitWhile(() => effect.IsAnimating);

            yield return new WaitForSeconds(0.5f);

            Object.Destroy(effect.gameObject);
        }

        // Проход по целям
        foreach (var target in targets)
        {
            if (target is RPGCharacter chr)
            {
                var turnData = _battle.data.TurnsData.First(i => i.Character == chr);

                if ((turnData.IsDead && !usable.ForDeath) || (!turnData.IsDead && !usable.ForAlive))
                {
                    continue;
                }
            }

            int oldHp = target.Heal;
            int oldMp = target.Mana;
            string[] oldStates = target.States.Select(x => x.Tag).ToArray();

            foreach (var effect in usable.Effects)
            {
                effect.Factor = minigameFactor;

                yield return _battle.StartCoroutine(effect.Invoke(user, target));
            }

            RPGEntityState[] states = target.States.Where(i => oldStates.All(y => i.Tag != y)).ToArray();

            // Если способность нужно расчитать урон/лечение по формуле
            if (ability != null)
            {
                if (ability.Formula >= 0)
                    target.Heal += Mathf.RoundToInt(ability.Formula * minigameFactor);
                else
                    target.GiveDamage(Mathf.RoundToInt((Mathf.Abs(ability.Formula) + user.Damage * 0.25f) * minigameFactor));
            }

            int healDif = target.Heal - oldHp, manaDif = target.Mana - oldMp;

            if (target is RPGEnemy enemy)
            {
                if (!_battle.data.Enemys.Contains(enemy))
                    continue;

                EnemyModel model = _battle.EnemyModels.GetModel(enemy);

                if (healDif < 0)
                {
                    SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);

                    model.Damage();
                }
                else if (healDif > 0)
                {
                    SpawnFallingText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    healDif.ToString(), Color.white, Color.green);
                }


                for (int i = 0; i < states.Length; i++)
                {
                    SpawnFallingText(model.AttackGlobalPoint + new Vector2(0, 0.2f + (0.2f * i)), states[i].Name,
                        Color.white, states[i].Color);
                }


                if (enemy.Heal <= 0)
                {
                    _battle.BattleAudio.PlaySound(_battle.data.EnemyDeath);
                    model.Death();
                }
                else if (healDif < 0)
                    _battle.BattleAudio.PlaySound(_battle.data.EnemyDamage);
                else
                    _battle.BattleAudio.PlaySound(_battle.data.Heal);
            }
            else if (target is RPGCharacter character)
            {
                var turnData = _battle.data.TurnsData.First(i => i.Character == character);

                CharacterBox box = _battle.UI.CharacterBox.GetBox(character);

                if (healDif < 0)
                    SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    Mathf.Abs(healDif).ToString(), Color.white, Color.red);
                else if (healDif > 0)
                    SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    healDif.ToString(), Color.white, Color.green);
                else if (target.Heal == target.MaxHeal)
                    SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1f),
                                    "MAX", Color.green);
                else
                {
                    if (ability != null && ability.Formula > 0 && healDif == 0)
                    {
                        SpawnFallingText(
                            (Vector2)box.transform.position + new Vector2(0, 1f),
                            "0",
                            Color.green);
                    }
                }

                if (manaDif > 0)
                    SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.5f),
                                    manaDif.ToString(), Color.white, Color.cyan);
                else if (target.Mana == target.MaxMana)
                    SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.5f),
                                    "MAX", Color.cyan);

                for (int i = 0; i < states.Length; i++)
                {
                    SpawnFallingText(
                        (Vector2)box.transform.position + new Vector2(0, 2f + (0.2f * i)), 
                        states[i].Name,                   
                        Color.white, 
                        states[i].Color);
                }

                if (character.Heal <= 0)
                    character.Heal = 1;

                if (usable.WakeupCharacter && turnData.IsDead)
                {
                    box.SetDead(false);

                    turnData.IsDead = false;

                    if (character.Heal <= 0)
                        character.Heal = 1;
                }

                if (healDif < 0)
                    _battle.BattleAudio.PlaySound(_battle.data.Hurt);
                else
                    _battle.BattleAudio.PlaySound(_battle.data.Heal);
            }

            yield return new WaitForSeconds(.5f);
        }

        // Если потребляемый предмет то нужно забрать из инвентаря 1 еденицу
        if (consumed != null)
            GameManager.Instance.Inventory.AddToItemCount(consumed, -1);
    }
}