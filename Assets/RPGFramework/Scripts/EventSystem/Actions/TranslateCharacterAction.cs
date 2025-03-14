using RPGF;
using RPGF.Character;
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
    public PlayableCharacterModelController CharacterInScene;

    public bool ReplaceInstantly;
    public bool Wait;

    public float Speed;

    public Vector2 Offset;
    public Transform Point;

    public ViewDirection Direction;

    public TranslateType Type;

    private LocalCharacterManager Character => LocalManager.Instance.Character;
    private PlayerExplorerManager Player => ExplorerManager.Instance.PlayerManager;

    public TranslateCharacterAction() : base("TranslateCharater")
    {
        InParty = false;

        CharacterTag = string.Empty;
        CharacterInScene = null;

        ReplaceInstantly = false;
        Wait = false;

        Speed = 0;

        Offset = Vector2.zero;
        Direction = ViewDirection.Down;

        Type = TranslateType.MoveRelative;
    }

    public override IEnumerator ActionCoroutine()
    {
        PlayableCharacterModelController model = null;

        bool isFisrtCharacter = false;
        if (InParty)
        {
            for (int index = 0; index < Character.Characters.Length; index++)
            {
                if (Character.Characters[index].Tag == CharacterTag)
                {
                    model = Character.Models[index];

                    isFisrtCharacter = index == 0;

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

                if (ReplaceInstantly)
                {
                    if (Type == TranslateType.Move)
                        model.transform.position = Point.transform.position;
                    else
                        model.transform.position = (Vector2)model.transform.position + Offset;
                }
                else
                {
                    if (Type == TranslateType.Move)
                    {
                        float time = Vector2.Distance(Point.transform.position, model.transform.position) / Speed;

                        model.MoveTo(Point.transform.position, time);
                    }
                    else
                    {
                        float time = Offset.magnitude / Speed;

                        model.MoveToRelative(Offset, time);
                    }
                        

                    if (Wait)
                        yield return new WaitWhile(() => model.IsMove);
                }

                if (isFisrtCharacter)
                {
                    if (Type == TranslateType.Move)
                        Player.TeleportToVector(Point.transform.position);
                    else
                        Player.TeleportToVector(ExplorerManager.GetPlayerPosition() + Offset);
                }

                break;
            case TranslateType.Rotate:
                if (isFisrtCharacter)
                {
                    //ExplorerManager.Instance.PlayerManager.movement.RotateTo(Direction);

                    //model.StopAnimation();
                    //model.RotateTo(Direction);
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