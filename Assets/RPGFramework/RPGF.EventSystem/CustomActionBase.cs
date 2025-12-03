using System.Collections;
using UnityEngine;

namespace RPGF.EventSystem
{
    public abstract class CustomActionBase : MonoBehaviour
    {
        public abstract IEnumerator ActionCoroutine();
    }
}