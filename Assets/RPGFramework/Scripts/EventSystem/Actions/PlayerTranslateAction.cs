using RPGF;
using System.Collections;
using UnityEngine;

public class PlayerTranslateAction : GraphActionBase
{
    public enum TranslateType
    {
        Move, MoveRelative
    }

    public MoveDirection Direction;
    public TranslateType Type;

    public float Speed;

    public bool ReplaceInstance;
    public bool Wait;

    public Transform MovePoint;
    public Vector2 Offset;

    public PlayerTranslateAction()
    {
        Direction = MoveDirection.Down;
        Type = TranslateType.MoveRelative;
        Speed = 1f;
        ReplaceInstance = false;
        Wait = false;
    }

    public override IEnumerator ActionCoroutine()
    {
        Vector3 playerPosition = ExplorerManager.GetPlayerPosition3D();

        switch (Type)
        {
            case TranslateType.Move:
                if (!ReplaceInstance)
                    ExplorerManager.Instance.PlayerManager.movement.TranslateBySpeed(MovePoint.transform.position - playerPosition, Speed);
                else
                    ExplorerManager.Instance.PlayerManager.TeleportToVector(MovePoint.transform.position);
                break;
            case TranslateType.MoveRelative:
                if (!ReplaceInstance)
                    ExplorerManager.Instance.PlayerManager.movement.TranslateBySpeed(Offset, Speed);
                else
                    ExplorerManager.Instance.PlayerManager.TeleportToVector((Vector2)playerPosition + Offset);
                break;
        }

        if (Wait)
            yield return new WaitWhile(() => ExplorerManager.Instance.PlayerManager.movement.IsAutoMoving);

        yield break;
    }

    public override string GetHeader()
    {
        return "Перемещение игрока";
    }
}