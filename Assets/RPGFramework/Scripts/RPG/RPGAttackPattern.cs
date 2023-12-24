using System.Collections;
using UnityEngine;

public abstract class RPGAttackPattern : MonoBehaviour
{
    [Tooltip("������������ �������� �� ������")]
    public float PatternTime = 1f;

    [Tooltip("������� ���������� ��������?")]
    public bool WaitEnd = false;

    protected bool isWorking = false;
    /// <summary>
    /// ������� �������� ������?
    /// </summary>
    public bool IsWorking => isWorking;

    /// <summary>
    /// �������� ��������
    /// </summary>
    [HideInInspector]
    public RPGEnemy enemy;

    /// <summary>
    /// ��������� �������
    /// </summary>
    /// <param name="tiny">����������� ��� ���</param>
    public void Invoke(bool tiny = false) => StartCoroutine(MainPatternCoroutine(tiny));

    /// <summary>
    /// ������ ������ ��������
    /// </summary>
    /// <param name="obj">������</param>
    /// <param name="offset">������ �� ������</param>
    public GameObject CreateObject(GameObject obj, Vector2 offset)
    {
        PatternBullet pb;

        obj.TryGetComponent(out pb);

        // ���� ���� �������� PatternBullet �� ������������� ��� ��� ���������
        if (pb != null)
            pb.enemy = enemy;
        else
            pb.enemy = BattleManager.Data.Enemys[0].Enemy;


        return BattleManager.Instance.pattern.CreatePatternObject(obj, offset);
    }

    /// <summary>
    /// �������� ��������
    /// </summary>
    protected abstract IEnumerator PatternCoroutine();
    /// <summary>
    /// �������� ������������ �������� (�� �����������)
    /// </summary>
    protected virtual IEnumerator TinyPatternCoroutine()
    {
        yield return StartCoroutine(PatternCoroutine());
    }

    /// <summary>
    /// �������� �������� ���������� ��������
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
