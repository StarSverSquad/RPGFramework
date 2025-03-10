using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPGF.Choice
{
    public class ChoiceUI : RPGFrameworkBehaviour, IDisposable
    {
        #region Element info
        [Serializable]
        public class Element
        {
            public object Value;

            public Sprite Icon;

            public string Name;
            public string Description;

            public ChoiceElement UIElement;

            public string CounterText;

            public bool locked;
        }
        #endregion

        #region Margin
        [Serializable]
        public struct Margin
        {
            public float top;
            public float bottom;
            public float left;
            public float right;
        }
        #endregion

        [Header("Ссылки")]
        [SerializeField]
        [Tooltip("Объект в котором содаёться содержимое")]
        protected RectTransform content;
        [SerializeField]
        [Tooltip("Объект относительно которого идёт позиционирование")]
        protected RectTransform rect;

        [Header("Префабы")]
        [SerializeField]
        protected ChoiceElement ElementPrefab;
        [SerializeField]
        protected TextMeshProUGUI TitlePrefab;

        [Header("Настройки")]
        public Margin _Margin;

        [SerializeField]
        private int columns;
        public int Columns => columns;

        [SerializeField]
        private Vector2 gap;
        public Vector2 Gap => gap;

        protected List<List<Element>> ElementLists = new();
        protected List<GameObject> AllGameObjects = new();

        private Vector2 cursor = Vector2.zero;

        private Coroutine choiceCorotine = null;

        public Element CurrentUIElement => ElementLists[listIndex.x][listIndex.y];
        public float ElementSizeX => (rect.sizeDelta.x - _Margin.left - _Margin.right - (gap.x * (columns - 1))) / columns;

        private Vector2Int listIndex = Vector2Int.zero;

        public bool IsChoicing => choiceCorotine != null;
        public bool IsChoiced { get; private set; } = false;
        public bool IsCanceled { get; private set; } = false;

        #region Events
        public event Action OnStart;
        public event Action OnEnd;
        public event Action OnSellectionChanged;
        public event Action OnCanceled;
        public event Action OnDeny;
        public event Action OnSuccess;
        #endregion

        private void Start()
        {
            Dispose();
        }

        public void AppendElements(params Element[] elements)
        {
            ElementLists.Add(new());

            for (int i = 0, j = 0; i < elements.Length; i++)
            {
                Element current = elements[i];

                GameObject obj = Instantiate(ElementPrefab.gameObject, content);

                var elRect = obj.GetComponent<RectTransform>();
                var objElement = obj.GetComponent<ChoiceElement>();

                objElement.Setup(current.Name, current.Icon, current.CounterText);

                objElement.SetLock(current.locked);

                elRect.anchoredPosition = Vector2.zero;
                elRect.anchoredPosition += cursor;
                elRect.sizeDelta = new Vector2(ElementSizeX, elRect.sizeDelta.y);

                current.UIElement = objElement;

                AllGameObjects.Add(obj);
                ElementLists.Last().Add(current);

                j++;

                cursor += new Vector2(ElementSizeX + (j < columns ? gap.x : 0), 0);

                if (j == columns)
                {
                    cursor = new Vector2(_Margin.left, cursor.y - elRect.sizeDelta.y - gap.y);

                    j = 0;
                }
                else if (i == elements.Length - 1)
                    cursor = new Vector2(_Margin.left, cursor.y - elRect.sizeDelta.y);
            }

            content.sizeDelta = new Vector2(rect.sizeDelta.x, -cursor.y);
        }

        public void AppendTitle(string text, TextAlignmentOptions aling)
        {
            GameObject obj = Instantiate(TitlePrefab.gameObject, content);
            TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();
            RectTransform rect = obj.GetComponent<RectTransform>();

            obj.transform.position = transform.position;

            rect.anchoredPosition += cursor;
            rect.sizeDelta = new Vector2(this.rect.sizeDelta.x - _Margin.left - _Margin.right, rect.sizeDelta.y);

            textMesh.text = text;

            textMesh.alignment = aling;

            AllGameObjects.Add(obj);

            cursor = new Vector2(_Margin.left, cursor.y - rect.sizeDelta.y);

            content.sizeDelta = new Vector2(this.rect.sizeDelta.x, -cursor.y);
        }

        public virtual void InvokeChoice()
        {
            if (IsChoicing)
            {
                StopCoroutine(choiceCorotine);
                choiceCorotine = null;
            }

            choiceCorotine = StartCoroutine(ChoiceCoroutine());

        }

        protected virtual void CheckIndex()
        {
            if (listIndex.y > ElementLists[listIndex.x].Count - 1)
            {
                if (listIndex.x < ElementLists.Count - 1)
                {
                    listIndex.x++;
                    listIndex.y = 0;
                }
                else
                    listIndex.y = ElementLists[listIndex.x].Count - 1;
            }
            else if (listIndex.y < 0)
            {
                if (listIndex.x > 0)
                {
                    listIndex.x--;
                    listIndex.y = ElementLists[listIndex.x].Count - 1;
                }
                else
                    listIndex.y = 0;
            }
        }

        private IEnumerator ChoiceCoroutine()
        {
            IsChoiced = false; IsCanceled = false;

            listIndex = Vector2Int.zero;

            yield return null;

            if (ElementLists.Count == 0)
                IsCanceled = true;
            else
            {
                OnStart?.Invoke();

                CurrentUIElement.UIElement.SetFocus(true);
            }

            while (!IsChoiced && !IsCanceled)
            {
                yield return null;

                if (Input.GetKeyDown(Game.BaseOptions.MoveUp))
                {
                    CurrentUIElement.UIElement.SetFocus(false);

                    listIndex.y -= columns;

                    CheckIndex();

                    CurrentUIElement.UIElement.SetFocus(true);

                    OnSellectionChanged?.Invoke();
                }
                else if (Input.GetKeyDown(Game.BaseOptions.MoveDown))
                {
                    CurrentUIElement.UIElement.SetFocus(false);

                    listIndex.y += columns;

                    CheckIndex();

                    CurrentUIElement.UIElement.SetFocus(true);

                    OnSellectionChanged?.Invoke();
                }
                else if (Input.GetKeyDown(Game.BaseOptions.MoveRight))
                {
                    CurrentUIElement.UIElement.SetFocus(false);

                    listIndex.y++;

                    CheckIndex();

                    CurrentUIElement.UIElement.SetFocus(true);

                    OnSellectionChanged?.Invoke();
                }
                else if (Input.GetKeyDown(Game.BaseOptions.MoveLeft))
                {
                    CurrentUIElement.UIElement.SetFocus(false);

                    listIndex.y--;

                    CheckIndex();

                    CurrentUIElement.UIElement.SetFocus(true);

                    OnSellectionChanged?.Invoke();
                }
                else if (Input.GetKeyDown(Game.BaseOptions.Accept))
                {
                    if (!CurrentUIElement.locked)
                    {
                        IsChoiced = true;
                        OnSuccess?.Invoke();
                    }
                    else
                        OnDeny?.Invoke();
                }
                else if (Input.GetKeyDown(Game.BaseOptions.Cancel))
                {
                    IsCanceled = true;
                    OnCanceled?.Invoke();
                }
            }

            choiceCorotine = null;

            OnEnd?.Invoke();
        }

        public virtual void Dispose()
        {
            foreach (var obj in AllGameObjects)
                Destroy(obj);
            AllGameObjects.Clear();

            ElementLists.Clear();

            cursor = new Vector2(_Margin.left, -_Margin.top);

            content.sizeDelta = new Vector2(rect.sizeDelta.x, _Margin.top + _Margin.bottom);
            content.position = rect.position;

        }
    }
}