using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeFalling : MonoBehaviour
{
    public float fastCubeSpeed = 5f;
    public float fallDistance = 2f;
    public float riseSpeed = 2f;
    public float waitTime = 1f;

    public List<Light> spotlights;
    public GameObject effect;
    private Vector3 fastCubeStartPos;
    private bool fastCubeFalling = true;
    private bool isWaiting = false;
    private bool sound = false;
    private AudioSource audio;

    private float maxIntensity;
    private float minIntensity = 0f;
    private float lightTransitionSpeed = 50f;
    private float startPositonX;

    public SpriteRenderer spriteRenderer;
    private Color colorArriba = new Color(0.3f, 0.3f, 0.3f, 1f);
    private Color colorAbajo = Color.white;
    public float colorChangeSpeedDown = 160f;
    public float colorChangeSpeedUp = 80f;

    void Start()
    {
        fastCubeStartPos = transform.position;
        startPositonX = transform.position.x;
        audio = GetComponent<AudioSource>();

        if (spotlights != null && spotlights.Count > 0)
        {
            maxIntensity = spotlights[0].intensity;
        }
    }

    void Update()
    {
        UpdateSpriteColor();

        if (!isWaiting)
        {
            if (fastCubeFalling)
            {
                

                transform.position += new Vector3(0, -fastCubeSpeed * Time.deltaTime, 0);

                if (fastCubeStartPos.y - transform.position.y >= fallDistance)
                {
                    fastCubeFalling = false;
                    Instantiate(effect).transform.position = transform.position;
                }
            }
            else
            {
                if (sound)
                {
                    sound = false;
                    audio.Play();
                }

                transform.position += new Vector3(0, riseSpeed * Time.deltaTime, 0);

                if (transform.position.y >= fastCubeStartPos.y)
                {
                    transform.position = fastCubeStartPos;
                    StartCoroutine(WaitBeforeFalling());
                }
            }
        }

        Vector3 pos = transform.position;
        pos.x = startPositonX; // Fijamos X para que no se desplace lateralmente
        transform.position = pos;

        LigthTrasparecen(); // Llamado directo en vez de usar Invoke
    }

    private void LigthTrasparecen()
    {
        if (spotlights != null && spotlights.Count > 0)
        {
            float targetIntensity = fastCubeFalling ? maxIntensity : minIntensity;

            foreach (Light light in spotlights)
            {
                if (light != null)
                {
                    light.intensity = Mathf.MoveTowards(
                        light.intensity,
                        targetIntensity,
                        lightTransitionSpeed * Time.deltaTime
                    );
                }
            }
        }
    }

    private IEnumerator WaitBeforeFalling()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        fastCubeFalling = true;
        isWaiting = false;
        sound = true;
    }

    private void UpdateSpriteColor()
    {
        if (spriteRenderer != null)
        {
            Color targetColor = fastCubeFalling ? colorAbajo : colorArriba;

            if (fastCubeFalling)
            {
                spriteRenderer.color = Color.Lerp(
                    spriteRenderer.color,
                    targetColor,
                    colorChangeSpeedDown * Time.deltaTime
                );
            }
            else
            {
                spriteRenderer.color = Color.Lerp(
                    spriteRenderer.color,
                    targetColor,
                    colorChangeSpeedUp * Time.deltaTime
                );
            }
        }
    }
}
