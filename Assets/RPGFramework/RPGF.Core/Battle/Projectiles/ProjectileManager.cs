using RPGF.Core.Battle.BattleField;
using RPGF.Core.Battle.Projectiles.Abstractions;
using RPGF.Domain.DI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGF.Core.Battle.Projectiles
{
    public class ProjectileManager : RPGFrameworkBehaviour, IDisposable
    {
        [Inject]
        private readonly BattleFieldManager _battleField;

        private readonly List<ProjectileBase> _projectiles = new();

        [SerializeField]
        private Transform projectileContainer;

        public T Create<T>(T originalProjectile, Vector2 position)
            where T : ProjectileBase
        {
            var projObject = Instantiate(
                originalProjectile.gameObject, 
                _battleField.Center + position, 
                Quaternion.identity, 
                projectileContainer
            );

            var projectile = projObject.GetComponent<ProjectileBase>();
            projectile.Initialize();

            _projectiles.Add(projectile);

            return projectile as T;
        }

        public T CreateHidden<T>(T originalProjectile, Vector2 position)
            where T : ProjectileBase
        {
            var projectile = Create(originalProjectile, position);
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
