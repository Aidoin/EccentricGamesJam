using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    public void SetVolumeMusic(float volume) {
        masterMixer.SetFloat("MusicVolume", Sound.VolumeToDb(volume));
    }
    public void SetVolumeSounds(float volume) {
        masterMixer.SetFloat("SoundsVolume", Sound.VolumeToDb(volume));
    }
}
