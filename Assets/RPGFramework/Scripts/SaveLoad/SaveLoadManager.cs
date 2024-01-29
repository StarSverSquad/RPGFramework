using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TreeEditor;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private DirectoryInfo PlaceForSaves = new DirectoryInfo(Application.dataPath + @"\Saves");
    public string PathToSave;

    public void Start()
    {
        if (!PlaceForSaves.Exists) PlaceForSaves.Create();
    }

    public void Save(int slotId)
    {
        PathToSave = PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave";

        SaveSlot CellForSave = new SaveSlot()
        {
            Id = slotId,
            IntValues = GameManager.Instance.GameData.IntValues,
            FloatValues = GameManager.Instance.GameData.FloatValues,
            BoolValues = GameManager.Instance.GameData.BoolValues,
            StringValues = GameManager.Instance.GameData.StringValues,
            LocationName = GameManager.Instance.locationManager.CurrentLocation.Name,
            SavedCharacters = SaveCharacters(
                GameManager.Instance.character.RegistredCharacters.ToList(),
                GameManager.Instance.character.Characters.ToList()),
            PlayerPosition = ExplorerManager.GetPlayerPosition()
        };

        foreach (InventorySlot slot in GameManager.Instance.inventory)
        {
            CellForSave.InventoryItems.Add(slot.Item.Name, slot.Count);
        }

        string JSONSave = JsonUtility.ToJson(CellForSave);

        using (var PlaceForSave = new StreamWriter(PathToSave, false))
        {
            PlaceForSave.WriteLine(JSONSave);
        }
    }

    public void Load(int slotId)
    {
        PathToSave = PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave";

        string JSONSave;

        if (!File.Exists(PathToSave)) return;

        using (var PlaceForSave = new StreamReader(PathToSave, false))
        {
            JSONSave = PlaceForSave.ReadToEnd();
        }

        SaveSlot CellForSave = JsonUtility.FromJson<SaveSlot>(JSONSave);

        {
            GameManager.Instance.GameData.IntValues = CellForSave.IntValues;
            GameManager.Instance.GameData.FloatValues = CellForSave.FloatValues;
            GameManager.Instance.GameData.BoolValues = CellForSave.BoolValues;
            GameManager.Instance.GameData.StringValues = CellForSave.StringValues;
            GameManager.Instance.locationManager.CurrentLocation.Name = CellForSave.LocationName;
            ExplorerManager.Instance.playerManager.TeleportToVector(CellForSave.PlayerPosition);

            foreach (var person in CellForSave.SavedCharacters)
                LoadCharacter(person);

            GameManager.Instance.inventory.Dispose();

            foreach (var item in CellForSave.InventoryItems.data)
            {
                GameManager.Instance.inventory.AddToItemCount(GameManager.Instance.GameData.Collectables.First(i => i.Name == item.Key), item.Value);
            }
        }

    }

    public List<CharacterSaveInfo> SaveCharacters(List<RPGCharacter> RegisteredCharacters, List<RPGCharacter> Characters)
    {
        List < CharacterSaveInfo > SavedCharacters = new List < CharacterSaveInfo >();

        foreach (var person in RegisteredCharacters)
        {
            CharacterSaveInfo Saver = new CharacterSaveInfo()
            {
                Name = person.Name,
                Heal = person.Heal,
                Level = person.Level,
                Expirience = person.Expireance,
                ExpirienceBorder = person.ExpireanceBorder,
                DefaultHeal = person.DefaultHeal,
                DefaultMana = person.DefaultMana,
                DefaultDamage = person.DefaultDamage,
                DefaultDefence = person.DefaultDefence,
                DefaultAgility = person.DefaultAgility,
                WeaponName = person.WeaponSlot?.Name,
                HeadName = person.HeadSlot?.Name,
                BodyName = person.BodySlot?.Name,
                ShieldName = person.ShieldSlot?.Name,
                TalismanName = person.TalismanSlot?.Name,
                Abilities = person.Abilities.Select(i => i.Name).ToList(),
                States = person.States.Select(i => i.Name).ToList(),
                InParty = Characters.Find(i => i == person)
            };

            Saver.PositionInParty = Saver.InParty ? Characters.IndexOf(person) : -1;

            SavedCharacters.Add(Saver);
        }

        return SavedCharacters;
    }

    public void LoadCharacter(CharacterSaveInfo SavedCharacter)
    {
        RPGCharacter Glek = Instantiate(GameManager.Instance.GameData.Characters.FirstOrDefault(i => i.Name == SavedCharacter.Name));

        {
            Glek.Name = SavedCharacter.Name;
            Glek.Heal = SavedCharacter.Heal;
            Glek.Level = SavedCharacter.Level;
            Glek.Expireance = SavedCharacter.Expirience;
            Glek.ExpireanceBorder = SavedCharacter.ExpirienceBorder;
            Glek.DefaultHeal = SavedCharacter.DefaultHeal;
            Glek.DefaultMana = SavedCharacter.DefaultMana;
            Glek.DefaultDamage = SavedCharacter.DefaultDamage;
            Glek.DefaultDefence = SavedCharacter.DefaultDefence;
            Glek.DefaultAgility = SavedCharacter.DefaultAgility;
            Glek.WeaponSlot = (RPGWeapon)GameManager.Instance.GameData.Collectables.First(i => i.Name == SavedCharacter.WeaponName);
            Glek.HeadSlot = (RPGWerable)GameManager.Instance.GameData.Collectables.First(i => i.Name == SavedCharacter.HeadName);
            Glek.BodySlot = (RPGWerable)GameManager.Instance.GameData.Collectables.First(i => i.Name == SavedCharacter.BodyName);
            Glek.ShieldSlot = (RPGWerable)GameManager.Instance.GameData.Collectables.First(i => i.Name == SavedCharacter.ShieldName);
            Glek.TalismanSlot = (RPGWerable)GameManager.Instance.GameData.Collectables.First(i => i.Name == SavedCharacter.TalismanName);

            Glek.Abilities.Clear();
            foreach (var ability in SavedCharacter.Abilities)
            {
                Glek.Abilities.Add(GameManager.Instance.GameData.Abilities.First(i => i.Name == ability));
            }

            Glek.RemoveAllStates();
            foreach (var state in SavedCharacter.States)
            {
                Glek.AddState(GameManager.Instance.GameData.States.First(i=> i.Name == state));
            }
        }

        if (SavedCharacter.InParty)
        {
            GameManager.Instance.character.AddCharacter(Glek);
        }
        else GameManager.Instance.character.RegisterCharacter(Glek);

    }
}