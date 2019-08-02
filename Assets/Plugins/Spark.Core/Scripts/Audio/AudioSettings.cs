using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {

    [SerializeField]
    private AudioMixerGroup master, music, sfx;

    [SerializeField]
    private Slider masterSlider, musicSlider, sfxSlider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMixerVolume(string category)
    {
        if (category == "master") master.audioMixer.SetFloat("masterVolume", LinearToDecibel(masterSlider.value));
        else if (category == "music") music.audioMixer.SetFloat("musicVolume", LinearToDecibel(musicSlider.value));
        else if (category == "sfx") sfx.audioMixer.SetFloat("sfxVolume", LinearToDecibel(sfxSlider.value));
    }

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

}
