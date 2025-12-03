using RPGF;
using RPGF.Domain.DI;
using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using RPGF.Explorer;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [GenerateActionNode("Настройка освещения", contextMenuPath: "Система/Настройка освещения")]
    public class SetupSunLightAction : ActionBase
    {
        [Inject]
        private readonly SunManager _sun;

        [ActionFieldOption("Интенсивность:")]
        public float Intensity;
        [ActionFieldOption("Цвет освещения:")]
        public Color Color;

        public SetupSunLightAction() : base()
        {
            Intensity = 1.0f;

            Color = Color.white;
        }

        public override IEnumerator ActionCoroutine()
        {
            _sun.SetIntensity(Intensity);
            _sun.SetColor(Color);

            yield break;
        }
    }
}