using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class RedpointControl : MonoBehaviour
{
    public Transform player;

    public BallTarget[] all;
    public Toggle t;
    void Start()
    {
        foreach (var b in all)
        {
            b.redPoint.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        foreach (var a in all)
        {

            a.redPoint.transform.position = player.position + (a.ball.transform.position - player.position) .normalized* 1.5f;
        }


        if (t.isOn)
        {
            foreach (var b in all)
            {
                if (b.ball.activeSelf)
                {
                    if (Vector3.Distance(transform.position, b.ball.transform.position) <= 50f)
                    {
                        b.redPoint.SetActive(true);
                    }
                    else{
                        b.redPoint.SetActive(false);
                    }
                }
                else
                {
                    b.redPoint.SetActive(false);
                }
            }
            StopAllCoroutines();
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (var a in all)
        {
            a.redPoint.SetActive(false);
        }
    }
}
[Serializable]
public class BallTarget
{
    public GameObject ball;
    public GameObject redPoint;
}
