using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyModel : MonoBehaviour 
{
    [SerializeField]
    private VisualDeathEffectBase deathEffect;
    [SerializeField]
    private VisualDamageEffectBase damageEffect;

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

    public bool IsAnimating => damageEffect.IsAnimating || deathEffect.IsAnimating;

    private BattleEnemyInfo enemy;

    private bool isInit = false;

    public void Initialize(BattleEnemyInfo enemy)
    {
        this.enemy = enemy;

        enemy.StatesChanged += UpdateStats;

        UpdateStats();

        isInit = true;
    }

    public void UpdateStats()
    {
        iconList.UpdateIcons(enemy.States.Select(i => i.rpg.Icon).ToArray());
    }

    public void Damage()
    {
        damageEffect.Invoke();
    }

    public void Death()
    {
        deathEffect.Invoke();
    }

    public void Cleanup()
    {
        damageEffect.Cleanup();
        deathEffect.Cleanup();
    }

    private void OnDestroy()
    {
        if (isInit)
        {
            enemy.StatesChanged -= UpdateStats;
            isInit = false;
        }
    }
}