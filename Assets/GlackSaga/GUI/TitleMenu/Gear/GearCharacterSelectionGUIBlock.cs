using System.Collections.Generic;
using System.Linq;
using RPGF.Core.Character;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.GUI.Interfaces;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Gear
{
    public class GearCharacterSelectionGUIBlock : GUISelectableBlock
    {
        [Inject]
        private readonly CharacterService _characterService = null!;

        [SerializeField]
        private GearSlotGUIBlock slotGUIBlock;
        [SerializeField]
        private List<GearCharacterItem> characterItems = new();

        public override void Initialize(IGUIManager manager)
        {
            base.Initialize(manager);

            SetElements(characterItems.ToArray());
        }

        protected override void OnActivate()
        {
            SetElements(characterItems.Take(_characterService.Characters.Length).ToArray());
            for (int i = 0; i < characterItems.Count; i++)
            {
                if (i < _characterService.Characters.Length)
                {
                    characterItems[i].SetData(_characterService.Characters[i]);
                    characterItems[i].gameObject.SetActive(true);
                }
                else
                {
                    characterItems[i].gameObject.SetActive(false);
                }
            }
            slotGUIBlock.SetData(_characterService.Characters[CurrentIndex]);
            slotGUIBlock.ClearItemPreview();

            base.OnActivate();
        }

        protected override void OnSelectionChanged()
        {
            slotGUIBlock.SetData(_characterService.Characters[CurrentIndex]);
        }
    }
}