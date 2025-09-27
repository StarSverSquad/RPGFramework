using RPGF;
using RPGF.EventSystem;
using System.Collections;

public class GameEndAction : GraphActionBase
{
    public GameEndAction() : base("GameEnd")
    {
    }

    public override IEnumerator ActionCoroutine()
    {
        GlobalManager.Instance.SceneLoader.LoadScene("DemoMenu"); // Надо будет поменять на просто MainMenu

        yield break;
    }

    public override string GetHeader()
    {
        return "Завершить игру";
    }
}