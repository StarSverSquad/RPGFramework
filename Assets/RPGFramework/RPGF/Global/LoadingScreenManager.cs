using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGF
{
    public class LoadingScreenManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject backgroundContainer;
        [SerializeField]
        private GameObject proggresBarContainer;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private TextMeshProUGUI progressBarText;

        public float BackgroundFadeTime = 0.5f;

        private float progress = 0;
        public float Progress
        {
            get => progress;
            set
            {
                progress = value;
                SetProgressBarValue(value);
            }
        }

        public bool IsBackgroundFading => backgroundTween != null;

        private Tween backgroundTween = null;

        private void OnEnable()
        {
            backgroundContainer.SetActive(false);
            proggresBarContainer.SetActive(false);
        }

        public void ShowBackground()
        {
            backgroundContainer.SetActive(true);

            backgroundTween = backgroundImage.DOFade(1, BackgroundFadeTime)
                                             .From(0)
                                             .SetLoops(0)
                                             .OnStepComplete(CleanBackgroudTween)
                                             .Play();
        }

        public void HideBackground()
        {
            backgroundTween = backgroundImage.DOFade(0, BackgroundFadeTime)
                                 .From(1)
                                 .SetLoops(0)
                                 .OnStepComplete(CleanBackgroudTween)
                                 .Play();
        }

        public void ShowProggresBar()
        {
            proggresBarContainer.SetActive(true);
        }

        public void HideProggresBar()
        {
            proggresBarContainer.SetActive(false);
        }

        private void SetProgressBarValue(float value)
        {
            if (proggresBarContainer.activeSelf)
                progressBarText.text = $"Loading: {Mathf.Floor(value * 100f)}%";
        }

        private void CleanBackgroudTween()
        {
            backgroundTween = null;
        }
    }
}