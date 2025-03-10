using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPGF.Choice
{
    public class ChoiceUI : MonoBehaviour, IDisposable
    {
        #region Element info
        [Serializable]
        public struct ElementInfo
        {
            public object value;

            public Sprite icon;

            public string name;
            public string description;

            public ChoiceElement element;

            public string counterText;

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
        protected RectTransform content;
        [SerializeField]
        protected RectTransform rect;

        [Header("Префабы")]
        [SerializeField]
        protected ChoiceElement elementPrefab;
        [SerializeField]
        protected TextMeshProUGUI titlePrefab;

        [Header("Настройки")]
        public Margin margin;

        [SerializeField]
        private int columns;
        public int Columns => columns;

        [SerializeField]
        private Vector2 gap;
        public Vector2 Gap => gap;

        #region Not unity serialize

        protected List<List<ElementInfo>> elementLists = new List<List<ElementInfo>>();
        protected List<GameObject> objBuffer = new List<GameObject>();

        private Vector2 cursor = Vector2.zero;

        private Coroutine choiceCorotine = null;

        public bool IsChoicing => choiceCorotine != null;
        public bool IsChoiced { get; private set; } = false;
        public bool IsCanceled { get; private set; } = false;

        [Obsolete]
        public ElementInfo CurrentItem => elementLists[listIndex.x][listIndex.y];

        public float ElementSizeX => (rect.sizeDelta.x - margin.left - margin.right - (gap.x * (columns - 1))) / columns;

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
            Dispose();
        }

        public void AppendElements(params ElementInfo[] elements)
        {
            elementLists.Add(new List<ElementInfo>());

            for (int i = 0, j = 0; i < elements.Length; i++)
            {
                ElementInfo cur = elements[i];

                GameObject obj = Instantiate(elementPrefab.gameObject, content);

                RectTransform elRect = obj.GetComponent<RectTransform>();
                ChoiceElement objElement = obj.GetComponent<ChoiceElement>();

                objElement.Setup(cur.name, cur.icon, cur.counterText);

                objElement.SetLock(cur.locked);

                elRect.anchoredPosition = Vector2.zero;
                elRect.anchoredPosition += cursor;
                elRect.sizeDelta = new Vector2(ElementSizeX, elRect.sizeDelta.y);

                cur.element = objElement;

                objBuffer.Add(obj);
                elementLists.Last().Add(cur);

                j++;

                cursor += new Vector2(ElementSizeX + (j < columns ? gap.x : 0), 0);

                if (j == columns)
                {
                    cursor = new Vector2(margin.left, cursor.y - elRect.sizeDelta.y - gap.y);

                    j = 0;
                }
                else if (i == elements.Length - 1)
                    cursor = new Vector2(margin.left, cursor.y - elRect.sizeDelta.y);
            }

            content.sizeDelta = new Vector2(this.rect.sizeDelta.x, -cursor.y);
        }

        public void AppendElements(ElementInfo element)
        {

        }

        public void AppendTitle(string text, TextAlignmentOptions aling)
        {
            GameObject obj = Instantiate(titlePrefab.gameObject, content);
            TextMeshProUGUI textMesh = obj.GetComponent<TextMeshProUGUI>();
            RectTransform rect = obj.GetComponent<RectTransform>();

            obj.transform.position = transform.position;

            rect.anchoredPosition += cursor;
            rect.sizeDelta = new Vector2(this.rect.sizeDelta.x - margin.left - margin.right, rect.sizeDelta.y);

            textMesh.text = text;

            textMesh.alignment = aling;

            objBuffer.Add(obj);

            cursor = new Vector2(margin.left, cursor.y - rect.sizeDelta.y);

            content.sizeDelta = new Vector2(this.rect.sizeDelta.x, -cursor.y);
        }


        /// <remarks>Лучше использовать Dispose</remarks>
        [Obsolete()]
        public virtual void CleanUp()
        {
            foreach (var obj in objBuffer)
                Destroy(obj);
            objBuffer.Clear();

            elementLists.Clear();

            cursor = new Vector2(margin.left, -margin.top);

            content.sizeDelta = new Vector2(rect.sizeDelta.x, margin.top + margin.bottom);
            content.position = rect.position;
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

        public void Dispose()
        {
            foreach (var obj in objBuffer)
                Destroy(obj);
            objBuffer.Clear();

            elementLists.Clear();

            cursor = new Vector2(margin.left, -margin.top);

            content.sizeDelta = new Vector2(rect.sizeDelta.x, margin.top + margin.bottom);
            content.position = rect.position;

        }
    }
}