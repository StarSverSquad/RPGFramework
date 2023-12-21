using System.Collections;
using UnityEngine;

public class BattleBackground : MonoBehaviour
{
    public GameObject BackGround;

    public void CreateBackground(GameObject Background)
    {
        BackGround = Instantiate(Background, transform, false);
    }

    public void DestoyBackground()
    {
        if (BackGround != null)
            Destroy(BackGround);

        BackGround = null;
    }
}