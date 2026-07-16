using NaughtyAttributes;
using RPGF.Domain;
using RPGF.Domain.DI;
using RPGF.Misc;
using UnityEngine;

namespace RPGF.Core
{
    [CreateAssetMenu(fileName = "Config", menuName = "RPGFramework/Config")]
    public class BaseOptions : ScriptableObject, Injectable
    {
        [Header("Базовое управление")]
        public KeyCode MoveUp = KeyCode.UpArrow;
        public KeyCode MoveDown = KeyCode.DownArrow;
        public KeyCode MoveLeft = KeyCode.LeftArrow;
        public KeyCode MoveRight = KeyCode.RightArrow;
        [Space]
        public KeyCode Run = KeyCode.LeftShift;
        [Space]
        public KeyCode Accept = KeyCode.Z;
        public KeyCode Cancel = KeyCode.X;
        public KeyCode Additional = KeyCode.C;

        [Header("Стартовые переменные")]
        public CustomDictionary<int> IntValues = new CustomDictionary<int>();
        public CustomDictionary<float> FloatValues = new CustomDictionary<float>();
        public CustomDictionary<bool> BoolValues = new CustomDictionary<bool>();
        public CustomDictionary<string> StringValues = new CustomDictionary<string>();

        [Header("Базовые сцены")]
        [Scene]
        public string GameStartScene;
        [Scene]
        public string MainMenuScene;
        [Scene]
        public string GameOverScene;

        [Header("Общие объекты")]
        public FallingText DamageText;
        public AudioClip HurtSound;
    }
}