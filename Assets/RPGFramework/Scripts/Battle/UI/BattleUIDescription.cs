using TMPro;
using UnityEngine;

public class BattleUIDescription : MonoBehaviour, IActive
{
    [SerializeField]
    private TextMeshProUGUI txt;

    [SerializeField]
    private GameObject container;
    
    public string Text
    {
        get => txt.text;
        set => txt.text = value;
    }

    public void SetActive(bool active)
    {
        container.SetActive(active);
    }
}