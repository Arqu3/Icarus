using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spark.UI;
using TMPro;
using UnityEngine.Events;

[AddComponentMenu("")]
public class SettingsCanvas : InstantiatableCanvas
{
	public Button closeButton, applyButton;
	[Header("Tabs")]
	public Button videoTabButton;
	public Button audioTabButton, gameplayTabButton;
	public RectTransform videoTab, audioTab, gameplayTab;
	[Header("Video")]
	public Slider fovSlider;
	public TMP_Dropdown resolutionDropdown;
	public TMP_Dropdown fullScreenModeDropdown;
	public TMP_Dropdown graphicsQualityDropdown;
	public Toggle vSyncToggle;
	public Slider targetFramerateSlider;
	public Toggle postProcessingEffectsToggle;
	[Header("Audio")]
	public Slider masterVolumeSlider;
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;
	[Header("Gameplay")]
	public Slider mouseSensitivitySlider;
	public TMP_Dropdown languageDropdown;
	public Button configureControlsButton;

}