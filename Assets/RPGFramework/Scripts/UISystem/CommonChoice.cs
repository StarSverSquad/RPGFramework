using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CommonChoiceUI : MonoBehaviour
{
    public enum Aling
    {
        left, center, right
    }
    public struct ElementInfo
    {
        public object value;

        public Sprite icon;

        public string name;
        public string description;

        public CommonChoiceUIElement element;

        public string counterText;

        public bool locked;
    }

    [Header("Ссылки")]
    [SerializeField]
    protected RectTransform content;
    [SerializeField]
    protected RectTransform rect;

    [Header("Префабы")]
    [SerializeField]
    protected CommonChoiceUIElement elementPrefab;
    [SerializeField]
    protected TextMeshProUGUI titlePrefab;

    [Header("Настройки")]
    [SerializeField]
    private Vector4 margin;
    public Vector4 Margin => margin;

    [SerializeField]
    private int columns;
    public int Columns => columns;
    [SerializeField]
    private Vector2 offset;
    public Vector2 Offset => offset;

    #region Not unity serialize

    protected List<List<ElementInfo>> elementLists = new List<List<ElementInfo>>();
    protected List<GameObject> objBuffer = new List<GameObject>();

    private Vector2 cursor = Vector2.zero;

    private Coroutine choiceCorotine = null;

    public bool IsChoicing => choiceCorotine != null;

    public bool IsChoiced { get; private set; } = false;
    public bool IsCanceled { get; private set; } = false;

    public ElementInfo CurrentItem => elementLists[listIndex.x][listIndex.y];

    public float ElementSizeX => (rect.sizeDelta.x - margin.x - margin.z - (offset.x * (columns - 1))) / columns;

    private Vector2Int listIndex = Vector2Int.zero;

    public event Action OnStart;
    public event Action OnEnd;
    public event Action OnSellectionChanged;
    public event Action OnCanceled;
    public event Action OnDeny;
    public event Action OnSuccess;

    #endregion

    private void Start()
    {
        CleanUp();
    }

    /// <summary>
    /// Добовление элементов в список выбора
    /// </summary>
    /// <param name="elements">Массив элементов</param>
    public void AppendElements(params ElementInfo[] elements)
    {
        elementLists.Add(new List<ElementInfo>());

        for (int i = 0, j = 0; i < elements.Length; i++)
        {
            ElementInfo cur = elements[i];

            GameObject obj = Instantiate(elementPrefab.gameObject, content);

            RectTransform elRect = obj.GetComponent<RectTransform>();
            CommonChoiceUIElement objElement = obj.GetComponent<CommonChoiceUIElement>();

            objElement.Setup(cur.name, cur.icon, cur.counterText);

            objElement.SetLock(cur.locked);

            elRect.anchoredPosition = Vector2.zero;
            elRect.anchoredPosition += cursor;
            elRect.sizeDelta = new Vector2(ElementSizeX, elRect.sizeDelta.y);

            cur.element = objElement;

            objBuffer.Add(obj);
            elementLists.Last().Add(cur);

            j++;

            cursor += new Vector2(ElementSizeX + (j < columns ? offset.x : 0), 0);

            if (j == columns)
            {
                cursor = new Vector2(margin.x, cursor.y - elRect.sizeDelta.y - offset.y);

                j = 0;
            }
            else if (i == elements.Length - 1)
                cursor = new Vector2(margin.x, cursor.y - elRect.sizeDelta.y);
        }

        content.sizeDelta = new Vector2(this.rect.sizeDelta.x, -cursor.y);
    }

    /// <summary>
    /// Добовление заголовка
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="aling">Расположение</param>
    public void AppendTittle(string text, Aling aling)
    {
        GameObject obj = Instantiate(titlePrefab.gameObject, content);
        TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();
        RectTransform rect = obj.GetComponent<RectTransform>();

        obj.transform.position = transform.position;

        rect.anchoredPosition += cursor;
        rect.sizeDelta = new Vector2(this.rect.sizeDelta.x - margin.x - margin.z, rect.sizeDelta.y);

        textMesh.text = text;

        switch (aling)
        {
            case Aling.left:
                textMesh.alignment = TextAlignmentOptions.Left;
                break;
            case Aling.center:
                textMesh.alignment = TextAlignmentOptions.Center;
                break;
            case Aling.right:
                textMesh.alignment = TextAlignmentOptions.Right;
                break;
        }

        objBuffer.Add(obj);

        cursor = new Vector2(margin.x, cursor.y - rect.sizeDelta.y);

        content.sizeDelta = new Vector2(this.rect.sizeDelta.x, -cursor.y);
    }

    /// <summary>
    /// Подчиска класса
    /// </summary>
    public virtual void CleanUp()
    {
        foreach (var obj in objBuffer)
            Destroy(obj);
        objBuffer.Clear();

        elementLists.Clear();

        cursor = new Vector2(margin.x, -margin.y);

        content.sizeDelta = new Vector2(rect.sizeDelta.x, margin.y + margin.w);
        content.position = rect.position;
    }

    /// <summary>
    /// Запуск выбора
    /// </summary>
    public virtual void InvokeChoice()
    {
        if (IsChoicing)
        {
            StopCoroutine(choiceCorotine);
            choiceCorotine = null;
        }

        choiceCorotine = StartCoroutine(ChoiceCoroutine());

    }

    /// <summary>
    /// Проверка индекса
    /// </summary>
    protected virtual void CheckIndex()
    {
        if (listIndex.y > elementLists[listIndex.x].Count - 1)
        {
            if (listIndex.x < elementLists.Count - 1)
            {
                listIndex.x++;
                listIndex.y = 0;
            }
            else
                listIndex.y = elementLists[listIndex.x].Count - 1;
        }
        else if (listIndex.y < 0)
        {
            if (listIndex.x > 0)
            {
                listIndex.x--;
                listIndex.y = elementLists[listIndex.x].Count - 1;
            }
            else
                listIndex.y = 0;
        }
    }

    /// <summary>
    /// Курутина выбора
    /// </summary>
    private IEnumerator ChoiceCoroutine()
    {
        IsChoiced = false; IsCanceled = false;

        listIndex = Vector2Int.zero;

        yield return null;

        if (elementLists.Count == 0)
            IsCanceled = true;
        else
        {
            OnStart?.Invoke();

            CurrentItem.element.SetFocus(true);
        }

        while (!IsChoiced && !IsCanceled)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CurrentItem.element.SetFocus(false);

                listIndex.y -= columns;

                CheckIndex();

                CurrentItem.element.SetFocus(true);

                OnSellectionChanged?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                CurrentItem.element.SetFocus(false);

                listIndex.y += columns;

                CheckIndex();

                CurrentItem.element.SetFocus(true);

                OnSellectionChanged?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                CurrentItem.element.SetFocus(false);

                listIndex.y++;

                CheckIndex();

                CurrentItem.element.SetFocus(true);

                OnSellectionChanged?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CurrentItem.element.SetFocus(false);

                listIndex.y--;

                CheckIndex();

                CurrentItem.element.SetFocus(true);

                OnSellectionChanged?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!CurrentItem.locked)
                {
                    IsChoiced = true;
                    OnSuccess?.Invoke();
                }
                else
                    OnDeny?.Invoke();
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                IsCanceled = true;
                OnCanceled?.Invoke();
            }
        }

        choiceCorotine = null;

        OnEnd?.Invoke();
    }
}