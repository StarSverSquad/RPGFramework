using GlackSaga.GUI.TitleMenu.FullInfo;
using RPGF.GUI;
using System.Linq;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Party
{
    public class PartyGUIBlock : GUIChoicableBlock
    {
        [SerializeField]
        private FullInfoBlockGUI _fullInfoGUI;
        [SerializeField]
        private PartyItemGUIElement[] Items = new PartyItemGUIElement[4];

        protected override void OnActivate()
        {
            var charactersCount = Game.Character.Characters.Length;

            SetElements(Items.Take(Mathf.Clamp(charactersCount, 0, 4)).ToArray());

            for (int i = 0; i < Items.Length; i++)
            {
                if (i < charactersCount)
                {
                    Items[i].SetData(Game.Character.Characters[i]);
                    Items[i].gameObject.SetActive(true);
                }
                else
                {
                    Items[i].gameObject.SetActive(false);
                }

            }

            base.OnActivate();
        }

        public override void OnChoiced(int index)
        {
            _fullInfoGUI.SetData(Game.Character.Characters[index]);
            Next(_fullInfoGUI);
        }

        protected override void OnFocus()
        {
            StartChoice();
        }
    }
}