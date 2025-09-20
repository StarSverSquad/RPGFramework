using System.Collections;
using UnityEngine;

namespace RPGF.EventSystem
{
    public abstract class CustomActionBase : MonoBehaviour
    {
        public string ActionTag;

        public Coroutine Invoke() => StartCoroutine(ActionCoroutine());

        protected abstract IEnumerator ActionCoroutine();
    }
}