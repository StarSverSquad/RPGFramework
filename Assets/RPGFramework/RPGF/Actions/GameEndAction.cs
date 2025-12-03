using RPGF;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System.Collections;

namespace RPGF.Actions
{
    [GenerateActionNode("Завершить игру")]
    public class GameEndAction : ActionBase
    {
        public GameEndAction() : base()
        {
        }

        public override IEnumerator ActionCoroutine()
        {
            /// TODO: Надо будет поменять на просто MainMenu
            GlobalManager.Instance.SceneLoader.LoadScene("DemoMenu"); 

            yield break;
        }
    }
}