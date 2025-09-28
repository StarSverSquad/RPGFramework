using RPGF.Domain.DI;
using RPGF.EventSystem.Default;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGF.EventSystem
{
    [Serializable]
    public class Event
    {
        [SerializeReference]
        public List<ActionBase> Actions = new();

        public bool IsPlaying => coroutine != null;

        private Coroutine coroutine;
        private MonoBehaviour listener;

        public event Action OnStart;
        public event Action OnEnd;

        public void Invoke(MonoBehaviour listener, DependencyInjection di = null)
        {
            if (!IsPlaying)
            {
                this.listener = listener;
                coroutine = this.listener.StartCoroutine(EventCoroutine(di));
            }
        }

        public void Break()
        {
            if (IsPlaying)
            {
                listener.StopCoroutine(coroutine);
                coroutine = null;
                listener = null;
            }
        }

        private IEnumerator EventCoroutine(DependencyInjection di)
        {
            OnStart?.Invoke();

            if (di is not null)
                Actions.ForEach(act => di.InjectInto(act));

            ActionBase current = Actions.FirstOrDefault(a => a is StartAction);

            if (current is null && Actions.Count > 0)
            {
                Debug.LogWarning("Не найдено стартовое событие!");
                current = Actions[0];
            }

            while (current != null)
            {
                current.Initialize();

                yield return current.ActionCoroutine();

                current.Dispose();

                current = current.GetActualNext().Action;
            }

            coroutine = null;
            listener = null;

            OnEnd?.Invoke();
        }
    }
}