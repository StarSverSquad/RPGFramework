using RPGF.Domain.DI;
using RPGF.EventSystem.Graph;
using System.Collections;
using UnityEngine;

namespace RPGF.EventSystem
{
    [CreateAssetMenu(fileName = "GlobalEvent", menuName = "RPGFramework/Global Event")]
    public class GlobalEvent : ScriptableObject
    {
        public GraphEvent InnerEvent;

        public bool IsPlaying => InnerEvent.IsPlaying;

        public void Invoke(MonoBehaviour listener, DependencyInjection di)
        {
            InnerEvent.Invoke(listener, di);
        }

        public IEnumerator WaitForEnd()
        {
            yield return new WaitWhile(() => IsPlaying);
        }
    }
}
