using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SimpleFallingText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private RectTransform rt;

    [SerializeField]
    private float upDistance = 10f;
    public float UpDistance { get { return upDistance; } set { upDistance = value; } }

    [SerializeField]
    private float fallingDistance = 20f;
    public float FallingDistance { get { return fallingDistance; } set {  fallingDistance = value; } }

    [SerializeField]
    private float fallingTime = 1f;
    public float FallingTime { get => fallingTime; set => fallingTime = value; }

    [SerializeField]
    private Color gradientIn = Color.red;
    public Color GradientIn { get { return gradientIn; } set {  gradientIn = value; } }

    [SerializeField]
    private Color gradientOut = Color.white;
    public Color GradientOut { get {  return gradientOut; } set { gradientOut = value; } }

    [SerializeField]
    private bool isFalling = false;
    public bool IsFalling => isFalling;
    
    public TextMeshProUGUI Text => text;

    public void Invoke(string text)
    {
        this.text.text = text;

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        isFalling = true;

        float asp = fallingDistance / (fallingDistance + upDistance);

        StartCoroutine(AnimationPack.ColorByTime(gradientOut, gradientIn, fallingTime, value =>
        {
            text.color = value;
        }));

        yield return StartCoroutine(AnimationPack.MoveToByTime(rt.anchoredPosition.y, rt.anchoredPosition.y + upDistance, (1 - asp) * fallingTime, value =>
        {
            rt.anchoredPosition = new Vector2(0, value);
        }));

        yield return StartCoroutine(AnimationPack.MoveToByTime(rt.anchoredPosition.y, rt.anchoredPosition.y - fallingDistance - upDistance, asp * fallingTime, value =>
        {
            rt.anchoredPosition = new Vector2(0, value);
        }));

        isFalling = false;
    }
}
