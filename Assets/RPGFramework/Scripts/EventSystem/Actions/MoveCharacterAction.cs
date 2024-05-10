using System;
using System.Collections;
using UnityEngine;


[Obsolete]
public class MoveCharacterAction : GraphActionBase
{
    public int Index;

    public Vector2 Offset;

    public float Speed;

    public bool IsWait;

    private LocalCharacterManager CharacterManager => LocalManager.Instance.Character;
    private PlayerExplorerManager Player => ExplorerManager.Instance.PlayerManager;

    public MoveCharacterAction() : base("MoveCharacter")
    {
        Index = 0;
        Offset = Vector2.zero;
        Speed = 1f;
        IsWait = true;
    }

    public override IEnumerator ActionCoroutine()
    {
        if (CharacterManager.Models.Count < Index + 1)
        {
            Debug.LogError($"Модель персонажа под индексом {Index} не найдена!");

            yield break;
        }

        DynamicExplorerObject model = CharacterManager.Models[Index];

        model.TranslateBySpeed(Offset, Speed);

        if (Index == 0)
            Player.TeleportToVector(ExplorerManager.GetPlayerPosition() + Offset);

        if (IsWait)
            yield return new WaitWhile(() => model.IsMove);
    }

    public override string GetHeader()
    {
        return "Двигать персонажа из партии";
    }
}