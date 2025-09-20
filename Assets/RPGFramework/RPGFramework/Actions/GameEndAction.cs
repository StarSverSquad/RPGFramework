using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndAction : GraphActionBase
{
    public GameEndAction() : base("GameEnd")
    {
    }

    public override IEnumerator ActionCoroutine()
    {
        GameManager.Instance.SceneLoader.LoadScene("DemoMenu"); // Надо будет поменять на просто MainMenu

        yield break;
    }

    public override string GetHeader()
    {
        return "Завершить игру";
    }
}