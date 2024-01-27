using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public void Save(int slotId)
    {
        SaveSlot CellForSave = new SaveSlot() 
        {
            Id = slotId, 
            LocationName = GameManager.Instance.locationManager.CurrentLocation.Name,
            SavedCharacters = SaveCharacters(
            GameManager.Instance.character.RegistredCharacters.ToList(),
            GameManager.Instance.character.Characters.ToList())
        };
    }

    public void Load(int slotId)
    {

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
                DefaultAgility = person.Agility,
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
}