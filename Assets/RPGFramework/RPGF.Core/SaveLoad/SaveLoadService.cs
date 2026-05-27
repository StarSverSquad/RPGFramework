using System;
using System.Collections.Generic;
using System.Linq;
using RPGF.Core.Character;
using RPGF.Core.Inventory;
using RPGF.Core.Location;
using RPGF.Domain.DI;
using RPGF.Explorer;
using RPGF.RPG;
using UnityEngine;

namespace RPGF.Core.SaveLoad
{
    public class SaveLoadService : ISupportDI
    {
        [Inject]
        private readonly CharacterService _characters = null!;
        [Inject]
        private readonly GameFilesService _gameFiles = null!;
        [Inject]
        private readonly InventoryService _inventory = null!;
        [Inject]
        private readonly GameCommonDataService _commonData = null!;
        [Inject]
        private readonly GlobalLocationManager _location = null!;
        [Inject]
        private readonly GameData _gameData = null!;

        public void GameSave(int slotId)
        {
            GameSlotData slotData = new()
            {
                Id = slotId,
                IntValues = _gameData.IntValues,
                FloatValues = _gameData.FloatValues,
                BoolValues = _gameData.BoolValues,
                StringValues = _gameData.StringValues,
                LocationTag = _location.CurrentLocation.Tag,
                SaveDateTime = DateTime.UtcNow.ToString(),
                PlayerPosition = ExplorerManager.GetPlayerPosition(),
                PlayerDirection = ExplorerManager.GetPlayerViewDirection(),
                SavedCharacters = SerializeCharacters(
                    _characters.RegistredCharacters.ToList(),
                    _characters.Characters.ToList()),
                BlockedLocationEvents = _gameData.BlockedLocationEvents
            };

            foreach (InventorySlotData inventorySlot in _inventory)
            {
                slotData.InventoryItems.Add(inventorySlot.Item.Tag, inventorySlot.Count);
            }

            _gameFiles.SaveSlot($"Slot{slotId}", slotData);
        }
        public void GameLoad(int slotId)
        {
            var slotData = _gameFiles.LoadSlot($"Slot{slotId}");

            _gameData.IntValues = slotData.IntValues;
            _gameData.FloatValues = slotData.FloatValues;
            _gameData.BoolValues = slotData.BoolValues;
            _gameData.StringValues = slotData.StringValues;

            _gameData.BlockedLocationEvents = slotData.BlockedLocationEvents;

            _characters.Dispose();
            foreach (var person in slotData.SavedCharacters)
                ParseAndApplyCharacters(person);

            _inventory.Dispose();
            foreach (var item in slotData.InventoryItems.data)
                _inventory.AddToItemCount(_gameData.Collectables.First(i => i.Tag == item.Key), item.Value);

            var location = _location.LoadLocationInfoByTag(slotData.LocationTag);
            if (location == null)
            {
                Debug.LogError($"Location with tag {slotData.LocationTag} not found");
                return;
            }

            _location.ChangeLocation(location, slotData.PlayerPosition, slotData.PlayerDirection);
        }

        public void FastSave()
        {
            int lastSlot = _commonData.CommonData.LastLoadedSlotId;

            if (lastSlot < 0)
                return;

            GameSave(lastSlot);
        }
        public void FastLoad()
        {
            int lastSlot = _commonData.CommonData.LastLoadedSlotId;

            if (lastSlot < 0)
                return;

            GameLoad(lastSlot);
        }

        private List<CharacterSaveData> SerializeCharacters(List<RPGCharacter> RegisteredCharacters, List<RPGCharacter> Characters)
        {
            List<CharacterSaveData> SavedCharacters = new List<CharacterSaveData>();

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
                    DefaultDefence = person.DefaultDefense,
                    DefaultAgility = person.DefaultAgility,
                    WeaponTag = person.WeaponSlot?.Tag,
                    HeadTag = person.HeadSlot?.Tag,
                    BodyTag = person.BodySlot?.Tag,
                    ShieldTag = person.ShieldSlot?.Tag,
                    TalismanTag = person.AccessorySlot?.Tag,
                    Abilities = person.Abilities.Select(i => i.Tag).ToList(),
                    States = person.States.Select(i => i.Tag).ToList(),
                    InParty = Characters.Find(i => i == person)
                };

                Saver.PositionInParty = Saver.InParty ? Characters.IndexOf(person) : -1;

                SavedCharacters.Add(Saver);
            }

            return SavedCharacters;
        }
        private void ParseAndApplyCharacters(CharacterSaveData SavedCharacter)
        {
            RPGCharacter Glek = _gameData.Characters.FirstOrDefault(i => i.Tag == SavedCharacter.Tag).Clone() as RPGCharacter;

            Glek.Level = SavedCharacter.Level;
            Glek.Expireance = SavedCharacter.Expirience;
            Glek.ExpireanceBorder = SavedCharacter.ExpirienceBorder;
            Glek.DefaultHeal = SavedCharacter.DefaultHeal;
            Glek.DefaultMana = SavedCharacter.DefaultMana;
            Glek.DefaultDamage = SavedCharacter.DefaultDamage;
            Glek.DefaultDefense = SavedCharacter.DefaultDefence;
            Glek.DefaultAgility = SavedCharacter.DefaultAgility;

            if (SavedCharacter.WeaponTag != string.Empty)
                Glek.WeaponSlot = (RPGWeapon)_gameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.WeaponTag);

            if (SavedCharacter.HeadTag != string.Empty)
                Glek.HeadSlot = (RPGWerable)_gameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.HeadTag);

            if (SavedCharacter.BodyTag != string.Empty)
                Glek.BodySlot = (RPGWerable)_gameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.BodyTag);

            if (SavedCharacter.ShieldTag != string.Empty)
                Glek.ShieldSlot = (RPGWerable)_gameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.ShieldTag);

            if (SavedCharacter.TalismanTag != string.Empty)
                Glek.AccessorySlot = (RPGWerable)_gameData.Collectables.FirstOrDefault(i => i.Tag == SavedCharacter.TalismanTag);

            Glek.Abilities.Clear();
            foreach (var ability in SavedCharacter.Abilities)
            {
                Glek.Abilities.Add(_gameData.Abilities.First(i => i.Tag == ability));
            }

            Glek.RemoveAllStates();
            foreach (var state in SavedCharacter.States)
            {
                Glek.AddState(_gameData.States.First(i => i.Tag == state));
            }

            if (SavedCharacter.InParty)
                _characters.AddCharacter(Glek);
            else
                _characters.RegisterCharacter(Glek);

            _characters.GetRegisteredCharacter(Glek).Heal = SavedCharacter.Heal;
            _characters.GetRegisteredCharacter(Glek).Mana = SavedCharacter.Mana;
        }
    }
}
