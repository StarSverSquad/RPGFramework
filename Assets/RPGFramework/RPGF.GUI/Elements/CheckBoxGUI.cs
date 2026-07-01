using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPGF.GUI.Elements
{
    public class CheckBoxGUI : GUIElement
    {
        [SerializeField]
        private Image checkImage;

        private bool value;
        public bool Value 
        { 
            get => value;

            set
            {
                this.value = value;
                OnCheckedChanged();
                OnChecked?.Invoke(value);
            } 
        }

        public UnityEvent<bool> OnChecked;

        public void SetupCheckBox(bool isChecked)
        {
            value = isChecked;
            OnCheckedChanged();
        }

        public void Check()
        {
            Value = !Value;
        }

        private void OnCheckedChanged()
        {
            checkImage.enabled = value;
        }
    }
}