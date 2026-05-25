using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.EventSystem
{
    [Serializable]
    public abstract class ActionBase : ICloneable<ActionBase>, IDisposable, IManagerInitialize, InjectionTarget
    {
        public const string DefaultNextTag = "DEFAULT";
        public const string DefaultNextName = "Далее";

        [SerializeReference]
        private List<NextAction> nexts;
        public List<NextAction> Nexts { get => nexts; }


        public ActionBase()
        {
            nexts = new List<NextAction>
            {
                new()
                {
                    IsNext = true,
                    Action = null,
                    Name = DefaultNextName,
                    Tag = DefaultNextTag
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

            if (next is null)
            {
                Debug.LogWarning($"NEXT {tag} не найден!");
                return;
            }
                

            next.Action = action;
        }

        public void ClearNextActions()
        {
            Nexts.ForEach(next => next.Action = null);
        }

        public void ClearNexts()
        {
            Nexts.Clear();
        }

        public NextAction GetActualNext()
        {
            return Nexts.FirstOrDefault(n => n.IsNext);
        }

        public NextAction GetNext(string tag)
        {
            return Nexts.FirstOrDefault(n => n.Tag == tag);
        }

        public void AddNext(string tag, string name = "")
        {
            Nexts.Add(new()
            {
                Tag = tag,
                Action = null,
                IsNext = false,
                Name = string.IsNullOrWhiteSpace(name) ? tag : name
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
                Nexts.First().IsNext = true;

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