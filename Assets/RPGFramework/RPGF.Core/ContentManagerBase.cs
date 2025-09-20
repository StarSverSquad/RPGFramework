using RPGF.Domain.Interfaces;
using UnityEngine;

public abstract class ContentManagerBase : MonoBehaviour, IActive
{
    [SerializeField]
    private GameObject container;

    public abstract void InitializeChild();

    public void SetActive(bool active) => container.SetActive(active);
}
