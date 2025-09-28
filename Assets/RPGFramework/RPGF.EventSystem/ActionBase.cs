using RPGF.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace RPGF.EventSystem
{
    [Serializable]
    public abstract class ActionBase : ICloneable<ActionBase>, IDisposable, IInitializable
    {
        public const string DefaultNextName = "DEFAULT";

        public List<NextAction> Nexts { get; private set; }

        public ActionBase()
        {
            Nexts = new List<NextAction>
            {
                new()
                {
                    IsNext = true,
                    Action = null,
                    Tag = DefaultNextName
                }
            };
        }

        public void SetNextAction(ActionBase action)
        {
            var next = GetActualNext();

            next.Action = action;
        }

        public void SetNextAction(ActionBase action, string tag)
        {
            var next = GetNext(tag);

            next.Action = action;
        }

        public NextAction GetActualNext()
        {
            return Nexts.FirstOrDefault(n => n.IsNext);
        }

        public NextAction GetNext(string tag)
        {
            return Nexts.FirstOrDefault(n => n.Tag == tag);
        }

        public void AddNext(string tag)
        {
            Nexts.Add(new()
            {
                Tag = tag,
                Action = null,
                IsNext = false
            });
        }

        public void RemoveNext(string tag)
        {
            Nexts.RemoveAll(n => n.Tag == tag);
        }

        public void SetNext(string tag)
        {
            foreach (var next in Nexts)
                next.IsNext = next.Tag == tag;

            if (Nexts.All(n => !n.IsNext))
            {
                Nexts.First(n => n.Tag == DefaultNextName).IsNext = true;

                Debug.LogWarning($"Не найден {tag}!");
            }
        }

        public abstract IEnumerator ActionCoroutine();

        public virtual ActionBase Clone()
        {
            return MemberwiseClone() as ActionBase;
        }

        public virtual void Dispose() { }

        public virtual void Initialize() { }
    }
}