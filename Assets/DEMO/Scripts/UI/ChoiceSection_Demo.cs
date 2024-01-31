using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ChoiceSection_Demo : UISectionBase
{
    public List<UIElementBase> Elements = new List<UIElementBase>();

    public List<string> Items = new List<string>();

    private bool allItems = false;

    public override void Initialize()
    {
        base.Initialize();

        StartCoroutine(Init());
    }

    public void InitializeWearable()
    {
        Items = GameManager.Instance.inventory.GetWerables().Select(i => i.Name).ToList();

        UpdateChoice();

        allItems = false;
    }

    public void InitializeCollectables()
    {
        Items = GameManager.Instance.inventory.Slots.Select(i => i.Item.Name).ToList();

        UpdateChoice();

        allItems = true;
    }

    private void UpdateChoice()
    {
        int count = Mathf.Min(Items.Count, Elements.Count);

        foreach (var item in Elements)
        {
            item.GetComponentInChildren<Image>().enabled = false;
            item.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
            

        for (int i = 0; i < count; i++)
        {
            InventorySlot slot = GameManager.Instance.inventory.GetSlotByItemName(Items[i]);

            Elements[i].GetComponentInChildren<Image>().sprite = slot.Item.Icon;
            Elements[i].GetComponentInChildren<TextMeshProUGUI>().text = slot.Item.Name + (slot.Count > 0 ? $" {slot.Count}x" : "");

            if (i != 0)
                Elements[i].UpTransmition = Elements[i - 1];
            else
                Elements[i].UpTransmition = null;

            if (i != count - 1)
                Elements[i].DownTransmition = Elements[i + 1];
            else
                Elements[i].DownTransmition = null;

            Elements[i].GetComponentInChildren<Image>().enabled = true;
            Elements[i].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }

    public void Accept()
    {

    }

    public override bool TransmitionDown()
    {


        return base.TransmitionDown();
    }

    public override bool TransmitionUp()
    {


        return base.TransmitionUp();
    }

    private IEnumerator Init()
    {
        yield return new WaitForFixedUpdate();

        if (GameManager.Instance.inventory.Slots.Length == 0 && allItems)
        {
            Deinitialize();

            yield break;
        }
        else if (GameManager.Instance.inventory.GetWerables().Count == 0 && !allItems)
        {
            Deinitialize();

            yield break;
        }
    }
}
