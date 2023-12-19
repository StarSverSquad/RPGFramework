using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBoxManager : MonoBehaviour, IDisposable, IActive
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private CharacterBox[] boxes = new CharacterBox[4];
    public CharacterBox[] Boxes => boxes;

    public Vector2[] BoxesGlobalPositions => boxes.Select(i => (Vector2)i.transform.position).ToArray();

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        Dispose();
    }

    public void Initialize(params BattleCharacterInfo[] characters)
    {
        SetActive(true);

        int count = Mathf.Min(characters.Length, boxes.Length);
        for (int i = 0; i < count; i++)
        {
            boxes[i].gameObject.SetActive(true);
            boxes[i].Initialize(characters[i]);
        }
    }

    public CharacterBox GetBox(BattleCharacterInfo character) => boxes.First(i => i.Character == character);

    public void ChangePosition(bool top)
    {
        animator.SetBool("Top", top);
    }

    public void Dispose()
    {
        foreach (var item in boxes)
        {
            item.Dispose();
            item.gameObject.SetActive(false);
        }

        SetActive(false);
    }

    public void SetActive(bool active) => container.SetActive(active);
}