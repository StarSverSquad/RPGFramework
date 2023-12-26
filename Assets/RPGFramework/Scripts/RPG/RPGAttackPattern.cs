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

    /// <summary>
    /// Владелец паттерна
    /// </summary>
    [HideInInspector]
    public RPGEnemy enemy;

    /// <summary>
    /// Запускает паттерн
    /// </summary>
    /// <param name="tiny">Сокращённый или нет</param>
    public void Invoke(bool tiny = false) => StartCoroutine(MainPatternCoroutine(tiny));

    /// <summary>
    /// Создаёт объект паттерна
    /// </summary>
    /// <param name="obj">Объект</param>
    /// <param name="offset">Отступ от центра</param>
    public GameObject CreateObject(GameObject obj, Vector2 offset)
    {
        PatternBullet pb;

        obj.TryGetComponent(out pb);

        // Если есть копонент PatternBullet то устанавливает ему его владельца
        if (pb != null)
            pb.enemy = enemy;
        else
            pb.enemy = BattleManager.Data.Enemys[0].Enemy;


        return BattleManager.Instance.pattern.CreatePatternObject(obj, offset);
    }

    /// <summary>
    /// Курутина паттерна
    /// </summary>
    protected abstract IEnumerator PatternCoroutine();
    /// <summary>
    /// Курутина сокращённого паттерна (Не обязательна)
    /// </summary>
    protected virtual IEnumerator TinyPatternCoroutine()
    {
        yield return StartCoroutine(PatternCoroutine());
    }

    /// <summary>
    /// Основная курутина выполнения паттерна
    /// </summary>
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
