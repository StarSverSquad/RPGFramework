using System.Collections;
using UnityEngine;

namespace RPGF.EventSystem
{
    public abstract class CustomActionBase : MonoBehaviour
    {
        protected abstract IEnumerator ActionCoroutine();
    }
}