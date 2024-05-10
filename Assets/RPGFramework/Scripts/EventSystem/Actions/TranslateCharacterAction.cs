using System.Collections;
using System.Linq;
using UnityEngine;

public class TranslateCharacterAction : GraphActionBase
{
    public enum TranslateType
    {
        Move, MoveRelative, Rotate, RotateToPlayer
    }

    public bool InParty;

    public string CharacterTag;
    public DynamicExplorerObject CharacterInScene;

    public bool ReplaceInstance;
    public bool Wait;

    public float Speed;

    public Vector2 Offset;
    public Transform Point;

    public CommonDirection Direction;

    public TranslateType Type;

    private LocalCharacterManager Character => LocalManager.Instance.Character;
    private PlayerExplorerManager Player => ExplorerManager.Instance.PlayerManager;

    public TranslateCharacterAction() : base("TranslateCharater")
    {
        InParty = false;

        CharacterTag = string.Empty;
        CharacterInScene = null;

        ReplaceInstance = false;
        Wait = false;

        Speed = 0;

        Offset = Vector2.zero;
        Direction = CommonDirection.None;

        Type = TranslateType.MoveRelative;
    }

    public override IEnumerator ActionCoroutine()
    {
        DynamicExplorerObject model = null;

        int index = 0;
        if (InParty)
        {
            for ( ; index < Character.Characters.Length; index++)
            {
                if (Character.Characters[index].Tag == CharacterTag)
                {
                    model = Character.Models[index];

                    break;
                }
            }
        }
        else
            model = CharacterInScene;

        if (model == null)
        {
            Debug.LogWarning($"Персонаж не найден!");
            yield break;
        }

        switch (Type)
        {
            case TranslateType.Move:
            case TranslateType.MoveRelative:

                if (ReplaceInstance)
                {
                    if (Type == TranslateType.Move)
                        model.transform.position = Point.transform.position;
                    else
                        model.transform.position = (Vector2)model.transform.position + Offset;
                }
                else
                {
                    if (Type == TranslateType.Move)
                        model.MoveToBySpeed(Point.transform.position, Speed);
                    else
                        model.TranslateBySpeed(Offset, Speed);

                    if (Wait)
                        yield return new WaitWhile(() => model.IsMove);
                }

                if (InParty && index == 0)
                {
                    if (Type == TranslateType.Move)
                        Player.TeleportToVector(Point.transform.position);
                    else
                        Player.TeleportToVector(ExplorerManager.GetPlayerPosition() + Offset);
                }

                break;
            case TranslateType.Rotate:
                if (InParty && index == 0)
                {
                    ExplorerManager.Instance.PlayerManager.movement.RotateTo(Direction);

                    model.StopAnimation();
                    model.RotateTo(Direction);
                }
                else
                    model.RotateTo(Direction);
                break;
            case TranslateType.RotateToPlayer:
                model.RotateToPlayer();
                break;
        }

        yield break;
    }

    public override string GetHeader()
    {
        return "Перемещение персонажа";
    }
}