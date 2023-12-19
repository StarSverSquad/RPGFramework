using System;
using System.Collections;
using UnityEngine;

public class PrimaryBattleChoiceUI : MonoBehaviour, IActive
{
    [SerializeField]
    private BattleButton[] buttons = new BattleButton[4];

    [SerializeField]
    private GameObject buttonsContainer;
    [SerializeField]
    private GameObject box;

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

    public void SetActive(bool active)
    {
        buttonsContainer.SetActive(active);
        box.SetActive(active);
    }

    public void InvokeChoice(int startChoice = 0)
    {
        choice = startChoice;
        isCanceled = false;

        for (int i = 0; i <= 3; i++)
        {
            if (choice == i)
            {
                if (!buttons[i].IsShow)
                    buttons[i].Show();
            }
            else
            {
                if (buttons[i].IsShow)
                    buttons[i].Hide();
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

            if (Input.GetKeyDown(KeyCode.UpArrow))
                newchoice = Mathf.Clamp(choice - 1, 0, 3);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                newchoice = Mathf.Clamp(choice + 1, 0, 3);

            if (newchoice != choice)
            {
                buttons[choice].Hide();
                buttons[newchoice].Show();

                choice = newchoice;

                OnSellectionChanged?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                OnSuccess?.Invoke();
                break;
            }
            else if (Input.GetKeyDown(KeyCode.X))
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