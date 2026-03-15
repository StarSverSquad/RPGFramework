using RPGF.Core.Battle.BattleField.Abstractions;
using RPGF.Domain.DI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.Core.Battle.BattleField
{
    public class BattleFieldManager : RPGFrameworkBehaviour, IEnumerable<BattleFieldBase>, IDisposable
    {
        [Inject]
        private readonly DependencyInjection DI;

        private readonly List<BattleFieldBase> fields = new();

        [SerializeField]
        private Vector2 centerOffset = Vector2.zero;
        public Vector2 Center => (Vector2)transform.position + centerOffset;

        public BattleFieldBase MainField => fields.FirstOrDefault();
        public bool HasField => fields.Any();

        public override void Initialize()
        {
            base.Initialize();
        }

        public T Create<T>(T original)
            where T : BattleFieldBase
        {
            return Create(original, Vector2.zero);
        }
        public T Create<T>(T original, Vector2 position)
            where T : BattleFieldBase
        {
            var gameObject = Instantiate(
                original.gameObject, 
                position, 
                Quaternion.identity, 
                new InstantiateParameters()
                {
                    parent = transform,
                    worldSpace = false
                });
            var field = gameObject.GetComponent<BattleFieldBase>();

            DI.InjectInto(field);
            field.Initialize();

            fields.Add(field);

            return field as T;
        }

        public void Remove(BattleFieldBase field)
        {
            if (fields.Contains(field))
            {
                fields.Remove(field);
            }

            field.Dispose();
            field.Hide();
            Destroy(field.gameObject, field.DestroyTime);
        }

        public IEnumerator<BattleFieldBase> GetEnumerator()
        {
            return fields.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var field in fields)
            {
                field.Dispose();
                field.Hide();
                Destroy(field.gameObject, field.DestroyTime);
            }
            fields.Clear();
        }
    }
}
