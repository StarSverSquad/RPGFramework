using System.Collections.Generic;
using RPGF.Core;
using RPGF.Core.Character;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using RPGF.Misc;
using RPGF.RPG;
using RPGF.Shared;
using UnityEngine;

namespace RPGF.Overworld
{
    public class OverworldDamageService : IService
    {
        private const float DamageTextHeightOffset = 1f;

        [Inject]
        private readonly BaseOptions _baseOptions = null!;
        [Inject]
        private readonly AudioManager _audio = null!;
        [Inject]
        private readonly CharacterManager _characterManager = null!;
        [Inject]
        private readonly SharedManager _shared = null!;

        public void DamageParty(int damage)
        {
            var characters = _characterManager.Characters;
            var models = _characterManager.Models;

            for (int i = 0; i < characters.Length; i++)
            {
                Vector3 position = GetCharacterPosition(i, models);
                ApplyDamage(characters[i], position, damage);
            }
        }

        public void Damage(int damage, RPGCharacter character)
        {
            if (character == null)
                return;

            var characters = _characterManager.Characters;
            int index = -1;

            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].Tag == character.Tag)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return;

            Vector3 position = GetCharacterPosition(index, _characterManager.Models);
            ApplyDamage(characters[index], position, damage);
        }

        private void ApplyDamage(RPGCharacter character, Vector3 position, int damage)
        {
            int resultDamage = character.GiveDamage(damage);

            SpawnFallingText(position + Vector3.up * DamageTextHeightOffset, resultDamage.ToString());

            if (_baseOptions.HurtSound != null)
                _audio.PlaySE(_baseOptions.HurtSound);
        }

        private Vector3 GetCharacterPosition(int index, List<PlayableCharacterModelController> models)
        {
            if (index >= 0 && index < models.Count && models[index] != null)
                return models[index].transform.position;

            return OverworldManager.GetPlayerPosition3D();
        }

        private void SpawnFallingText(Vector3 position, string text)
        {
            if (_baseOptions.DamageText == null)
                return;

            GameObject obj = Object.Instantiate(
                _baseOptions.DamageText.gameObject,
                position,
                Quaternion.identity,
                _shared.Canvas.transform);

            obj.transform.position = position;

            FallingText fallingText = obj.GetComponent<FallingText>();
            fallingText.Invoke(text, Color.white, Color.red);
        }
    }
}
