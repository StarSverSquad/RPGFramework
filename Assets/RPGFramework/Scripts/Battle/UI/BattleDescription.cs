using System.Collections;
using TMPro;
using UnityEngine;

public class BattleDescription : MonoBehaviour, IActive
{
    [SerializeField]
    private GameObject container;

    [SerializeField]
    private TextMeshProUGUI textUI;

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool active) => container.SetActive(active);

    public void SetText(string txt) => textUI.SetText(txt);
}