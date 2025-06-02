using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer masterMixer;

    public Slider musicVolumMenu;
    public Slider effectVolumMenu;
    public Toggle helpMenu;

    public Slider musicVolumPlay;
    public Slider effectVolumPlay;
    public Toggle helpPlay;

    public void AudioMenu()
    {
        GameManager.instance.Load();

        GameObject canvas = GameObject.Find("Canvas");
        Slider[] allSliders = canvas.GetComponentsInChildren<Slider>(true);

        foreach (Slider s in allSliders)
        {
            if (s.name == "SliderMusic")
            {
                musicVolumMenu = s;
            }

            if (s.name == "SliderEffect")
            {
                effectVolumMenu = s;
            }
        }

        Toggle[] allToggle = canvas.GetComponentsInChildren<Toggle>(true);

        foreach (Toggle g in allToggle)
        {
            if (g.name == "Toggle")
            {
                helpMenu = g;
            }
        }

       

        SetEffect(GameManager.instance.effectVol);
        SetMusic(GameManager.instance.musicVol);

        effectVolumMenu.value = GameManager.instance.effectVol;
        musicVolumMenu.value = GameManager.instance.musicVol;
        helpMenu.isOn = GameManager.instance.helpControls;
    }

    public void AudioPlay()
    {
        GameManager.instance.Load();

        GameObject canvas = GameObject.Find("Canvas");
        Slider[] allSliders = canvas.GetComponentsInChildren<Slider>(true);

        foreach (Slider s in allSliders)
        {
            if (s.name == "SliderMusic")
            {
                musicVolumMenu = s;
            }

            if (s.name == "SliderEffect")
            {
                effectVolumMenu = s;
            }
        }

        Toggle[] allToggle = canvas.GetComponentsInChildren<Toggle>(true);

        foreach (Toggle g in allToggle)
        {
            if (g.name == "Toggle")
            {
                helpMenu = g;
            }
        }



        SetEffect(GameManager.instance.effectVol);
        SetMusic(GameManager.instance.musicVol);

        effectVolumPlay.value = GameManager.instance.effectVol;
        musicVolumPlay.value = GameManager.instance.musicVol;
        helpPlay.isOn = GameManager.instance.helpControls;
    }

    public void SetMusic(float soundLevel)
    {
        masterMixer.SetFloat("musicVol", soundLevel);

        GameManager.instance.musicVol = soundLevel;
    }

    public void SetEffect(float soundLevel)
    {
        masterMixer.SetFloat("effectVol", soundLevel);

        GameManager.instance.effectVol = soundLevel;
    }

}
