using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyModel : MonoBehaviour 
{
    [Serializable]
    public struct ModelAnimator
    {
        public Animator Animator;
        public string Tag;
    }

    [Header("Ссылки")]
    [SerializeField]
    private GameObject modelContainer;
    [SerializeField]
    private IconList iconList;

    [SerializeField]
    private RectTransform damageTxtPoint;
    public Vector2 DamageTextGlobalPoint => damageTxtPoint.transform.position;

    [SerializeField]
    private RectTransform attackPoint;
    public Vector2 AttackGlobalPoint => attackPoint.transform.position;

    [Header("Еффекты")]
    [SerializeField]
    private VisualDeathEffectBase deathEffect;
    [SerializeField]
    private VisualDamageEffectBase damageEffect;

    [Header("Аниматоры")]
    [SerializeField]
    private List<ModelAnimator> animators = new List<ModelAnimator>();

    public bool IsAnimatingEffect => damageEffect.IsAnimating || deathEffect.IsAnimating;

    private BattleEnemyInfo enemy;

    private bool isInit = false;

    public void Initialize(BattleEnemyInfo enemy)
    {
        this.enemy = enemy;

        enemy.Entity.OnStateChanged += UpdateStats;

        UpdateStats(null);

        isInit = true;
    }

    public void UpdateStats(RPGEntityState state)
    {
        iconList.UpdateIcons(enemy.States.Select(i => i.Icon).ToArray());
    }

    public void Damage()
    {
        if (damageEffect != null) 
            damageEffect.Invoke();
    }

    public void Death()
    {
        if (deathEffect != null) 
            deathEffect.Invoke();
    }

    public void Cleanup()
    {
        if (damageEffect != null)
            damageEffect.Cleanup();

        if (deathEffect != null)
            deathEffect.Cleanup();
    }

    public Animator GetAnimator(string tag) => animators.First(a => a.Tag == tag).Animator;

    private void OnDestroy()
    {
        if (isInit)
        {
            enemy.Entity.OnStateChanged -= UpdateStats;
            isInit = false;
        }
    }
}