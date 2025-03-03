using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer masterMixer; 
    public void SetMusic(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);
    }

    public void SetEffect(float soundLevel)
    {
        masterMixer.SetFloat("effectVol", soundLevel);
    }

}
