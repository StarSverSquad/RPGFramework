using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadManager
{
    public readonly DirectoryInfo PlaceForSaves = new DirectoryInfo(Application.dataPath + @"\Saves");

    public SaveLoadManager()
    {
        if (!PlaceForSaves.Exists) PlaceForSaves.Create();
    }

    public void Save(int slotId)
    {
        string PathToSave = PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave";

        SaveSlot CellForSave = new SaveSlot()
        {
            Id = slotId,
            IntValues = GameManager.Instance.GameData.IntValues,
            FloatValues = GameManager.Instance.GameData.FloatValues,
            BoolValues = GameManager.Instance.GameData.BoolValues,
            StringValues = GameManager.Instance.GameData.StringValues,
            LocationName = GameManager.Instance.LocationManager.CurrentLocation.Name,
            PlayerPosition = ExplorerManager.GetPlayerPosition(),
            PlayerDirection = ExplorerManager.GetPlayerViewDirection(),
            SavedCharacters = SaveCharacters(
                GameManager.Instance.Character.RegistredCharacters.ToList(),
                GameManager.Instance.Character.Characters.ToList())
        };

        foreach (InventorySlot slot in GameManager.Instance.Inventory)
        {
            CellForSave.InventoryItems.Add(slot.Item.Tag, slot.Count);
        }

        string JSONSave = JsonUtility.ToJson(CellForSave, true);

        using (var PlaceForSave = new StreamWriter(PathToSave, false))
        {
            PlaceForSave.WriteLine(JSONSave);
        }
    }

    public void Load(int slotId)
    {
        string PathToSave = PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave";

        string JSONSave;

        if (!File.Exists(PathToSave)) return;

        using (var PlaceForSave = new StreamReader(PathToSave, false))
        {
            JSONSave = PlaceForSave.ReadToEnd();
        }

        SaveSlot CellForSave = JsonUtility.FromJson<SaveSlot>(JSONSave);

        GameManager.Instance.GameData.IntValues = CellForSave.IntValues;
        GameManager.Instance.GameData.FloatValues = CellForSave.FloatValues;
        GameManager.Instance.GameData.BoolValues = CellForSave.BoolValues;
        GameManager.Instance.GameData.StringValues = CellForSave.StringValues;

        GameManager.Instance.Character.Dispose();
        foreach (var person in CellForSave.SavedCharacters)
            LoadCharacter(person);

        GameManager.Instance.Inventory.Dispose();

        foreach (var item in CellForSave.InventoryItems.data)
        {
            GameManager.Instance.Inventory.AddToItemCount(GameManager.Instance.GameData.Collectables.First(i => i.Tag == item.Key), item.Value);
        }

        LocationInfo location = GameManager.Instance.LocationManager.LoadLocationInfoByName(CellForSave.LocationName);

        GameManager.Instance.LocationManager.ChangeLocation(location, CellForSave.PlayerPosition, CellForSave.PlayerDirection);
    }

    public void SaveConfig()
    {

    }

    public void LoadConfig()
    {

    }

    public List<CharacterSaveInfo> SaveCharacters(List<RPGCharacter> RegisteredCharacters, List<RPGCharacter> Characters)
    {
        List <CharacterSaveInfo> SavedCharacters = new List <CharacterSaveInfo>();

        foreach (var person in RegisteredCharacters)
        {
            CharacterSaveInfo Saver = new CharacterSaveInfo()
            {
                Tag = person.Tag,
                Heal = person.Heal,
                Mana = person.Mana,
                Level = person.Level,
                Expirience = person.Expireance,
                ExpirienceBorder = person.ExpireanceBorder,
                DefaultHeal = person.DefaultHeal,
                DefaultMana = person.DefaultMana,
                DefaultDamage = person.DefaultDamage,
                DefaultDefence = person.DefaultDefence,
                DefaultAgility = person.DefaultAgility,
                WeaponTag = person.WeaponSlot?.Tag,
                HeadTag = person.HeadSlot?.Tag,
                BodyTag = person.BodySlot?.Tag,
                ShieldTag = person.ShieldSlot?.Tag,
                TalismanTag = person.TalismanSlot?.Tag,
                Abilities = person.Abilities.Select(i => i.Tag).ToList(),
                States = person.States.Select(i => i.Tag).ToList(),
                InParty = Characters.Find(i => i == person)
            };

            Saver.PositionInParty = Saver.InParty ? Characters.IndexOf(person) : -1;

            SavedCharacters.Add(Saver);
        }

        return SavedCharacters;
    }

    public void LoadCharacter(CharacterSaveInfo SavedCharacter)
    {
        RPGCharacter Glek = GameManager.Instance.GameData.Characters.FirstOrDefault(i => i.Tag == SavedCharacter.Tag).Clone() as RPGCharacter;

        Glek.Level = SavedCharacter.Level;
        Glek.Expireance = SavedCharacter.Expirience;
        Glek.ExpireanceBorder = SavedCharacter.ExpirienceBorder;
        Glek.DefaultHeal = SavedCharacter.DefaultHeal;
        Glek.DefaultMana = SavedCharacter.DefaultMana;
        Glek.DefaultDamage = SavedCharacter.DefaultDamage;
        Glek.DefaultDefence = SavedCharacter.DefaultDefence;
        Glek.DefaultAgility = SavedCharacter.DefaultAgility;

        if (SavedCharacter.WeaponTag != string.Empty)
            Glek.WeaponSlot = (RPGWeapon)GameManager.Instance.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.WeaponTag);
        
        if (SavedCharacter.HeadTag != string.Empty)
            Glek.HeadSlot = (RPGWerable)GameManager.Instance.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.HeadTag);
        
        if (SavedCharacter.BodyTag != string.Empty)
            Glek.BodySlot = (RPGWerable)GameManager.Instance.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.BodyTag);
        
        if (SavedCharacter.ShieldTag != string.Empty)
            Glek.ShieldSlot = (RPGWerable)GameManager.Instance.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.ShieldTag);
        
        if (SavedCharacter.TalismanTag != string.Empty)
            Glek.TalismanSlot = (RPGWerable)GameManager.Instance.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.TalismanTag);

        Glek.Abilities.Clear();
        foreach (var ability in SavedCharacter.Abilities)
        {
            Glek.Abilities.Add(GameManager.Instance.GameData.Abilities.First(i => i.Tag == ability));
        }

        Glek.RemoveAllStates();
        foreach (var state in SavedCharacter.States)
        {
            Glek.AddState(GameManager.Instance.GameData.States.First(i => i.Tag == state));
        }

        if (SavedCharacter.InParty)
            GameManager.Instance.Character.AddCharacter(Glek);
        else 
            GameManager.Instance.Character.RegisterCharacter(Glek);

        GameManager.Instance.Character.GetRegisteredCharacter(Glek).Heal = SavedCharacter.Heal;
        GameManager.Instance.Character.GetRegisteredCharacter(Glek).Mana = SavedCharacter.Mana;
    }
}