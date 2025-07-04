using RPGF.Inventory;
using RPGF.RPG;
using RPGF.SaveLoad;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveLoadService
{
    private readonly GameManager _game;
    private readonly GameFilesService _gameFiles;


    public SaveLoadService(GameManager game, GameFilesService gameFiles)
    {

        _game = game;
        _gameFiles = gameFiles;
    }

    public void GameSave(int slotId)
    {
        GameSlotData CellForSave = new()
        {
            Id = slotId,
            IntValues = _game.GameData.IntValues,
            FloatValues = _game.GameData.FloatValues,
            BoolValues = _game.GameData.BoolValues,
            StringValues = _game.GameData.StringValues,
            LocationName = _game.LocationManager.CurrentLocation.Name,
            PlayerPosition = ExplorerManager.GetPlayerPosition(),
            PlayerDirection = ExplorerManager.GetPlayerViewDirection(),
            SavedCharacters = SaveCharacters(
                _game.Character.RegistredCharacters.ToList(),
                _game.Character.Characters.ToList()),
            BlockedLocationEvents = _game.GameData.BlockedLocationEvents
        };

        foreach (InventorySlotData slot in _game.Inventory)
        {
            CellForSave.InventoryItems.Add(slot.Item.Tag, slot.Count);
        }

        string JSONSave = JsonUtility.ToJson(CellForSave, true);
    }
    public void GameLoad(int slotId)
    {
        GameSlotData slot = null;

        _game.GameData.IntValues = slot.IntValues;
        _game.GameData.FloatValues = slot.FloatValues;
        _game.GameData.BoolValues = slot.BoolValues;
        _game.GameData.StringValues = slot.StringValues;

        _game.GameData.BlockedLocationEvents = slot.BlockedLocationEvents;

        _game.Character.Dispose();
        foreach (var person in slot.SavedCharacters)
            LoadCharacter(person);

        _game.Inventory.Dispose();

        foreach (var item in slot.InventoryItems.data)
        {
            _game.Inventory.AddToItemCount(_game.GameData.Collectables.First(i => i.Tag == item.Key), item.Value);
        }

        LocationInfo location = _game.LocationManager.LoadLocationInfoByName(slot.LocationName);

        _game.LocationManager.ChangeLocation(location, slot.PlayerPosition, slot.PlayerDirection);
    }

    public bool HasFastSaveKey(string key)
    {
        return false;
    }
    public int GetFastSave(string key)
    {
        return 0;
    }
    public void SetFastSave(string key, int value)
    {

    }

    public void SaveConfig(GameConfigData gameConfig)
    {

    }
    public GameConfigData LoadConfig()
    {

        return null;
    }

    public List<CharacterSaveData> SaveCharacters(List<RPGCharacter> RegisteredCharacters, List<RPGCharacter> Characters)
    {
        List <CharacterSaveData> SavedCharacters = new List <CharacterSaveData>();

        foreach (var person in RegisteredCharacters)
        {
            CharacterSaveData Saver = new CharacterSaveData()
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
    public void LoadCharacter(CharacterSaveData SavedCharacter)
    {
        RPGCharacter Glek = _game.GameData.Characters.FirstOrDefault(i => i.Tag == SavedCharacter.Tag).Clone() as RPGCharacter;

        Glek.Level = SavedCharacter.Level;
        Glek.Expireance = SavedCharacter.Expirience;
        Glek.ExpireanceBorder = SavedCharacter.ExpirienceBorder;
        Glek.DefaultHeal = SavedCharacter.DefaultHeal;
        Glek.DefaultMana = SavedCharacter.DefaultMana;
        Glek.DefaultDamage = SavedCharacter.DefaultDamage;
        Glek.DefaultDefence = SavedCharacter.DefaultDefence;
        Glek.DefaultAgility = SavedCharacter.DefaultAgility;

        if (SavedCharacter.WeaponTag != string.Empty)
            Glek.WeaponSlot = (RPGWeapon)_game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.WeaponTag);
        
        if (SavedCharacter.HeadTag != string.Empty)
            Glek.HeadSlot = (RPGWerable)_game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.HeadTag);
        
        if (SavedCharacter.BodyTag != string.Empty)
            Glek.BodySlot = (RPGWerable)_game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.BodyTag);
        
        if (SavedCharacter.ShieldTag != string.Empty)
            Glek.ShieldSlot = (RPGWerable)_game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.ShieldTag);
        
        if (SavedCharacter.TalismanTag != string.Empty)
            Glek.TalismanSlot = (RPGWerable)_game.GameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.TalismanTag);

        Glek.Abilities.Clear();
        foreach (var ability in SavedCharacter.Abilities)
        {
            Glek.Abilities.Add(_game.GameData.Abilities.First(i => i.Tag == ability));
        }

        Glek.RemoveAllStates();
        foreach (var state in SavedCharacter.States)
        {
            Glek.AddState(_game.GameData.States.First(i => i.Tag == state));
        }

        if (SavedCharacter.InParty)
            _game.Character.AddCharacter(Glek);
        else 
            _game.Character.RegisterCharacter(Glek);

        _game.Character.GetRegisteredCharacter(Glek).Heal = SavedCharacter.Heal;
        _game.Character.GetRegisteredCharacter(Glek).Mana = SavedCharacter.Mana;
    }
}