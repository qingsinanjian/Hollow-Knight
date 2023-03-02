using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Text[] volText;

    public void SetMasterVolume(float volume)
    {
        volText[0].text = volume.ToString();
        audioMixer.SetFloat("MasterVolume", Remap01ToDB(volume));
    }

    public void SetSoundVolume(float volume)
    {
        volText[1].text = volume.ToString();
        audioMixer.SetFloat("SoundVolume", Remap01ToDB(volume));
    }

    public void SetMusicVolume(float volume)
    {
        volText[2].text = volume.ToString();
        audioMixer.SetFloat("MusicVolume", Remap01ToDB(volume));
    }

    private float Remap01ToDB(float x)
    {
        if (x <= 0.0f) x = 0.0001f;
        else x /= 10;
        return Mathf.Log10(x) * 20.0f;
    }
}
