using RPGF.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RPGF.Explorer
{
    public class SunManager : RPGFrameworkBehaviour
    {
        [SerializeField]
        private Light2D Light;

        public void SetIntensity(float intensity) => Light.intensity = intensity;

        public void SetColor(Color color) => Light.color = color;
    }
}