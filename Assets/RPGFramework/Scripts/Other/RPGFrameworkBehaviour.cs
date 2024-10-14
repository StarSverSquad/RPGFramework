using UnityEngine;

public class RPGFrameworkBehaviour : MonoBehaviour, IManagerInitialize
{
    protected GameManager Game => GameManager.Instance;   
    protected LocalManager Local => LocalManager.Instance;
    protected ExplorerManager Explorer => ExplorerManager.Instance;
    protected CommonManager Common => CommonManager.Instance;
    protected BattleManager Battle => BattleManager.Instance;

    public virtual void Initialize() { }
}