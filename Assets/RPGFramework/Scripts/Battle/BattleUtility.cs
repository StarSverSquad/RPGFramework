using System.Linq;
using UnityEngine;

public class BattleUtility : RPGFrameworkBehaviour
{
    public BattleData Data => BattleManager.Data;

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

    public void AddEnemy(RPGEnemy enemy, Vector2 position)
    {
        var entity = Instantiate(enemy);
        entity.InitializeEntity();

        Data.Enemys.Add(entity);

        Battle.EnemyModels.AddModel(Data.Enemys.Last(), position);
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
    /// <param name="data">Данный хода</param>
    /// <param name="bullet">Пуля</param>
    public void DamageCharacterByBullet(BattleTurnData data, PatternBullet bullet)
    {
        CharacterBox box = BattleManager.Instance.UI.CharacterBox.GetBox(data.Character);

        float defenceAspect = data.BattleAction == BattleTurnData.TurnAction.Defence ? .5f : 1f;

        int realDamage = data.Character.GiveDamage(Mathf.RoundToInt(bullet.enemy.Damage * bullet.DamageModifier * defenceAspect));

        BattleManager.Instance.Shaker.Shake(2);

        if (bullet.State != null)
        {
            data.Character.AddState(bullet.State);
            SpawnFallingText((Vector2)box.transform.position + new Vector2(0, 2f), bullet.State.Name,
                bullet.State.Color, Color.white);
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

    /// <summary>
    /// Измененяет значение концентрации
    /// </summary>
    public void AddConcetration(int value)
    {
        Data.Concentration = Mathf.Clamp(Data.Concentration + value, 0, Data.MaxConcentration);

        BattleManager.Instance.UI.Concentration.SetConcentration(Data.Concentration);
    }
}