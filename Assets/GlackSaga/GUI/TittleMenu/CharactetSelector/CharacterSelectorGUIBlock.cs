using GlackSaga.GUI.TitleMenu;
using RPGF.Core.Character;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.GUI.Interfaces;
using RPGF.RPG;
using UnityEngine;

namespace GlackSaga.GUI.TittleMenu.CharactetSelector
{
    public class CharacterSelectorGUIBlock : GUISelectableBlock
    {
        [Inject]
        private readonly CharacterService _characterService;

        [Header("Links:")]
        [SerializeField]
        private CharacterInformationManager characterInformation;

        public RPGCharacter SeletedCharacter { get; private set; } = null;

        public int CharactersAmount { get; private set; }

        protected override void OnActivate()
        {
            base.OnActivate();

            CharactersAmount = _characterService.Characters.Length;

            characterInformation.Show();
        }

        protected override void OnDiativate()
        {
            base.OnDiativate();

            characterInformation.Hide();

            foreach (var item in Elements)
            {
                item.SetFocus(false);
            }
        }

        protected override void OnChoiced(int index)
        {
            base.OnChoiced(index);

            SeletedCharacter = _characterService.Characters[index];

            Preview();
        }

        protected override void ChangeSelect(int newIndex)
        {
            if (newIndex < CharactersAmount)
            {
                base.ChangeSelect(newIndex);
            }
        }
    }
}
