using RPGF.Domain.Interfaces;
using UnityEngine;

namespace RPGF.Core.Architecture
{
    public abstract class KernelManagerBase : MonoBehaviour, IActive, IManagerInitialize
    {
        [SerializeField]
        private GameObject container;

        public abstract void Initialize();

        public abstract void InitializeChild();

        public void SetActive(bool value) => container.SetActive(value);
    }
}
