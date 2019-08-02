using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spark.UI;
using Util;
using UnityEngine.UI;
using System;
using TMPro;

public class SettingsUI : InstantiatableUI<SettingsUI, SettingsCanvas>
{
	public readonly UnityEvent onApply = new UnityEvent();
	public readonly UnityEvent onClose = new UnityEvent();

	private InstantiatableUIBase controlSetupUI;

	private RectTransform openTab;

	private SettingsUI(InstantiatableUIBase controlSetupUI) : base(null)
	{
		this.controlSetupUI = controlSetupUI;

		HookSelectableEvent(Canvas.applyButton, onApply);
		HookSelectableEvent(Canvas.closeButton, onClose);

		HookSelectableEvent(Canvas.audioTabButton, OpenAudio);
		HookSelectableEvent(Canvas.gameplayTabButton, OpenGameplay);
		HookSelectableEvent(Canvas.videoTabButton, OpenVideo);
		if (controlSetupUI != null)
		{
			HookSelectableEvent(Canvas.configureControlsButton, ConfigureControls);
		}
		else
		{
			Canvas.configureControlsButton.gameObject.SetActive(false);
		}

		openTab = Canvas.gameplayTab;
		openTab.gameObject.SetActive(true);
		Canvas.audioTab.gameObject.SetActive(false);
		Canvas.videoTab.gameObject.SetActive(false);
	}

	private void OpenAudio()
	{
		SwitchTab(Canvas.audioTab);
	}
	private void OpenGameplay()
	{
		SwitchTab(Canvas.gameplayTab);
	}
	private void OpenVideo()
	{
		SwitchTab(Canvas.videoTab);
	}
	private void SwitchTab(RectTransform tab)
	{
		if (openTab != tab)
		{
			openTab.gameObject.SetActive(false);
			openTab = tab;
			openTab.gameObject.SetActive(true);
		}
	}

	private void ConfigureControls()
	{
		StartCoroutine(_ConfigureControls());
	}
	private IEnumerator _ConfigureControls()
	{
		controlSetupUI.Show();
		yield return new WaitWhile(() => controlSetupUI.IsShowing);
		controlSetupUI.Hide();
	}

	public override void Dispose()
	{
		base.Dispose();
		controlSetupUI.Dispose();
	}

	public static SettingsTemplateBuilder Build(InstantiatableUIBase controlSetupUI)
	{
		return new SettingsTemplateBuilder(new SettingsUI(controlSetupUI));
	}
	public static SettingsTemplateBuilder Build()
	{
		return new SettingsTemplateBuilder(null);
	}

	public class SettingsTemplateBuilder
	{
		SettingsUI ui;
		public SettingsTemplateBuilder(SettingsUI ui)
		{
			this.ui = ui;
		}

		public void Video_HookFov(ObservableValue<int> value)
		{
			HookSlider(value, ui.Canvas.fovSlider);
		}

		public void Video_BuildResolutionDropdown(string[] values, ObservableValue<int> value)
		{
			BuildDropdown(ui.Canvas.resolutionDropdown, values, value);
		}

		public void Video_BuildFullScreenModeDropdown(string[] values, ObservableValue<int> value)
		{
			BuildDropdown(ui.Canvas.fullScreenModeDropdown, values, value);
		}

		public void Video_BuildGraphicsQualityDropdown(string[] values, ObservableValue<int> value)
		{
			BuildDropdown(ui.Canvas.graphicsQualityDropdown, values, value);
		}

		public void Video_HookVsync(ObservableValue<bool> value)
		{
			HookToggle(value, ui.Canvas.vSyncToggle);
		}

		public void Video_HookFramerate(ObservableValue<int> value)
		{
			HookSlider(value, ui.Canvas.targetFramerateSlider);
		}

		public void Video_HookPostprocessing(ObservableValue<bool> value)
		{
			HookToggle(value, ui.Canvas.postProcessingEffectsToggle);
		}

		public void Audio_HookMasterVolume(ObservableValue<float> value)
		{
			HookSlider(value, ui.Canvas.masterVolumeSlider);
		}

		public void Audio_HookMusicVolume(ObservableValue<float> value)
		{
			HookSlider(value, ui.Canvas.musicVolumeSlider);
		}

		public void Audio_HookSfxVolume(ObservableValue<float> value)
		{
			HookSlider(value, ui.Canvas.sfxVolumeSlider);
		}

		public void Gameplay_HookMouseSens(ObservableValue<float> value)
		{
			HookSlider(value, ui.Canvas.mouseSensitivitySlider);
		}

		public void Gameplay_BuildLanguageDropdown(string[] values, ObservableValue<int> value)
		{
			BuildDropdown(ui.Canvas.languageDropdown, values, value);
		}

		public SettingsUI Finish()
		{
			return ui;
		}

		private void BuildDropdown(TMP_Dropdown dropdown, string[] values, ObservableValue<int> value)
		{
			UnityAction<int> act = f => dropdown.value = f;

			dropdown.ClearOptions();
			dropdown.AddOptions(new List<string>(values));
			dropdown.value = value;

			value.onChange.AddListener(act);
			dropdown.onValueChanged.AddListener(i =>
			{
				value.onChange.RemoveListener(act);
				value.Value = i;
				value.onChange.AddListener(act);
			});

			dropdown.RefreshShownValue();
		}

		private void HookToggle(ObservableValue<bool> value, Toggle toggle)
		{
			UnityAction<bool> act = f => toggle.isOn = f;
			toggle.isOn = value;
			value.onChange.AddListener(act);
			toggle.onValueChanged.AddListener(f =>
			{
				value.onChange.RemoveListener(act);
				value.Value = f;
				value.onChange.AddListener(act);
			});
		}

		private void HookSlider(ObservableValue<float> value, Slider slider)
		{
			UnityAction<float> act = f => slider.value = f;
			slider.value = value;
			value.onChange.AddListener(act);
			slider.onValueChanged.AddListener(f =>
			{
				value.onChange.RemoveListener(act);
				value.Value = f;
				value.onChange.AddListener(act);
			});
		}
		private void HookSlider(ObservableValue<int> value, Slider slider)
		{
			UnityAction<int> act = f => slider.value = f;
			slider.value = value;
			value.onChange.AddListener(act);
			slider.onValueChanged.AddListener(f =>
			{
				value.onChange.RemoveListener(act);
				value.Value = (int)f;
				value.onChange.AddListener(act);
			});
		}
	}
}