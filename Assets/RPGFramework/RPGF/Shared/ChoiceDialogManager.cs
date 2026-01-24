using DG.Tweening;
using RPGF.Core.Choice;
using RPGF.Domain.DI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPGF.Shared
{
    public class ChoiceDialogManager : ChoiceBase<ChoiceItem>
    {
        [Inject]
        private readonly BaseOptions _options;
        [Inject]
        private readonly AudioManager _audio;

        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private RectTransform arrow;

        [SerializeField]
        private AudioClip selectionChangeSound;
        [SerializeField]
        private AudioClip confirmSound;
        [SerializeField]
        private AudioClip cancelSound;

        public bool CancelBlocked { get; private set; }

        private Vector2 arrowInitialPisition;

        private Tween arrowChangeSelectionAnimation;

        private GameObject[] itemObjects;

        public override void Initialize()
        {
            base.Initialize();

            content.gameObject.SetActive(false);
        }

        public void SetCancelBlock(bool blocked)
        {
            CancelBlocked = blocked;
        }

        protected override void OnStarted()
        {
            content.gameObject.SetActive(true);

            var objectList = new List<GameObject>();

            float yOffset = 0;
            foreach (var item in Items)
            {
                var newObject = Instantiate(itemPrefab, content);
                var newObjectTransform = newObject.GetComponent<RectTransform>();
                var newObjectText = newObject.GetComponentInChildren<TextMeshProUGUI>();

                newObjectTransform.anchoredPosition = new Vector2(0, yOffset);

                newObjectText.text = item.Label;

                yOffset += newObjectTransform.sizeDelta.y;

                objectList.Add(newObject);
            }

            var currentObject = objectList[Index].GetComponent<RectTransform>();

            arrow.anchoredPosition = new Vector2(arrow.anchoredPosition.x, currentObject.anchoredPosition.y - currentObject.sizeDelta.y / 2f);

            content.DOKill();
            content.DOSizeDelta(new Vector2(content.sizeDelta.x, yOffset), .4f)
                   .From(new Vector2(content.sizeDelta.x, 24))
                   .SetEase(Ease.OutCubic)
                   .Play();

            StartArrowAnimation();

            itemObjects = objectList.ToArray();
        }

        protected override void OnSelectionChanged(ChoiceItem item, int index, int prevIndex)
        {
            var prevItemObject = itemObjects[prevIndex];
            var currentItemObject = itemObjects[index];

            prevItemObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            currentItemObject.GetComponentInChildren<TextMeshProUGUI>().color = new Color(235, 174, 52);

            var currentItemRect = currentItemObject.GetComponent<RectTransform>();

            arrowChangeSelectionAnimation?.Kill();
            arrowChangeSelectionAnimation = arrow
                .DOAnchorPosY(currentItemRect.anchoredPosition.y + currentItemRect.sizeDelta.y / 2f, 0.15f)
                .SetEase(Ease.OutCubic)
                .Play();

            _audio.PlaySE(selectionChangeSound);
        }   

        protected override void OnEnded()
        {
            StopArrowAnimation();

            foreach (var obj in itemObjects)
                Destroy(obj);

            itemObjects = null;

            content.DOKill();
            content.DOSizeDelta(new Vector2(content.sizeDelta.x, 0), .4f)
                   .SetEase(Ease.OutCubic)
                   .Play();

            content.gameObject.SetActive(false);
        }

        protected override void OnConfirmed(ChoiceItem resultItem)
        {
            _audio.PlaySE(confirmSound);
        }

        protected override void OnCanceled()
        {
            _audio.PlaySE(cancelSound);
        }

        protected override bool CancelCanExecute()
        {
            return Input.GetKeyDown(_options.Cancel) && !CancelBlocked;
        }

        protected override bool ConfirmCanExecute()
        {
            return Input.GetKeyDown(_options.Accept);
        }

        protected override int SelectionChange(int currentIndex)
        {
            if (Input.GetKeyDown(_options.MoveDown))
            {
                return currentIndex - 1; 
            }
            else if (Input.GetKeyDown(_options.MoveUp))
            {
                return currentIndex + 1;
            }

            return currentIndex;
        }

        private void StartArrowAnimation()
        {
            arrowInitialPisition = arrow.anchoredPosition;

            arrow.DOLocalRotateQuaternion(Quaternion.Euler(360, 0, 0), 4)
                 .SetLoops(-1)
                 .Play();

            var sequence = DOTween.Sequence();

            sequence.Append(arrow.DOAnchorPosX(-16, .25f).From(arrowInitialPisition).SetRelative(true));
            sequence.Append(arrow.DOAnchorPosX(16, .25f).SetRelative(true));
            sequence.SetLoops(-1);
            sequence.Play();
        }
        private void StopArrowAnimation()
        {
            arrow.DOKill();

            arrow.anchoredPosition = arrowInitialPisition;
        }
    }
}