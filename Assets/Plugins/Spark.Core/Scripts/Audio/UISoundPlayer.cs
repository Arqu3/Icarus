using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Audio;
using System.Linq;
using TMPro;

enum SearchMode
{
    Ignore,
    IncludeOnly
}

//Add this to the UI Canvas

public class UISoundPlayer : MonoBehaviour {

    private Selectable[] elements;

    private readonly Type[] specialTypes = new Type[] { typeof (Slider), typeof (Toggle), typeof (TMP_Dropdown) };

    /// <summary>
    ///Stuff to be done:
    /// Dropdowns should support hover and click sounds
    /// </summary>

    // Use this for initialization
    void Start ()
    {
        elements = GetComponentsInChildren<Selectable> (true);

        foreach (Selectable element in elements)
        {
            AddSoundListener (EventTriggerType.PointerEnter, element, UISoundManager.Instance.TaskOnHover,
                SearchMode.IncludeOnly, typeof (Button), typeof (Slider), typeof (Toggle));

            AddSoundListener(EventTriggerType.PointerDown, element, UISoundManager.Instance.TaskOnClick);
        }
    }

    void AddSoundListener ( EventTriggerType triggerType, Selectable element, 
        UnityAction function, SearchMode mode = SearchMode.IncludeOnly, params Type[] types )
    {
        if (types.Length > 0)
        {
            var ele = ( from type in types
                        where element.GetType ().IsAssignableFrom (type)
                        select type ).ToList ();

            switch ( mode )
            {
                case SearchMode.Ignore:
                    if ( ele.Count > 0 ) return;
                    break;
                case SearchMode.IncludeOnly:
                    if ( ele.Count == 0 ) return;
                    break;
                default:
                    break;
            }
        }

        if ( triggerType == EventTriggerType.PointerDown && specialTypes.Contains (element.GetType ()) )
        {
            SetupSpecialSelectables (element, triggerType, function);
            return;
        }

        SetupEventTrigger (element, triggerType, function);
    }

    void SetupSpecialSelectables(Selectable element, EventTriggerType triggerType, UnityAction function)
    {
        Toggle t = element as Toggle;
        if ( t != null ) t.onValueChanged.AddListener (UISoundManager.Instance.TaskOnToggle);

        Slider s = element as Slider;
        if ( s != null )
        {
            EventTrigger et = SetupEventTrigger (element, triggerType, function);
            AddEventTriggerEvent (et, EventTriggerType.PointerDown, () =>
            {
                UISoundManager.Instance.allowHoverSound = false;
            });
            AddEventTriggerEvent (et, EventTriggerType.PointerUp, () =>
            {
                UISoundManager.Instance.allowHoverSound = true;
                UISoundManager.Instance.VolumeSliderRelease (UISoundManager.SliderReleaseCategory.MasterVolume);
            });
        }

        TMP_Dropdown d = element as TMP_Dropdown;
        if (d != null)
        {
            EventTrigger et = SetupEventTrigger (element, EventTriggerType.PointerClick, function);
            d.onValueChanged.AddListener (( i ) =>
             {
                 UISoundManager.Instance.allowHoverSound = true;
                 UISoundManager.Instance.TaskOnClick ();
             });
            AddEventTriggerEvent (et, EventTriggerType.PointerClick, () =>
             {
                 UISoundManager.Instance.allowHoverSound = false;
                 Debug.Log ("click");
             });
            AddEventTriggerEvent (et, EventTriggerType.Submit, () =>
             {
                 UISoundManager.Instance.allowHoverSound = false;
                 Debug.Log ("submit");
             });
            AddEventTriggerEvent (et, EventTriggerType.Cancel, () =>
             {
                 UISoundManager.Instance.allowHoverSound = true;
                 Debug.Log ("cancel");
             });
        }
    }

    EventTrigger SetupEventTrigger(Selectable element, EventTriggerType triggerType, UnityAction function)
    {
        EventTrigger et = element.GetComponent<EventTrigger> ();
        if ( !et ) et = element.gameObject.AddComponent<EventTrigger> ();

        AddEventTriggerEvent (et, triggerType, function);

        return et;
    }

    void AddEventTriggerEvent(EventTrigger et, EventTriggerType triggerType, UnityAction function)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry ();
        entry.eventID = triggerType;

        entry.callback.AddListener (( data ) =>
        {
            function.Invoke ();
        });
        et.triggers.Add (entry);
    }
}

public class UISoundManager : MonoSingleton<UISoundManager>
{
    public bool allowHoverSound = true;

    private AudioSource source, musicSource, masterSource;

    private void Start ()
    {
        source = gameObject.AddComponent<AudioSource> ();
        musicSource = gameObject.AddComponent<AudioSource> ();
        masterSource = gameObject.AddComponent<AudioSource> ();

        source.outputAudioMixerGroup = Resources.Load<AudioMixer> ("Audio/Master").FindMatchingGroups ("Interface")[0];
        musicSource.outputAudioMixerGroup = Resources.Load<AudioMixer> ("Audio/Master").FindMatchingGroups ("User Music")[0];
        masterSource.outputAudioMixerGroup = Resources.Load<AudioMixer> ("Audio/Master").FindMatchingGroups ("Master")[0];

        source.bypassListenerEffects = true;
        musicSource.bypassListenerEffects = true;
        masterSource.bypassListenerEffects = true;

        source.bypassReverbZones = true;
        musicSource.bypassReverbZones = true;
        masterSource.bypassReverbZones = true;
    }

    public void TaskOnClick ()
    {
        source.PlayOneShotSafeRandom (Resources.LoadAll<AudioClip> ("Audio/UI/Click"));
    }

    public void TaskOnHover ()
    {
        if ( !allowHoverSound ) return;

        source.PlayOneShotSafeRandom (Resources.LoadAll<AudioClip> ("Audio/UI/Hover Select"));
    }

    public void TaskOnToggle ( bool isToggled )
    {
        string path = "Audio/UI/Toggle/";
        path += isToggled ? "True" : "False";

        source.PlayOneShotSafeRandom (Resources.LoadAll<AudioClip> (path));
    }

    public enum SliderReleaseCategory
    {
        MasterVolume,
        MusicVolume,
        SfxVolume
    }

    public void VolumeSliderRelease ( SliderReleaseCategory category )
    {
        switch ( category )
        {
            case SliderReleaseCategory.MasterVolume:
                masterSource.PlayOneShotSafeRandom (Resources.LoadAll<AudioClip> ("Audio/UI/Mixer Release/Master"));
                break;
            case SliderReleaseCategory.MusicVolume:
                musicSource.PlayOneShotSafeRandom (Resources.LoadAll<AudioClip> ("Audio/UI/Mixer Release/Music"));
                break;
            case SliderReleaseCategory.SfxVolume:
                source.PlayOneShotSafeRandom (Resources.LoadAll<AudioClip> ("Audio/UI/Mixer Release/SFX"));
                break;
            default:
                break;
        }
    }
}
