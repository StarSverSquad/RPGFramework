using UnityEngine;
using UnityEngine.UIElements;

public class TittleMenuScript : MonoBehaviour
{
    [SerializeField]
    private UIDocument document;

    private void Start()
    {
        var menuElement = document.rootVisualElement.Q("menu");

    }
}
