using ARPGF.Core;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using UnityEngine;

namespace RPGF.Core
{
    public abstract class KernelManagerBase : MonoBehaviour, IActive, IManagerInitialize, Injectable
    {
        public GameManager Game => GameManager.Instance;

        [SerializeField]
        private GameObject container;

        public abstract void Initialize();

        public virtual void InitializeChild() { }

        public void SetActive(bool value) => container.SetActive(value);
    }
}
