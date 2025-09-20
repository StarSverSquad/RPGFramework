using RPGF.RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Battle.Enemy
{
    public class BattleEnemyModelsManager : MonoBehaviour, IDisposable
    {
        public struct ModelInfo
        {
            public BattleEnemyModel model;
            public RPGEnemy enemy;
        }

        private List<ModelInfo> models = new List<ModelInfo>();

        public bool IsAnyAnimating => models.Any(i => i.model.IsAnyEffectPlaying);

        public void AddModel(RPGEnemy enemy, Vector2 anposition)
        {
            if (HasModel(enemy))
                return;

            GameObject obj = Instantiate(enemy.EnemyModel.gameObject, transform, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            BattleEnemyModel mod = obj.GetComponent<BattleEnemyModel>();

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

        public BattleEnemyModel GetModel(RPGEnemy enemy)
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
}