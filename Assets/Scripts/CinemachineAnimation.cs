using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CinemachineAnimation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualMain;

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

    public PlayableDirector directorLevel5;
    public CinemachineVirtualCamera virtualCamera5;
    public List<Light> lightsLevel5 = new List<Light>();

    public CinemachineImpulseSource impulseSource;

    public Transform canvas;

    public AudioSource musicAudioSource;

    public GameObject ball;
    public GameObject enemy;
    public GameObject enemyAnimation1;

    public GameObject boss;

    private GameObject enemyInstanciate;
    private GameObject ballInstanciate;

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

        virtualMain.m_Lens.OrthographicSize = 10;
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

        if (directorLevel5 != null)
            directorLevel5.stopped += OnTimelineFinishedLevel5;
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

        if (directorLevel5 != null)
            directorLevel5.stopped -= OnTimelineFinishedLevel5;


    }

    public void PlayTimelineLevel1()
    {
        if (directorLevel1 != null)
        {
            canvas.gameObject.SetActive(false);
            GameManager.instance.canMove = false;
            TurnOnLightsLevel1();
            directorLevel1.Play();
            enemyAnimation1.SetActive(false);
            enemyInstanciate = Instantiate(enemy, new Vector3(792, 17, -8), Quaternion.identity);
            Enemy enemyScrupt = enemyInstanciate.GetComponent<Enemy>();
            enemyScrupt.patrolDistance = 0;
            enemyInstanciate.SetActive(true);
            Invoke("ShowBallAndEnemy", 9f);
        }
    }

    private void ShowBallAndEnemy()
    {
        ballInstanciate = Instantiate(ball, new Vector3(784, 18.5f, -8), Quaternion.identity);

        Rigidbody rb = ballInstanciate.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, -10, 0);
        rb.isKinematic = false;
    }

    private void OnTimelineFinishedLevel1(PlayableDirector pd)
    {
        TurnOffLightsLevel1();
        canvas.gameObject.SetActive(true);
        GameManager.instance.canMove = true;

        enemyInstanciate.SetActive(false);
        enemyAnimation1.SetActive(true);
        ballInstanciate.SetActive(false);
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
            canvas.gameObject.SetActive(false);
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
            canvas.gameObject.SetActive(false);
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
            canvas.gameObject.SetActive(false);
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

    public void PlayTimelineLevel5()
    {
        if (directorLevel5 != null)
        {
            canvas.gameObject.SetActive(false);
            GameManager.instance.canMove = false;
            TurnOnLightsLevel5();
            directorLevel5.Play();

            Invoke("IncreaseOrthoSize", 2f);

            BossConroller boosController = boss.GetComponent<BossConroller>();
            boosController.onAnimation = true;
            boosController.Invoke("StartAnimation", 12f);
            Invoke("ImpulseCameraBoss", 12f);
            StartCoroutine(FadeOutCoroutine(5f));
        }
    }

    private void IncreaseOrthoSize()
    {
        virtualMain.m_Lens.OrthographicSize = 20;

        var transposer = virtualMain.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (transposer != null)
        {
            transposer.m_ScreenY = 0.72f;
        }
    }

    private void ImpulseCameraBoss()
    {
        impulseSource.GenerateImpulse();
        AudioSource bossAudioSource = boss.GetComponent<AudioSource>();
        bossAudioSource.Play();
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = musicAudioSource.volume;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop();
    }

    private void OnTimelineFinishedLevel5(PlayableDirector pd)
    {
        musicAudioSource.volume = 1f;
        musicAudioSource.Play();
        canvas.gameObject.SetActive(true);
        TurnOffLightsLevel5();
        GameManager.instance.canMove = true; 
        
        BossConroller boosController = boss.GetComponent<BossConroller>();
        boosController.onAnimation = false;

        foreach (Transform hijo in canvas)
        {
            if (hijo.name == "CirculoNota(Clone)")
            {
                hijo.position = new Vector3(hijo.position.x, hijo.position.y-20, hijo.position.z);
            }
        }
    }

    public void TurnOffLightsLevel5()
    {
        foreach (Light light in lightsLevel5)
        {
            if (light != null)
            {
                light.intensity = 0;
                light.enabled = false;
            }
        }

        GameManager.instance.canMove = true;
    }

    public void TurnOnLightsLevel5()
    {
        foreach (Light light in lightsLevel5)
        {
            if (light != null)
            {
                light.enabled = true;
                light.intensity = 90f;
            }
        }
    }
}
