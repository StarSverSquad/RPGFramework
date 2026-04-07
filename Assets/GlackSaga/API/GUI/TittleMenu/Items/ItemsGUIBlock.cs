using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.GUI.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GlackSaga.GUI.TitleMenu.Items
{
    public class ItemsGUIBlock : GUIChoiceBlock
    {
        public enum ItemsTabs
        {
            Regular = 0, 
            Important = 1
        }
    }
}
