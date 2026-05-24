using RPGF.Domain.Interfaces;
using RPGF.GUI;
using TMPro;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Gear
{
    public class GearStatDisplay : GUIWidget
    {
        [SerializeField]
        private TextMeshProUGUI statText;
        [SerializeField]
        private TextMeshProUGUI statChangeText;

        public void SetStat(string value)
        {
            statText.text = value;
        }

        public void SetStatChange(int value)
        {
            if (value == 0)
            {
                statChangeText.text = string.Empty;
                return;
            }

            var color = value > 0 ? "green" : "red";
            var text = value > 0 ? $"+{value}" : $"{value}";

            statChangeText.text = $"<color={color}>({text})</color>";
        }
    }
}
