using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGF.GUI
{
    public class GUIChoicableBlock : GUIBlockBase
    {
        [SerializeField]
        private List<GUIElementBase> _elements = new();
        [SerializeField]
        private bool _isHorizontal = true;

        private int index = 0;
        private Coroutine choiceCoroutine = null;

        protected GUIElementBase CurrentElement => _elements[index];

        public UnityEvent OnSelectionChangedEvent;
        public UnityEvent OnCanceledEvent;
        public UnityEvent<int> OnChoicedEvent;

        public override void Initialize(GUIManagerBase manager)
        {
            base.Initialize(manager);
        }

        protected override void OnActivate()
        {
            StartChoice();
        }

        protected override void OnDispose()
        {
            index = 0;
        }

        protected override void OnDiativate()
        {
            foreach (var item in _elements)
            {
                item.SetFocus(false);
            }

            DisposeChoice();
        }

        protected void StartChoice()
        {
            _elements[index].SetFocus(true);

            DisposeChoice();
            choiceCoroutine = StartCoroutine(ChoiceCoroutine());
        }
        protected void DisposeChoice()
        {
            if (choiceCoroutine != null)
            {
                StopCoroutine(choiceCoroutine);
                choiceCoroutine = null;
            }
        }

        protected void ChangeSelect(int newIndex)
        {
            CurrentElement.SetFocus(false);

            index = Mathf.Clamp(newIndex, 0, _elements.Count - 1);

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

                if (_isHorizontal)
                {
                    if (Input.GetKeyDown(Game.BaseOptions.MoveLeft))
                    {
                        ChangeSelect(index - 1);
                    }
                    else if (Input.GetKeyDown(Game.BaseOptions.MoveRight))
                    {
                        ChangeSelect(index + 1);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(Game.BaseOptions.MoveDown))
                    {
                        ChangeSelect(index + 1);

                    }
                    else if (Input.GetKeyDown(Game.BaseOptions.MoveUp))
                    {
                        ChangeSelect(index - 1);
                    }
                }

                if (Input.GetKeyDown(Game.BaseOptions.Accept))
                {
                    end = true;

                    CurrentElement.Select();

                    OnChoiced(index);
                    OnChoicedEvent?.Invoke(index);
                }

                if (Input.GetKeyDown(Game.BaseOptions.Cancel))
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
