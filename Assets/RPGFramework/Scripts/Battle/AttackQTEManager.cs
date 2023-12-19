using System.Collections;
using UnityEngine;

public class AttackQTEManager : MonoBehaviour
{
    public enum Positions
    {
        Default = -1, Box0, Box1, Box2, Box3
    }

    [SerializeField]
    private AttackQTE qte;
    public AttackQTE QTE => qte;

    [SerializeField]
    private RectTransform qteTransform;

    [Tooltip("Default, CharBox0..3")]
    [SerializeField]
    private Vector2[] positions = new Vector2[5];

    public void InvokeQTE(Positions position)
    {
        if (position == Positions.Default)
            return;

        qteTransform.anchoredPosition = positions[(int)position + 1];

        qte.Invoke();
    }

    public void DropQTE()
    {
        qteTransform.anchoredPosition = positions[0];
    }
}