using RPGF.Shared;
using UnityEngine;

public class RPGFrameworkBehaviour : MonoBehaviour, IManagerInitialize
{
    protected GameManager Game => GameManager.Instance;   
    protected LocalManager Local => LocalManager.Instance;
    protected ExplorerManager Explorer => ExplorerManager.Instance;
    protected SharedManager Common => SharedManager.Instance;
    protected BattleManager Battle => BattleManager.Instance;

    public virtual void Initialize() { }

    protected string GetLocale(string tag) => Game.Localization.GetLocale(tag);
}