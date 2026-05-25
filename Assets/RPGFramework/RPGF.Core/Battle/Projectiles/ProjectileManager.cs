using System;
using System.Collections.Generic;
using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.Projectiles.Abstractions;
using RPGF.Domain.DI;
using RPGF.RPG;
using UnityEngine;

namespace RPGF.Core.Battle.Projectiles
{
    public class ProjectileManager : RPGFrameworkBehaviour, IDisposable
    {
        private readonly List<ProjectileBase> _projectiles = new();

        [SerializeField]
        private Transform projectileContainer;

        public T Create<T>(T originalProjectile, Vector2 position, RPGEnemy owner = null)
            where T : ProjectileBase
        {
            var projObject = Instantiate(
                originalProjectile.gameObject,
                position,
                Quaternion.identity,
                new InstantiateParameters()
                {
                    parent = projectileContainer.transform,
                    worldSpace = false,
                }
            );

            var projectile = projObject.GetComponent<ProjectileBase>();
            projectile.Initialize();
            projectile.Owner = owner;

            _projectiles.Add(projectile);

            return projectile as T;
        }

        public T CreateHidden<T>(T originalProjectile, Vector2 position, RPGEnemy owner = null)
            where T : ProjectileBase
        {
            var projectile = Create(originalProjectile, position, owner);
            projectile.SetHide(true);

            return projectile;
        }

        public void Dispose()
        {
            foreach (var projectile in _projectiles)
            {
                if (projectile == null)
                    continue;

                projectile.Dispose();
                Destroy(projectile.gameObject);
            }
            _projectiles.Clear();
        }
    }
}
