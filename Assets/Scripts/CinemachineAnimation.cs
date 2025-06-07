using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CinemachineAnimation : MonoBehaviour
{
    public PlayableDirector directorLevel1;
    public CinemachineVirtualCamera virtualCamera1;
    public List<Light> lightsLevel1 = new List<Light>();

    public PlayableDirector directorLevel2;
    public CinemachineVirtualCamera virtualCamera2;
    public List<Light> lightsLevel2 = new List<Light>();

    public PlayableDirector directorLevel3;
    public CinemachineVirtualCamera virtualCamera3;
    public List<Light> lightsLevel3 = new List<Light>();

    public PlayableDirector directorLevel4;
    public CinemachineVirtualCamera virtualCamera4;
    public List<Light> lightsLevel4 = new List<Light>();

    public Transform canvas;

    private void Start()
    {
        var dolly = virtualCamera1.GetCinemachineComponent<CinemachineTrackedDolly>();

        if (dolly != null)
        {
            dolly.m_PathPosition = 0f;
        }

        dolly = virtualCamera2.GetCinemachineComponent<CinemachineTrackedDolly>();

        if (dolly != null)
        {
            dolly.m_PathPosition = 0f;
        }

        dolly = virtualCamera3.GetCinemachineComponent<CinemachineTrackedDolly>();

        if (dolly != null)
        {
            dolly.m_PathPosition = 0f;
        }

        dolly = virtualCamera4.GetCinemachineComponent<CinemachineTrackedDolly>();

        if (dolly != null)
        {
            dolly.m_PathPosition = 0f;
        }
    }
    private void OnEnable()
    {
        if (directorLevel1 != null)
            directorLevel1.stopped += OnTimelineFinishedLevel1;

        if (directorLevel2 != null)
            directorLevel2.stopped += OnTimelineFinishedLevel2;

        if (directorLevel3 != null)
            directorLevel3.stopped += OnTimelineFinishedLevel3;

        if (directorLevel4 != null)
            directorLevel4.stopped += OnTimelineFinishedLevel4;
    }

    private void OnDisable()
    {
        if (directorLevel1 != null)
            directorLevel1.stopped -= OnTimelineFinishedLevel1;

        if (directorLevel2 != null)
            directorLevel2.stopped -= OnTimelineFinishedLevel2;

        if (directorLevel3 != null)
            directorLevel3.stopped -= OnTimelineFinishedLevel3;

        if (directorLevel4 != null)
            directorLevel4.stopped -= OnTimelineFinishedLevel4;


    }

    public void PlayTimelineLevel1()
    {
        if (directorLevel1 != null)
        {
            GameManager.instance.canMove = false;
            TurnOnLightsLevel1();
            directorLevel1.Play();
        }
    }

    private void OnTimelineFinishedLevel1(PlayableDirector pd)
    {
        TurnOffLightsLevel1();
        canvas.gameObject.SetActive(true);
        GameManager.instance.canMove = true;
    }

    public void TurnOffLightsLevel1()
    {
        foreach (Light light in lightsLevel1)
        {
            if (light != null)
            {
                light.intensity = 0;
                light.enabled = false;
            }
        }

        GameManager.instance.canMove = true;
    }

    public void TurnOnLightsLevel1()
    {
        foreach (Light light in lightsLevel1)
        {
            if (light != null)
            {
                light.enabled = true;
                light.intensity = 90f;
            }
        }
    }


    public void PlayTimelineLevel2()
    {
        if (directorLevel2 != null)
        {
            GameManager.instance.canMove = false;
            TurnOnLightsLevel2();
            directorLevel2.Play();
        }
    }

    private void OnTimelineFinishedLevel2(PlayableDirector pd)
    {
        canvas.gameObject.SetActive(true);
        TurnOffLightsLevel2();
        GameManager.instance.canMove = true;
    }

    public void TurnOffLightsLevel2()
    {
        foreach (Light light in lightsLevel2)
        {
            if (light != null)
            {
                light.intensity = 0;
                light.enabled = false;
            }
        }

        GameManager.instance.canMove = true;
    }

    public void TurnOnLightsLevel2()
    {
        foreach (Light light in lightsLevel2)
        {
            if (light != null)
            {
                light.enabled = true;
                light.intensity = 90f;
            }
        }
    }

    public void PlayTimelineLevel3()
    {
        if (directorLevel3 != null)
        {
            GameManager.instance.canMove = false;
            TurnOnLightsLevel3();
            directorLevel3.Play();
        }
    }

    private void OnTimelineFinishedLevel3(PlayableDirector pd)
    {
        canvas.gameObject.SetActive(true);
        TurnOffLightsLevel3();
        GameManager.instance.canMove = true;
    }

    public void TurnOffLightsLevel3()
    {
        foreach (Light light in lightsLevel3)
        {
            if (light != null)
            {
                light.intensity = 0;
                light.enabled = false;
            }
        }

        GameManager.instance.canMove = true;
    }

    public void TurnOnLightsLevel3()
    {
        foreach (Light light in lightsLevel3)
        {
            if (light != null)
            {
                light.enabled = true;
                light.intensity = 90f;
            }
        }
    }

    public void PlayTimelineLevel4()
    {
        if (directorLevel4 != null)
        {
            GameManager.instance.canMove = false;
            TurnOnLightsLevel4();
            directorLevel4.Play();
        }
    }

    private void OnTimelineFinishedLevel4(PlayableDirector pd)
    {
        canvas.gameObject.SetActive(true);
        TurnOffLightsLevel4();
        GameManager.instance.canMove = true;
    }

    public void TurnOffLightsLevel4()
    {
        foreach (Light light in lightsLevel4)
        {
            if (light != null)
            {
                light.intensity = 0;
                light.enabled = false;
            }
        }

        GameManager.instance.canMove = true;
    }

    public void TurnOnLightsLevel4()
    {
        foreach (Light light in lightsLevel4)
        {
            if (light != null)
            {
                light.enabled = true;
                light.intensity = 90f;
            }
        }
    }
}
