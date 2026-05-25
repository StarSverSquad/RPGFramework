using RPGF.Core.SaveLoad;
using RPGF.Domain.Interfaces;
using RPGF.GUI;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.SaveLoad
{
    public class SaveSlotItem : GUIInteractable, ISetData<GameSlotData>
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        public void SetData(GameSlotData data)
        {
            _text.text = data.Id.ToString();
        }
    }
}