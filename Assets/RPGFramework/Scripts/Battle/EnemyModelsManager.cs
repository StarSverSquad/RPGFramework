using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyModelsManager : MonoBehaviour, IDisposable
{
    public struct ModelInfo
    {
        public EnemyModel model;
        public RPGEnemy enemy;
    }

    private List<ModelInfo> models = new List<ModelInfo>();

    public bool IsAnyAnimating => models.Any(i => i.model.IsAnimatingEffect);

    public void AddModel(RPGEnemy enemy, Vector2 anposition)
    {
        if (HasModel(enemy))
            return;

        GameObject obj = Instantiate(enemy.EnemyModel, transform, false);

        RectTransform rect = obj.GetComponent<RectTransform>();
        EnemyModel mod = obj.GetComponent<EnemyModel>();

        rect.anchoredPosition = anposition;

        mod.Initialize(enemy);

        models.Add(new ModelInfo { model = mod, enemy = enemy });
    }

    public void DeleteModel(RPGEnemy enemy)
    {
        if (!HasModel(enemy)) 
            return; 

        ModelInfo info = models.First(i => i.enemy == enemy);
        models.Remove(info);

        Destroy(info.model.gameObject);
    }

    public bool HasModel(RPGEnemy enemy)
    {
        foreach (var item in models)
        {
            if (enemy == item.enemy) 
                return true;
        }

        return false;
    }

    public EnemyModel GetModel(RPGEnemy enemy)
    {
        if (!HasModel(enemy))
            return null;

        return models.First(i => i.enemy == enemy).model;
    }

    public void Dispose()
    {
        foreach (var model in models)
            Destroy(model.model.gameObject);
        models.Clear();
    }
}