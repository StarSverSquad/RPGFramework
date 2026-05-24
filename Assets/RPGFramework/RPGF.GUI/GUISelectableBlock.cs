using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RPGF.GUI.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI
{
    public class GUISelectableBlock : GUIBlock
    {
        [Space, Header("Choice block options:")]
        [ReorderableList]
        [SerializeField]
        protected List<GUIInteractable> Elements = new();
        [SerializeField]
        private bool startChoiceOnActivate = false;
        [SerializeField]
        private bool isHorizontal = true;

        public GUIInteractable CurrentElement => Elements[index];
        public int CurrentIndex => index;
        public bool Canceled { get; private set; } = false;

        private int index = 0;
        private Coroutine choiceCoroutine = null;

        [Space]
        [Foldout("Choice block events")]
        public UnityEvent OnSelectionChangedEvent;
        [Foldout("Choice block events")]
        public UnityEvent OnCanceledEvent;
        [Foldout("Choice block events")]
        public UnityEvent<int> OnChoicedEvent;

        public override void Initialize(IGUIManager manager)
        {
            base.Initialize(manager);
        }

        protected override void OnActivate()
        {
            if (startChoiceOnActivate)
                StartChoice();
        }

        protected override void OnDispose()
        {
            index = 0;
            StopChoice();
        }

        protected override void OnDiativate()
        {
            foreach (var item in Elements)
                item.SetFocus(false);

            StopChoice();
        }

        public void StartChoice()
        {
            Elements[index].SetFocus(true);

            Canceled = false;

            StopChoice();
            choiceCoroutine = StartCoroutine(SelectCoroutine());
        }
        public void StopChoice()
        {
            if (choiceCoroutine != null)
            {
                StopCoroutine(choiceCoroutine);
                choiceCoroutine = null;
            }
        }

        public void SetElements(params GUIInteractable[] elements)
        {
            Elements = elements.ToList();
        }

        protected virtual void ChangeSelect(int newIndex)
        {
            CurrentElement.SetFocus(false);

            index = Mathf.Clamp(newIndex, 0, Elements.Count - 1);

            CurrentElement.SetFocus(true);

            OnSelectionChanged();
            OnSelectionChangedEvent?.Invoke();
        }

        private IEnumerator SelectCoroutine()
        {
            bool end = false;

            while (!end)
            {
                yield return null;

                OnSelectUpdate();

                if (isHorizontal)
                {
                    if (Input.GetKeyDown(Global.BaseOptions.MoveLeft))
                    {
                        ChangeSelect(index - 1);
                    }
                    else if (Input.GetKeyDown(Global.BaseOptions.MoveRight))
                    {
                        ChangeSelect(index + 1);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(Global.BaseOptions.MoveDown))
                    {
                        ChangeSelect(index + 1);

                    }
                    else if (Input.GetKeyDown(Global.BaseOptions.MoveUp))
                    {
                        ChangeSelect(index - 1);
                    }
                }

                if (Input.GetKeyDown(Global.BaseOptions.Accept))
                {
                    end = true;

                    CurrentElement.Select();

                    OnChoiced(index);
                    OnChoicedEvent?.Invoke(index);
                }

                if (Input.GetKeyDown(Global.BaseOptions.Cancel))
                {
                    end = true;

                    Canceled = true;

                    CurrentElement.Cancel();

                    OnCanceled();
                    OnCanceledEvent?.Invoke();
                }
            }

            choiceCoroutine = null;
        }

        protected virtual void OnChoiced(int index) { }
        protected virtual void OnSelectionChanged() { }
        protected virtual void OnCanceled() { }
        protected virtual void OnSelectUpdate() { }
    }
}
