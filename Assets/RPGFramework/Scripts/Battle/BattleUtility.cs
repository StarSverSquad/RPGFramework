﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class BattleUtility : MonoBehaviour
{
    public BattleData Data => BattleManager.Data;

    public void StartBattle(RPGBattleInfo info)
    {
        Data.BattleInfo = info;

        foreach (var item in info.enemySquad.Enemies)
            AddEnemy(item.Enemy, item.ScreenPosition);

        BattleManager.instance.pipeline.InvokeMainPipeline();
    }

    public void StopBattle()
    {
        CleanupBattle();

        foreach (var item in GameManager.Instance.characterManager.characters)
            item.UpdateStats();
    }

    public void CleanupBattle()
    {
        Data.Concentration = 0;
        Data.BattleInfo = null;

        Data.Characters.Clear();
        Data.Enemys.Clear();

        BattleManager.instance.SetActive(false);
    }

    public void AddEnemy(RPGEnemy enemy, Vector2 position)
    {
        enemy.InitializeEntity();
        Data.Enemys.Add(new BattleEnemyInfo(enemy));
        BattleManager.instance.enemyModels.AddModel(Data.Enemys.Last(), position);
    }

    public void RemoveEnemy(BattleEnemyInfo enemy)
    {
        if (!Data.Enemys.Contains(enemy))
            return;

        BattleManager.instance.enemyModels.DeleteModel(enemy);

        Data.Enemys.Remove(enemy);
    }

    public void DamageEnemy(BattleCharacterInfo who, BattleEnemyInfo enemy, float damageFactor = 1f)
    {
        EnemyModel model = BattleManager.instance.enemyModels.GetModel(enemy);

        int dmg = enemy.Entity.GiveDamage(Mathf.RoundToInt(who.Entity.Damage * damageFactor), true);

        enemy.Heal -= dmg;

        if (dmg > 0)
        {
            if (enemy.Heal <= 0)
            {
                model.Death();
                BattleManager.instance.battleAudio.PlaySound(Data.EnemyDeath);
            }
            else
            {
                model.Damage();
                BattleManager.instance.battleAudio.PlaySound(Data.EnemyDamage);
            }

            SpawnDamageText(model.DamageTextGlobalPoint, dmg);
        }
        else
        {
            BattleManager.instance.battleAudio.PlaySound(Data.Miss);
            SpawnDamageText(model.DamageTextGlobalPoint, "ПРОМАХ");
        }
    }

    public void ConsumeItem(RPGConsumed item, params BattleEntityInfo[] entitys)
    {
        foreach (var info in entitys)
        {
            if (item.AddHeal != 0)
                info.Heal += item.AddHeal;
            if (item.AddMana != 0)
                info.Mana += item.AddMana;

            if (info is BattleCharacterInfo character)
            {
                CharacterBox box = BattleManager.instance.characterBox.GetBox(character);

                if (item.AddConcentration != 0)
                    AddConcetration(item.AddConcentration);

                if (item.AddHeal < 0)
                    SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 2.6f),
                                    Mathf.Abs(item.AddHeal).ToString(), Color.red, Color.white);
                else if (item.AddHeal > 0)
                    SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 2.6f),
                                    item.AddHeal.ToString(), Color.green, Color.white);

                if (item.AddMana > 0)
                    SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 3f),
                                    item.AddHeal.ToString(), Color.cyan, Color.white);

                if (character.Heal <= 0)
                    FallCharacter(character);

                if (item.CharacterBecomeAlive && character.IsDead)
                {
                    box.SetDead(false);

                    character.IsDead = false;

                    if (info.Heal <= 0)
                        info.Heal = 1;
                }
            }
            else if (info is BattleEnemyInfo enemy)
            {
                EnemyModel model = BattleManager.instance.enemyModels.GetModel(enemy);

                if (item.AddHeal < 0)
                    SpawnDamageText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    Mathf.Abs(item.AddHeal).ToString(), Color.red, Color.white);
                else if (item.AddHeal > 0)
                    SpawnDamageText((Vector2)model.transform.position + new Vector2(0, 0.5f),
                                    item.AddHeal.ToString(), Color.green, Color.white);

                if (enemy.Heal <= 0)
                {
                    model.Death();
                    BattleManager.instance.battleAudio.PlaySound(Data.EnemyDeath);
                }
            }
        }

        if (item.AddHeal < 0)
            BattleManager.instance.battleAudio.PlaySound(Data.Hurt);
        else if (item.AddHeal > 0 || item.AddMana > 0
                || item.AddConcentration > 0 || item.CharacterBecomeAlive)
            BattleManager.instance.battleAudio.PlaySound(Data.Heal);

        GameManager.Instance.inventory.AddToItemCount(item, -1);
    }

    public void SpawnDamageText(Vector2 position, int damage)
    {
        GameObject obj = Instantiate(Data.DmgText.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);
        obj.transform.position = position;

        DamageText dmg = obj.GetComponent<DamageText>();

        dmg.Invoke(damage);
    }
    public void SpawnDamageText(Vector2 position, string text)
    {
        GameObject obj = Instantiate(Data.DmgText.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);
        obj.transform.position = position;

        DamageText dmg = obj.GetComponent<DamageText>();

        dmg.OutputSimpleText(text);
    }
    public void SpawnDamageText(Vector2 position, string text, Color gradientIn, Color gradientOut)
    {
        GameObject obj = Instantiate(Data.DmgText.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);
        obj.transform.position = position;

        DamageText dmg = obj.GetComponent<DamageText>();

        dmg.OutputSimpleText(text, gradientIn, gradientOut);
    }

    /// <summary>
    /// Создаёт объект AttackEffect
    /// </summary>
    /// <param name="instance">Создоваеммый экземпляр</param>
    /// <param name="position">Позиция</param>
    /// <returns>Новый экземпляр</returns>
    public AttackEffect SpawnAttackEffect(AttackEffect instance, Vector2 position)
    {
        GameObject obj = Instantiate(instance.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);

        obj.transform.position = position;

        return obj.GetComponent<AttackEffect>();
    }

    /// <summary>
    /// Даёт урон персонажу взависимости от пули
    /// </summary>
    /// <param name="character">Персонаж</param>
    /// <param name="bullet">Пуля</param>
    public void DamageCharacter(BattleCharacterInfo character, PatternBullet bullet)
    {
        CharacterBox box = BattleManager.instance.characterBox.GetBox(character);

        int total = bullet.AdditionDamage + bullet.enemy.Damage;
        int damage = Mathf.RoundToInt((Mathf.RoundToInt(Random.Range(total * 0.75f, total * 1.25f)) - Mathf.FloorToInt(character.Entity.Defence / 2f)) / (character.IsDefence ? 2 : 1));

        character.Damage(damage);

        if (bullet.State != null && character.States.All(i => i.rpg != bullet.State))
        {
            character.AddState(bullet.State);
            SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 2.6f), bullet.State.Name);
        }       

        if (character.Entity.Heal <= 0)
        {
            FallCharacter(character);

            if (BattleManager.instance.pipeline.IsEnemyTurn 
                && Data.Characters.Where(i => i.IsTarget == true).Count() == 0)
            {
                Data.Characters.Where(i => i.IsDead == false).ToList().ForEach(item =>
                {
                    item.IsTarget = true;
                    BattleManager.instance.characterBox.GetBox(item).MarkTarget(true);
                });
            }

            if (Data.Characters.Where(i => i.IsDead == false).Count() == 0)
            {
                BattleManager.instance.pipeline.InvokeLose();
            }
        }
        else
            SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1.4f), damage);
    }

    public void FallCharacter(BattleCharacterInfo character)
    {
        CharacterBox box = BattleManager.instance.characterBox.GetBox(character);

        SpawnDamageText((Vector2)box.transform.position + new Vector2(0, 1.4f), "ПАЛ");

        box.SetDead(true);
        box.MarkTarget(false);

        character.IsDead = true;
        character.IsTarget = false;

        character.RemoveAllStates();

        character.Entity.Heal = 0;
    }

    /// <summary>
    /// Измененяет значение концентрации
    /// </summary>
    public void AddConcetration(int value)
    {
        Data.Concentration = Mathf.Clamp(Data.Concentration + value, 0, Data.MaxConcentration);

        BattleManager.instance.concentrationBar.UpdateValue();
    }
}