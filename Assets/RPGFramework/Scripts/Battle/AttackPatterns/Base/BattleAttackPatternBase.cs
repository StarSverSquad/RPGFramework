using RPGF.Battle.Pattern;
using RPGF.RPG;
using System;
using System.Collections;
using UnityEngine;

public abstract class BattleAttackPatternBase : RPGFrameworkBehaviour
{
    public string PatternTag = "";

    [Tooltip("Длительность паттерна во времни")]
    public float PatternTime = 1f;
    [Tooltip("Ожидать завершения паттерна?")]
    public bool WaitEnd = false;

    protected bool isWorking = false;
    public bool IsWorking => isWorking;

    protected Vector2 BattleFieldPosition => Battle.BattleField.transform.position;

    [HideInInspector]
    public RPGEnemy enemy;

    public void Invoke(bool tiny = false) => StartCoroutine(MainPatternCoroutine(tiny));

    protected GameObject CreateObjectRelativeCenter(GameObject obj, Vector2 offset)
    {
        PatternBulletBase pb;

        if (obj.TryGetComponent(out pb))
            pb.enemy = enemy ?? BattleManager.Data.Enemys[0];

        return BattleManager.Instance.Pattern.CreateObjectRelativeCenter(obj, offset);
    }
    protected GameObject CreateObjectRelativeBattleField(GameObject obj, Vector2 offset)
    {
        PatternBulletBase pb;

        if (obj.TryGetComponent(out pb))
            pb.enemy = enemy ?? BattleManager.Data.Enemys[0];

        return BattleManager.Instance.Pattern.CreateObjectRelativeBattleField(obj, offset);
    }
    protected GameObject CreateObjectInWorldSpace(GameObject obj, Vector2 position)
    {
        PatternBulletBase pb;

        if (obj.TryGetComponent(out pb))
            pb.enemy = enemy ?? BattleManager.Data.Enemys[0];

        return BattleManager.Instance.Pattern.CreateObjectInWorldSpace(obj, position);
    }

    protected abstract IEnumerator PatternCoroutine();
    protected virtual IEnumerator TinyPatternCoroutine()
    {
        yield return StartCoroutine(PatternCoroutine());
    }

    private IEnumerator MainPatternCoroutine(bool tiny)
    {
        isWorking = true;

        if (tiny)
            yield return StartCoroutine(TinyPatternCoroutine());
        else 
            yield return StartCoroutine(PatternCoroutine());

        isWorking = false;
    }
}
