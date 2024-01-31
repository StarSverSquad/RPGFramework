using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunManager : MonoBehaviour
{
    [SerializeField]
    private Light2D Light;

    public void SetIntensity(float intensity) => Light.intensity = intensity;

    public void SetColor(Color color) => Light.color = color;
}
