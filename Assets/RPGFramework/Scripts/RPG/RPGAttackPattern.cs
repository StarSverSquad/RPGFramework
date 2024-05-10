using System;
using System.Collections;
using UnityEngine;

public abstract class RPGAttackPattern : MonoBehaviour
{
    [Tooltip("Длительность паттерна во времни")]
    public float PatternTime = 1f;

    [Tooltip("Ожидать завершения паттерна?")]
    public bool WaitEnd = false;

    protected bool isWorking = false;
    /// <summary>
    /// Паттерн работает сейчас?
    /// </summary>
    public bool IsWorking => isWorking;

    protected Vector2 BattleFieldPosition => BattleManager.Instance.battleField.transform.position;

    [HideInInspector]
    public RPGEnemy enemy;

    public void Invoke(bool tiny = false) => StartCoroutine(MainPatternCoroutine(tiny));


    [Obsolete]
    protected GameObject CreateObject(GameObject obj, Vector2 offset)
    {
        return CreateObjectRelativeCenter(obj, offset);
    }

    protected GameObject CreateObjectRelativeCenter(GameObject obj, Vector2 offset)
    {
        PatternBullet pb;

        if (obj.TryGetComponent(out pb))
            pb.enemy = enemy ?? BattleManager.Data.Enemys[0].Enemy;

        return BattleManager.Instance.pattern.CreateObjectRelativeCenter(obj, offset);
    }
    protected GameObject CreateObjectRelativeBattleField(GameObject obj, Vector2 offset)
    {
        PatternBullet pb;

        if (obj.TryGetComponent(out pb))
            pb.enemy = enemy ?? BattleManager.Data.Enemys[0].Enemy;

        return BattleManager.Instance.pattern.CreateObjectRelativeBattleField(obj, offset);
    }
    protected GameObject CreateObjectInWorldSpace(GameObject obj, Vector2 position)
    {
        PatternBullet pb;

        if (obj.TryGetComponent(out pb))
            pb.enemy = enemy ?? BattleManager.Data.Enemys[0].Enemy;

        return BattleManager.Instance.pattern.CreateObjectInWorldSpace(obj, position);
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
