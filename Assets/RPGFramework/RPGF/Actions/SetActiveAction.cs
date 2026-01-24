using RPGF.EventSystem;
using RPGF.EventSystem.Attributes;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Actions
{
    [Serializable]
    [GenerateActionNode("Упр. игровым объектом", "Управление игровым объектом на сцене", "Система/Упр. игровым объектом")]
    public class ManageGameObjectAction : ActionBase
    {
        [ActionFieldOption("Объект:", AllowSceneObjects = true)]
        public GameObject gameObject;

        [ActionFieldOption("Включен?")]
        public bool isActive;

        public ManageGameObjectAction() : base()
        {
            isActive = true;
            gameObject = null;
        }

        public override IEnumerator ActionCoroutine()
        {
            gameObject.SetActive(isActive);

            yield break;
        }
    }
}