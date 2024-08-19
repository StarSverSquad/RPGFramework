using System.Collections;
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

        BattleManager.Instance.pipeline.InvokeMainPipeline();
    }

    public void StopBattle()
    {
        BattleManager.Instance.pipeline.InvokeBreak();
    }

    public void AddEnemy(RPGEnemy enemy, Vector2 position)
    {
        enemy.InitializeEntity();
        Data.Enemys.Add(new BattleEnemyInfo(enemy));
        BattleManager.Instance.enemyModels.AddModel(Data.Enemys.Last(), position);
    }

    public void RemoveEnemy(BattleEnemyInfo enemy)
    {
        if (!Data.Enemys.Contains(enemy))
            return;

        BattleManager.Instance.enemyModels.DeleteModel(enemy);

        Data.Enemys.Remove(enemy);
    }

    public void DamageEnemy(BattleCharacterInfo who, BattleEnemyInfo enemy, float damageFactor = 1f)
    {
        EnemyModel model = BattleManager.Instance.enemyModels.GetModel(enemy);

        int dmg = enemy.Entity.GiveDamage(who.Entity, damageFactor, true);

        enemy.Heal -= dmg;

        if (dmg > 0)
        {
            if (enemy.Heal <= 0)
            {
                model.Death();
                BattleManager.Instance.battleAudio.PlaySound(Data.EnemyDeath);
            }
            else
            {
                model.Damage();
                BattleManager.Instance.battleAudio.PlaySound(Data.EnemyDamage);
            }

            SpawnFallingText(model.DamageTextGlobalPoint, dmg.ToString(), Color.white, Color.red);
        }
        else
        {
            BattleManager.Instance.battleAudio.PlaySound(Data.Miss);
            SpawnFallingText(model.DamageTextGlobalPoint, "ПРОМАХ");
        }
    }

    public void SpawnFallingText(Vector2 position, string text)
    {
        GameObject obj = Instantiate(Data.DmgText.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);
        obj.transform.position = position;

        FallingText dmg = obj.GetComponent<FallingText>();

        dmg.Invoke(text);
    }
    public void SpawnFallingText(Vector2 position, string text, Color colorStart, Color colorEnd)
    {
        GameObject obj = Instantiate(Data.DmgText.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);
        obj.transform.position = position;

        FallingText dmg = obj.GetComponent<FallingText>();

        dmg.Invoke(text, colorStart, colorEnd);
    }

    /// <summary>
    /// Создаёт объект AttackEffect
    /// </summary>
    /// <param name="instance">Создоваеммый экземпляр</param>
    /// <param name="position">Позиция</param>
    /// <returns>Новый экземпляр</returns>
    public VisualAttackEffect SpawnAttackEffect(VisualAttackEffect instance, Vector2 position)
    {
        GameObject obj = Instantiate(instance.gameObject, position, Quaternion.identity, Data.BattleCanvas.transform);

        return obj.GetComponent<VisualAttackEffect>();
    }
    /// <summary>
    /// Создаёт объект AttackEffect
    /// </summary>
    /// <param name="instance">Создоваеммый экземпляр</param>
    /// <returns>Новый экземпляр</returns>
    public VisualAttackEffect SpawnAttackEffect(VisualAttackEffect instance)
    {
        GameObject obj = Instantiate(instance.gameObject, (Vector2)Data.BattleCanvas.transform.position + new Vector2(0, 0.5f), 
            Quaternion.identity, Data.BattleCanvas.transform);

        return obj.GetComponent<VisualAttackEffect>();
    }

    /// <summary>
    /// Даёт урон персонажу взависимости от пули
    /// </summary>
    /// <param name="character">Персонаж</param>
    /// <param name="bullet">Пуля</param>
    public void DamageCharacterByBullet(BattleCharacterInfo character, PatternBullet bullet)
    {
        CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(character);

        int realDamage = character.Entity.GiveDamage(Mathf.RoundToInt(bullet.enemy.Damage * bullet.DamageModifier * (character.IsDefence ? .5f : 1f)));

        BattleManager.Instance.Shaker.Shake(2);

        if (bullet.State != null)
        {
            character.Entity.AddState(bullet.State);
            SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 2f), bullet.State.Name,
                bullet.State.Color, Color.white);
        }       

        if (character.Entity.Heal <= 0)
        {
            FallCharacter(character);

            if (BattleManager.Instance.pipeline.IsEnemyTurn 
                && Data.Characters.Where(i => i.IsTarget == true).Count() == 0)
            {
                Data.Characters.Where(i => i.IsDead == false).ToList().ForEach(item =>
                {
                    item.IsTarget = true;
                    BattleManager.Instance.UI.CharacterBox.GetBox(item).MarkTarget(true);
                });
            }

            if (Data.Characters.Where(i => i.IsDead == false).Count() == 0)
            {
                BattleManager.Instance.pipeline.InvokeLose();
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

    public void FallCharacter(BattleCharacterInfo character)
    {
        CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(character);

        SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 1.4f), "ПАЛ");

        box.SetDead(true);
        box.MarkTarget(false);

        character.IsDead = true;
        character.IsTarget = false;

        character.Entity.RemoveAllStates();

        character.Entity.Heal = 0;
    }

    /// <summary>
    /// Измененяет значение концентрации
    /// </summary>
    public void AddConcetration(int value)
    {
        Data.Concentration = Mathf.Clamp(Data.Concentration + value, 0, Data.MaxConcentration);

        BattleManager.Instance.UI.Concentration.SetConcentration(Data.Concentration);
    }
}