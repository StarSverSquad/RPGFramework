using RPGF.RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharAbilityItem : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _name;

    public void SetData(RPGAbility ability)
    {
        _icon.sprite = ability.Icon;
        _name.text = ability.Name;
    }
}
