using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadManager
{
    private readonly DirectoryInfo PlaceForSaves;
    private readonly GameManager Game;

    public SaveLoadManager(GameManager game)
    {
        PlaceForSaves = new(Application.dataPath + @"\Saves");

        if (!PlaceForSaves.Exists) PlaceForSaves.Create();
        Game = game;
    }

    public void Save(int slotId)
    {
        string PathToSave = PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave";

        SaveSlot CellForSave = new()
        {
            Id = slotId,
            IntValues = Game.GameData.IntValues,
            FloatValues = Game.GameData.FloatValues,
            BoolValues = Game.GameData.BoolValues,
            StringValues = Game.GameData.StringValues,
            LocationName = Game.LocationManager.CurrentLocation.Name,
            PlayerPosition = ExplorerManager.GetPlayerPosition(),
            PlayerDirection = ExplorerManager.GetPlayerViewDirection(),
            SavedCharacters = SaveCharacters(
                Game.Character.RegistredCharacters.ToList(),
                Game.Character.Characters.ToList()),
            BlockedLocationEvents = Game.GameData.BlockedLocationEvents
        };

        foreach (InventorySlot slot in Game.Inventory)
        {
            CellForSave.InventoryItems.Add(slot.Item.Tag, slot.Count);
        }

        string JSONSave = JsonUtility.ToJson(CellForSave, true);

        File.WriteAllText(PathToSave, JSONSave);
    }
    public void Load(int slotId)
    {
        string PathToLoad = PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave";

        if (!File.Exists(PathToLoad)) return;

        string JSONSave = File.ReadAllText(PathToLoad);

        SaveSlot slot = JsonUtility.FromJson<SaveSlot>(JSONSave);

        Game.GameData.IntValues = slot.IntValues;
        Game.GameData.FloatValues = slot.FloatValues;
        Game.GameData.BoolValues = slot.BoolValues;
        Game.GameData.StringValues = slot.StringValues;

        Game.GameData.BlockedLocationEvents = slot.BlockedLocationEvents;

        Game.Character.Dispose();
        foreach (var person in slot.SavedCharacters)
            LoadCharacter(person);

        Game.Inventory.Dispose();

        foreach (var item in slot.InventoryItems.data)
        {
            Game.Inventory.AddToItemCount(Game.GameData.Collectables.First(i => i.Tag == item.Key), item.Value);
        }

        LocationInfo location = Game.LocationManager.LoadLocationInfoByName(slot.LocationName);

        Game.LocationManager.ChangeLocation(location, slot.PlayerPosition, slot.PlayerDirection);
    }

    public void SaveConfig(GameConfig gameConfig)
    {
        string PathToSave = PlaceForSaves.FullName + @"\Config.cfg";

        string JSONSave = JsonUtility.ToJson(gameConfig, true);

        using (var PlaceForSave = new StreamWriter(PathToSave, false))
        {
            PlaceForSave.WriteLine(JSONSave);
        }
    }
    public GameConfig? LoadConfig()
    {
        string PathToSave = PlaceForSaves.FullName + @"\Config.cfg";

        string JSONSave;

        if (!File.Exists(PathToSave)) return null;

        using (var PlaceForSave = new StreamReader(PathToSave, false))
        {
            JSONSave = PlaceForSave.ReadToEnd();
        }

        GameConfig config = JsonUtility.FromJson<GameConfig>(JSONSave);

        return config;
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
        RPGCharacter Glek = Game.GameData.Characters.FirstOrDefault(i => i.Tag == SavedCharacter.Tag).Clone() as RPGCharacter;

        Glek.Level = SavedCharacter.Level;
        Glek.Expireance = SavedCharacter.Expirience;
        Glek.ExpireanceBorder = SavedCharacter.ExpirienceBorder;
        Glek.DefaultHeal = SavedCharacter.DefaultHeal;
        Glek.DefaultMana = SavedCharacter.DefaultMana;
        Glek.DefaultDamage = SavedCharacter.DefaultDamage;
        Glek.DefaultDefence = SavedCharacter.DefaultDefence;
        Glek.DefaultAgility = SavedCharacter.DefaultAgility;

        if (SavedCharacter.WeaponTag != string.Empty)
            Glek.WeaponSlot = (RPGWeapon)Game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.WeaponTag);
        
        if (SavedCharacter.HeadTag != string.Empty)
            Glek.HeadSlot = (RPGWerable)Game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.HeadTag);
        
        if (SavedCharacter.BodyTag != string.Empty)
            Glek.BodySlot = (RPGWerable)Game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.BodyTag);
        
        if (SavedCharacter.ShieldTag != string.Empty)
            Glek.ShieldSlot = (RPGWerable)Game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.ShieldTag);
        
        if (SavedCharacter.TalismanTag != string.Empty)
            Glek.TalismanSlot = (RPGWerable)Game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.TalismanTag);

        Glek.Abilities.Clear();
        foreach (var ability in SavedCharacter.Abilities)
        {
            Glek.Abilities.Add(Game.GameData.Abilities.First(i => i.Tag == ability));
        }

        Glek.RemoveAllStates();
        foreach (var state in SavedCharacter.States)
        {
            Glek.AddState(Game.GameData.States.First(i => i.Tag == state));
        }

        if (SavedCharacter.InParty)
            Game.Character.AddCharacter(Glek);
        else 
            Game.Character.RegisterCharacter(Glek);

        Game.Character.GetRegisteredCharacter(Glek).Heal = SavedCharacter.Heal;
        Game.Character.GetRegisteredCharacter(Glek).Mana = SavedCharacter.Mana;
    }
}