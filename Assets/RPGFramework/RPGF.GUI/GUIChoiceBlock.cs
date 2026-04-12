using RPGF.GUI.Abstractions;
using RPGF.GUI.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI
{
    public class GUIChoiceBlock : GUIBlockBase
    {
        [Space, Header("Choice block options:")]
        [SerializeField]
        protected List<GUIInteractableBase> Elements = new();
        [SerializeField]
        private bool startChoiceOnActivate = false;
        [SerializeField]
        private bool isHorizontal = true;

        public GUIInteractableBase CurrentElement => Elements[index];
        public int CurrentElementIndex => index;

        private int index = 0;
        private Coroutine choiceCoroutine = null;

        [Space]
        public UnityEvent OnSelectionChangedEvent;
        public UnityEvent OnCanceledEvent;
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

            StopChoice();
            choiceCoroutine = StartCoroutine(ChoiceCoroutine());
        }
        public void StopChoice()
        {
            if (choiceCoroutine != null)
            {
                StopCoroutine(choiceCoroutine);
                choiceCoroutine = null;
            }
        }

        public void SetElements(params GUIInteractableBase[] elements)
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

        private IEnumerator ChoiceCoroutine()
        {
            bool end = false;

            while (!end)
            {
                yield return null;

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

                    CurrentElement.Cancel();

                    OnCanceled();
                    OnCanceledEvent?.Invoke();
                }
            }

            choiceCoroutine = null;
        } 

        public virtual void OnChoiced(int index) { }
        public virtual void OnSelectionChanged() { }
        public virtual void OnCanceled() { }
    }
}
