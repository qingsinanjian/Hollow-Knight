using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public enum AudioType
    {
        Jump,
        Landing,
        Falling,
        TakeDamage
    }

    [SerializeField] private AudioSource mainAudio;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource landingAudio;
    [SerializeField] private AudioSource fallingAudio;
    [SerializeField] private AudioSource takeDamageAudio;

    public void PlayAudio(AudioType audioType, bool playState)
    {
        AudioSource audioSource = null;
        switch (audioType)
        {
            case AudioType.Jump:
                audioSource = jumpAudio;
                break;
            case AudioType.Landing:
                audioSource = landingAudio;
                break;
            case AudioType.Falling:
                audioSource = fallingAudio;
                break;
            case AudioType.TakeDamage:
                audioSource = takeDamageAudio;
                break;
            default:
                break;
        }
        if(audioSource != null)
        {
            if(playState) audioSource.Play();
            else audioSource.Stop();
        }
    }
}
