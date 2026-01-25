using DG.Tweening;
using RPGF.Core;
using RPGF.UI;
using RPGF.RPG;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPGF.Core.Battle;

namespace RPGF.Battle.UI
{
    public class CharacterQueryElement : RPGFrameworkBehaviour
    {
        [SerializeField]
        private GameObject fallContent;
        public bool FallIsActive
        {
            set => fallContent.SetActive(value);
            get => fallContent.activeSelf;
        }

        [SerializeField]
        private GameObject usedContent;
        public bool UsedIsActive
        {
            set => usedContent.SetActive(value);
            get => usedContent.activeSelf;
        }

        [SerializeField]
        private Image actionImage;

        [SerializeField]
        private RectTransform content;

        [SerializeField]
        private Image gradient;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private TextMeshProUGUI characterNameTxt;

        [SerializeField]
        private IconList iconList;
        [SerializeField]
        private LineBar healBar;
        [SerializeField]
        private LineBar manaBar;

        private RPGCharacter linkedCharacter;
        private BattleTurnData CharacterTurnData => Battle.Data.TurnsData.First(i => i.Character == linkedCharacter);

        public float moveDuration = 0.2f;

        private RectTransform rectTransform;

        private Tween moveAnimation;

        public void Initialize(RPGCharacter character)
        {
            gradient.color = character.Color;
            icon.sprite = character.BattleQueryImage;

            characterNameTxt.text = character.Name;

            iconList.UpdateIcons(character.States.Select(i => i.Icon).ToArray());

            healBar.SetValue((float)character.Heal / (float)character.MaxHeal);
            manaBar.SetValue((float)character.Mana / (float)character.MaxMana);

            rectTransform = GetComponent<RectTransform>();

            linkedCharacter = character;

            FallIsActive = CharacterTurnData.IsDead;
        }

        public void MoveToPoint(Vector2 rectPoint, Vector2 fromPoint)
        {
            if (moveAnimation != null)
                moveAnimation.Kill();

            rectTransform.anchoredPosition = fromPoint;

            moveAnimation = rectTransform.DOAnchorPos(rectPoint, moveDuration).Play();
        }

        public void UpdateDynamic()
        {
            if (!FallIsActive)
            {
                Sprite acitonSprite = Battle.Config.GetActionIcon(CharacterTurnData.BattleAction);

                actionImage.sprite = acitonSprite;
                actionImage.gameObject.SetActive(acitonSprite != null);

                UsedIsActive = acitonSprite != null;
            }
        }

        private void OnDestroy()
        {
            moveAnimation.Kill();
        }
    }
}