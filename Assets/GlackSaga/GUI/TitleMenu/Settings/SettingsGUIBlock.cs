using System;
using System.Linq;
using RPGF;
using RPGF.Core.Extensions;
using RPGF.Core.SaveLoad;
using RPGF.Domain.DI;
using RPGF.GUI;
using RPGF.GUI.Elements;
using RPGF.GUI.Interfaces;
using UnityEngine;

namespace GlackSaga.GUI.TitleMenu.Settings
{
    public class SettingsGUIBlock : GUISelectableBlock
    {
        [Inject]
        private readonly AudioManager _audio = null!;
        [Inject]
        private readonly GameConfigService _config = null!;

        [SerializeField]
        private OptionSelectGUI resolutionSelect;
        [SerializeField]
        private CheckBoxGUI fullsceenCheckbox;
        [SerializeField]
        private SliderGUI bgmSlider;
        [SerializeField]
        private SliderGUI bgsSlider;
        [SerializeField]
        private SliderGUI seSlider;
        [SerializeField]
        private SliderGUI meSlider;

        public override void Initialize(IGUIManager manager)
        {
            base.Initialize(manager);

            fullsceenCheckbox.SetupCheckBox(Screen.fullScreenMode == FullScreenMode.FullScreenWindow);

            var filteredResolutions = Screen.resolutions
                .Where(resolution => Display.main.IsSupportedResolution(resolution))
                .ToList();

            var resolutionsOptions = filteredResolutions.Select(item => new OptionSelectItem
            {
                Label = $"{item.width}x{item.height} ({Mathf.RoundToInt((float)item.refreshRateRatio.value)})",
                Metadata = item,
            });

            var current = Screen.currentResolution;
            var currentResolutionIndex = filteredResolutions.FindIndex(item =>
                item.width == current.width && item.height == current.height 
                && item.refreshRateRatio.numerator == current.refreshRateRatio.numerator
                && item.refreshRateRatio.denominator == current.refreshRateRatio.denominator);

            if (currentResolutionIndex < 0)
                currentResolutionIndex = 0;

            resolutionSelect.SetupSelect(resolutionsOptions, currentResolutionIndex);

            bgmSlider.SetupSlider(_config.Config.BGMVolume);
            bgsSlider.SetupSlider(_config.Config.BGSVolume);
            seSlider.SetupSlider(_config.Config.SEVolume);
            meSlider.SetupSlider(_config.Config.MEVolume);

            resolutionSelect.OnOptionSelectionChanged.AddListener(OnResolutionOptionChanged);
            resolutionSelect.OnOptionSelectionCanceled.AddListener(OnResolutionOptionChanged);

            fullsceenCheckbox.OnChecked.AddListener(OnFullSceenChecked);

            bgmSlider.OnSliderChanged.AddListener(OnBGMSliderChanged);
            bgmSlider.OnSliderCanceled.AddListener(OnBGMSliderChanged);

            bgsSlider.OnSliderChanged.AddListener(OnBGSSliderChanged);
            bgsSlider.OnSliderCanceled.AddListener(OnBGSSliderChanged);

            seSlider.OnSliderChanged.AddListener(OnSESliderChanged);
            seSlider.OnSliderCanceled.AddListener(OnSESliderChanged);

            meSlider.OnSliderChanged.AddListener(OnMESliderChanged);
            meSlider.OnSliderCanceled.AddListener(OnMESliderChanged);

            resolutionSelect.OnOptionSelected.AddListener(OnOptionSelectSaveConfig);

            bgmSlider.OnSldierConfirmed.AddListener(OnSliderSaveConfig);
            bgsSlider.OnSldierConfirmed.AddListener(OnSliderSaveConfig);
            seSlider.OnSldierConfirmed.AddListener(OnSliderSaveConfig);
            meSlider.OnSldierConfirmed.AddListener(OnSliderSaveConfig);
        }


        private void OnResolutionOptionChanged(OptionSelectItem option)
        {
            var resolution = (Resolution)option.Metadata;
            var fmode = _config.Config.Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

            Screen.SetResolution(resolution.width, resolution.height, fmode, resolution.refreshRateRatio);
        }

        private void OnOptionSelectSaveConfig(OptionSelectItem _)
        {
            SaveConfig();
        }

        private void OnFullSceenChecked(bool isChecked)
        {
            Screen.fullScreenMode = isChecked ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

            SaveConfig();
        }

        private void OnBGMSliderChanged(float value)
        {
            _audio.SetBGMMixerVolumeNormilized(value);
        }

        private void OnBGSSliderChanged(float value)
        {
            _audio.SetBGSMixerVolumeNormilized(value);
        }

        private void OnSESliderChanged(float value)
        {
            _audio.SetSEMixerVolumeNormilized(value);
        }

        private void OnMESliderChanged(float value)
        {
            _audio.SetMEMixerVolumeNormilized(value);
        }

        private void OnSliderSaveConfig(float _)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            var config = _config.Config;
            var resolution = (Resolution)resolutionSelect.CurrentOption.Metadata;

            config.Fullscreen = fullsceenCheckbox.Value;

            config.ResolutionX = resolution.width;
            config.ResolutionY = resolution.height;
            config.RefreshRateDenominator = resolution.refreshRateRatio.denominator;
            config.RefreshRateNumenator = resolution.refreshRateRatio.numerator;

            config.BGMVolume = bgmSlider.Value;
            config.BGSVolume = bgsSlider.Value;
            config.SEVolume = seSlider.Value;
            config.MEVolume = meSlider.Value;

            _config.Save();
        }
    }
}