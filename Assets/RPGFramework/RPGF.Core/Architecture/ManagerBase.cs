using System.Collections.Generic;

namespace RPGF.Core.Architecture
{
    public abstract class ManagerBase : RPGFrameworkBehaviour
    {
        public List<ModelBase> Models = new();

        public List<GameSystemBase> Systems = new();

        public override void Initialize()
        {
            
        }
    }
}
