using RPGF;
using RPGF.Core;
using RPGF.Domain.DI;
using System.Collections;
using UnityEngine;

namespace ARPGF.Core
{
    public class GameManager : KernelManagerBase
    {
        public static GameManager Instance;

        [SerializeField]
        private GlobalManager global;

        public DependencyInjection DI { get; private set; }

        public GlobalManager Global { get; private set; }
        public LocalManager Local { get; private set; }

        private bool globalIsReady = false;

        private Coroutine localInitializeCoroutine = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);

                Initialize();
            }
            else
                Destroy(gameObject);
        }

        public override void Initialize()
        {
            DI = new DependencyInjection();

            InitializeChild();
        }

        public override void InitializeChild()
        {
            Global = global;
            global.Initialize();

            globalIsReady = true;
        }

        public void LocalInitializeRequest(LocalManager local)
        {
            localInitializeCoroutine ??= StartCoroutine(LocalInitializeCoroutine(local));
        }

        public IEnumerator LocalInitializeCoroutine(LocalManager local)
        {
            yield return new WaitWhile(() => !globalIsReady);

            Local = local;
            local.Initialize();

            localInitializeCoroutine = null;
        }
    }
}
