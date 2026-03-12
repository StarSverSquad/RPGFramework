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

        public BattleFieldBase MainField => fields.FirstOrDefault();
        public bool HasField => fields.Any();

        public override void Initialize()
        {
            base.Initialize();
        }

        public BattleFieldBase Create(BattleFieldBase original, Vector2 position)
        {
            var gameObject = Instantiate(original.gameObject, transform.position, Quaternion.identity, transform);
            var field = gameObject.GetComponent<BattleFieldBase>();

            DI.InjectInto(field);

            field.MoveTo(position);

            fields.Add(field);

            return field;
        }
        public T Create<T>(T original, Vector2 position)
            where T : BattleFieldBase
        {
            return Create(original, position);
        }

        public void Remove(BattleFieldBase field)
        {
            if (fields.Contains(field))
            {
                fields.Remove(field);
            }

            field.Dispose();
            field.Hide();
            Destroy(field.gameObject, field.AnimationTime);
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
                Destroy(field.gameObject, field.AnimationTime);
            }
            fields.Clear();
        }
    }
}
