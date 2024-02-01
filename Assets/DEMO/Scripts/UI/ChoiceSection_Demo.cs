using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEditor.VersionControl.Asset;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ChoiceSection_Demo : UISectionBase
{
    public List<UIElementBase> Elements = new List<UIElementBase>();

    public List<string> Items = new List<string>();

    [Space]
    [SerializeField]
    private GameUIManager gameUIManager;
    [SerializeField]
    private MainSection_Demo mainSection;

    [SerializeField]
    private GameObject[] arrows = new GameObject[2];

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioClip errorSound;
    [SerializeField]
    private AudioClip consumeSound;
    [SerializeField]
    private AudioClip equipSound;
    [SerializeField]
    private AudioClip selectSound;

    private bool allItems = false;

    private int currentElementId = 0;
    private int startIndex = 0;

    public override void Initialize()
    {
        base.Initialize();

        StartCoroutine(Init());
    }

    public void InitializeWearable()
    {
        Items = GameManager.Instance.inventory.GetWerables().Select(i => i.Name).ToList();

        startIndex = 0;
        currentElementId = 0;

        UpdateChoice();

        allItems = false;
    }

    public void InitializeCollectables()
    {
        Items = GameManager.Instance.inventory.Slots.Select(i => i.Item.Name).ToList();

        startIndex = 0;
        currentElementId = 0;

        UpdateChoice();

        allItems = true;
    }

    private void UpdateChoice()
    {
        int count = Mathf.Min(Items.Count, Elements.Count);

        arrows[0].SetActive(startIndex != 0);
        arrows[1].SetActive(startIndex + Elements.Count - 1 < Items.Count - 1);

        foreach (var item in Elements)
        {
            item.GetComponentInChildren<Image>().enabled = false;
            item.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        }
            
        for (int i = startIndex; i < count + startIndex; i++)
        {
            int elementIndex = i - startIndex;

            InventorySlot slot = GameManager.Instance.inventory.GetSlotByItemName(Items[i]);

            Elements[elementIndex].GetComponentInChildren<Image>().sprite = slot.Item.Icon;
            Elements[elementIndex].GetComponentInChildren<TextMeshProUGUI>().text = slot.Item.Name + (slot.Count > 1 ? $" {slot.Count}x" : "");

            Elements[elementIndex].UpTransmition = elementIndex != 0 ? Elements[elementIndex - 1] : null;
            Elements[elementIndex].DownTransmition = elementIndex != count - 1 ? Elements[elementIndex + 1] : null;

            Elements[elementIndex].GetComponentInChildren<Image>().enabled = true;
            Elements[elementIndex].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }

    public void Accept()
    {
        if (GameManager.Instance.inventory.Slots.Length <= 0 || GameManager.Instance.character.Characters.Length <= 0)
            return;

        InventorySlot slot = GameManager.Instance.inventory[Items[currentElementId]];

        RPGCharacter character = GameManager.Instance.character.Characters[0];

        if (allItems)
        {
            if (slot.Item.Usage == RPGCollectable.Usability.Noway || slot.Item.Usage == RPGCollectable.Usability.Battle)
            {
                GameManager.Instance.gameAudio.PlaySE(errorSound);
                return;
            }

            if (slot.Item is RPGConsumed consumed)
            {
                GameManager.Instance.gameAudio.PlaySE(consumeSound);

                ExplorerManager.Instance.ItemConsumer.CosumeItem(consumed, character, character);
            }
            else
            {
                GameManager.Instance.gameAudio.PlaySE(selectSound);

                if (slot.Item.Event != null)
                    ExplorerManager.Instance.eventHandler.InvokeEvent(slot.Item.Event);
            }

            animator.SetTrigger("OUT_INSTANCE");

            gameUIManager.Close();
        }
        else
        {
            RPGWerable werable = slot.Item as RPGWerable;

            switch (werable.UsedOn)
            {
                case RPGWerable.UsedType.Head:
                    if (character.HeadSlot != null)
                        GameManager.Instance.inventory.AddToItemCount(character.HeadSlot, 1);

                    GameManager.Instance.inventory.AddToItemCount(werable, -1);

                    character.HeadSlot = werable;
                    break;
                case RPGWerable.UsedType.Body:
                    if (character.BodySlot != null)
                        GameManager.Instance.inventory.AddToItemCount(character.BodySlot, 1);

                    GameManager.Instance.inventory.AddToItemCount(werable, -1);

                    character.BodySlot = werable;
                    break;
                case RPGWerable.UsedType.Shield:
                    if (character.ShieldSlot != null)
                        GameManager.Instance.inventory.AddToItemCount(character.ShieldSlot, 1);

                    GameManager.Instance.inventory.AddToItemCount(werable, -1);

                    character.ShieldSlot = werable;
                    break;
                case RPGWerable.UsedType.Talisman:
                    if (character.TalismanSlot != null)
                        GameManager.Instance.inventory.AddToItemCount(character.TalismanSlot, 1);

                    GameManager.Instance.inventory.AddToItemCount(werable, -1);

                    character.TalismanSlot = werable;
                    break;
                case RPGWerable.UsedType.Weapon:
                    if (werable is RPGWeapon weapon)
                    {
                        if (character.WeaponSlot != null)
                            GameManager.Instance.inventory.AddToItemCount(character.WeaponSlot, 1);

                        GameManager.Instance.inventory.AddToItemCount(weapon, -1);

                        character.WeaponSlot = weapon;
                    }
                    else
                    {
                        Debug.LogError("Is not the weapon! [RPGWearable != RPGWeapon]");

                        return;
                    }
                        
                    break;
                default:
                    Debug.LogError("Unknown werable slot!");
                    return;
            }

            animator.SetTrigger("OUT_INSTANCE");

            character.UpdateStats();

            GameManager.Instance.gameAudio.PlaySE(equipSound);

            if ((slot.Item.Usage == RPGCollectable.Usability.Explorer || slot.Item.Usage == RPGCollectable.Usability.Any)
                && slot.Item.Event != null)
            {
                ExplorerManager.Instance.eventHandler.InvokeEvent(slot.Item.Event);

                gameUIManager.Close();
            }
            else
            {
                mainSection.UpdateInfo();

                Deinitialize();
            }
        }
    }

    public void Change()
    {
        description.text = GameManager.Instance.inventory[Items[currentElementId]].Item.Description;
    }

    public override bool TransmitionDown()
    {
        if (!base.TransmitionDown())
            return false;

        if (currentElementId >= Items.Count - 1)
            return false;

        currentElementId++;

        if (currentElementId > startIndex + Elements.Count - 1)
        {
            startIndex++;
            UpdateChoice();

            OnChange.Invoke();

            return false;
        }

        return true;
    }

    public override bool TransmitionUp()
    {
        if (!base.TransmitionUp())
            return false;

        if (currentElementId == 0)
            return false;

        currentElementId--;

        if (currentElementId < startIndex)
        {
            startIndex--;
            UpdateChoice();

            OnChange.Invoke();

            return false;
        }

        return true;
    }

    private IEnumerator Init()
    {
        yield return new WaitForFixedUpdate();

        if (GameManager.Instance.inventory.Slots.Length == 0 && allItems)
        {
            animator.SetTrigger("OUT_INSTANCE");

            Deinitialize();

            yield break;
        }
        else if (GameManager.Instance.inventory.GetWerables().Count == 0 && !allItems)
        {
            animator.SetTrigger("OUT_INSTANCE");

            Deinitialize();

            yield break;
        }

        Change();
    }
}
