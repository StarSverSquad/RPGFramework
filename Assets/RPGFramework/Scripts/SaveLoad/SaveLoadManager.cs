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
    //public StreamWriter PlaceForSave; -- в раздумиях на надобностью применения

    public void Start()
    {
        if (!PlaceForSaves.Exists) PlaceForSaves.Create();
    }

    public void Save(int slotId)
    {
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
                GameManager.Instance.character.Characters.ToList())
        };

        string JSONSave = JsonUtility.ToJson(CellForSave);

        using (var PlaceForSave = new StreamWriter(PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave", false))
        {
            PlaceForSave.WriteLine(JSONSave);
        }
    }

    public void Load(int slotId)
    {
        string JSONSave;

        using (var PlaceForSave = new StreamReader(PlaceForSaves.FullName + @"\Slot" + slotId.ToString() + ".glaksave", false))
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

            foreach (var person in CellForSave.SavedCharacters)
                LoadCharacter(person);
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
            Glek.WeaponSlot.Name = SavedCharacter.WeaponName;
            //Glek.HeadName = SavedCharacter.HeadSlot?.Name;
            //Glek.BodyName = SavedCharacter.BodySlot?.Name;
            //Glek.ShieldName = SavedCharacter.ShieldSlot?.Name;
            //Glek.TalismanName = SavedCharacter.TalismanSlot?.Name;
            //Glek.Abilities = SavedCharacter.Abilities;
            //Glek.States = SavedCharacter.States.Select(i => i.Name).ToList();
        }

        if (SavedCharacter.InParty)
        {

        }
        

    }
}