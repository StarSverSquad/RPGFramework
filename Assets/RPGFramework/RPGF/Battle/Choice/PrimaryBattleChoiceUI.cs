using RPGF.Core;
using RPGF.Domain.DI;
using RPGF.Domain.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace RPGF.Battle.Choice
{
    public class PrimaryBattleChoiceUI : RPGFrameworkBehaviour, IActive
    {
        [Inject]
        private readonly BaseOptions _options = null!;

        [SerializeField]
        private BattleChoiceItemUI[] buttons = new BattleChoiceItemUI[4];

        [SerializeField]
        private GameObject buttonsContainer;

        [SerializeField]
        private int choice;
        public int Choice => choice;

        [SerializeField]
        private bool isCanceled = false;
        public bool IsCanceled => isCanceled;

        public bool IsChoicing => choiceCoroutine != null;

        private Coroutine choiceCoroutine;

        public event Action OnStart;
        public event Action OnEnd;
        public event Action OnCanceled;
        public event Action OnSuccess;
        public event Action OnSellectionChanged;

        public override void Initialize()
        {
            base.Initialize();

            foreach (var button in buttons)
            {
                button.Initialize();
            }
        }

        public void SetActive(bool active)
        {
            buttonsContainer.SetActive(active);
        }

        public void InvokeChoice(int startChoice = 0)
        {
            choice = startChoice;
            isCanceled = false;

            for (int i = 0; i <= 3; i++)
            {

                if (choice == i)
                {
                    buttons[i].Focus();
                }
                else
                {
                    buttons[i].UnFocus();
                }
            }

            choiceCoroutine = StartCoroutine(ChoiceCoroutine());
        }

        private IEnumerator ChoiceCoroutine()
        {
            OnStart?.Invoke();

            int newchoice = choice;

            while (true)
            {
                yield return null;

                if (Input.GetKeyDown(_options.MoveUp))
                    newchoice = Mathf.Clamp(choice - 1, 0, 3);
                else if (Input.GetKeyDown(_options.MoveDown))
                    newchoice = Mathf.Clamp(choice + 1, 0, 3);

                if (newchoice != choice)
                {
                    buttons[choice].UnFocus();
                    buttons[newchoice].Focus();

                    choice = newchoice;

                    OnSellectionChanged?.Invoke();
                }

                if (Input.GetKeyDown(_options.Accept))
                {
                    OnSuccess?.Invoke();
                    break;
                }
                else if (Input.GetKeyDown(_options.Cancel))
                {
                    choiceCoroutine = null;
                    isCanceled = true;
                    break;
                }
            }

            if (!isCanceled)
                OnEnd?.Invoke();
            else
                OnCanceled?.Invoke();

            choiceCoroutine = null;
        }
    }
}
