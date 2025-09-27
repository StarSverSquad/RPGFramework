using RPGF.Battle;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using RPGF.Explorer;
using RPGF.Shared;
using UnityEngine;

namespace RPGF.Core
{
    public class RPGFrameworkBehaviour : MonoBehaviour, IManagerInitialize, Injectable
    {
        protected GlobalManager Global => GlobalManager.Instance;
        protected LocalManager Local => LocalManager.Instance;
        protected ExplorerManager Explorer => ExplorerManager.Instance;
        protected SharedManager Common => SharedManager.Instance;
        protected BattleManager Battle => BattleManager.Instance;

        public virtual void Initialize() { }

        protected string GetLocale(string tag) => Global.Localization.GetLocale(tag);
    }
}