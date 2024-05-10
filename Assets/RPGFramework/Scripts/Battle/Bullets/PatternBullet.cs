using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PatternBullet : MonoBehaviour
{
    public int DamageModifier = 1;

    public int AdditionConcentration = 4;

    public bool CanHitBorder = true;
    public bool DestroyAfterHit = true;
    public bool IgnoreHitCooldown = false;
    public bool AddState = false;

    public RPGEntityState State;

    [HideInInspector]
    public RPGEnemy enemy;

    public bool IsHitBorder { get; set; } = false;

    /// <summary>
    /// Вызывается при попадании
    /// </summary>
    public virtual void OnHit() { }

    /// <summary>
    /// Вызывается при попадании во время кулдауна (Не работает для тех пуль у которых IgnoreHitCooldown)
    /// </summary>
    public virtual void OnCooldownHit() { }

    /// <summary>
    /// Вызывается при попадании в бордер (Не работает для тех пуль где !IsCanHitBorder)
    /// </summary>
    public virtual void OnHitBorder() { }

    private void OnDestroy()
    {
        foreach (var item in GetComponents<Component>())
            item.DOKill(false);
    }
}