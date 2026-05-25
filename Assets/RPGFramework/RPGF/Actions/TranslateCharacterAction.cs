using RPGF;
using RPGF.Core.Character;
using RPGF.Domain;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.Explorer;
using RPGF.Explorer.Player;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    public class TranslateCharacterAction : ActionBase
    {
        public enum TranslateType
        {
            Move, MoveRelative, Rotate, RotateToPlayer
        }

        [Inject]
        private readonly CharacterManager _character = null!;
        [Inject]
        private readonly PlayerExplorerManager _player = null!;

        public bool InParty;

        public string CharacterTag;
        public CharacterModelControllerBase CharacterInScene;

        public bool ReplaceInstantly;
        public bool Wait;
        public bool WithRotation;

        public float Time;

        public Vector2 Offset;
        public Transform Point;

        public ViewDirection Direction;

        public TranslateType Type;

        public TranslateCharacterAction() : base()
        {
            InParty = false;

            CharacterTag = string.Empty;
            CharacterInScene = null;

            ReplaceInstantly = false;
            Wait = false;
            WithRotation = true;

            Time = 0;

            Offset = Vector2.zero;
            Direction = ViewDirection.Down;

            Type = TranslateType.MoveRelative;
        }

        public override IEnumerator ActionCoroutine()
        {
            CharacterModelControllerBase model = null;

            bool isFisrtCharacter = false;
            if (InParty)
            {
                for (int index = 0; index < _character.Characters.Length; index++)
                {
                    if (_character.Characters[index].Tag == CharacterTag)
                    {
                        model = _character.Models[index];

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
                            if (WithRotation)
                                model.MoveTo(Point.transform.position, Time);
                            else
                                model.MoveToNotRotate(Point.transform.position, Time);
                        }
                        else
                        {
                            if (WithRotation)
                                model.MoveToRelative(Offset, Time);
                            else
                                model.MoveToRelativeNotRotate(Offset, Time);
                        }


                        if (Wait)
                            yield return new WaitWhile(() => model.IsMove);
                    }

                    if (isFisrtCharacter)
                    {
                        if (Type == TranslateType.Move)
                            _player.TeleportToVector(Point.transform.position);
                        else
                            _player.TeleportToVector(ExplorerManager.GetPlayerPosition() + Offset);
                    }

                    break;
                case TranslateType.Rotate:
                    if (isFisrtCharacter)
                        _player.movement.RotateTo(Direction);

                    model.RotateTo(Direction);
                    break;
                case TranslateType.RotateToPlayer:
                    model.RotateToPlayer();
                    break;
            }

            yield break;
        }
    }
}
