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

    private PlayerMovement movement;
    void Start()
    {
        foreach (var b in all)
        {
            b.redPoint.SetActive(false);
        }
        t.onValueChanged.AddListener((b) =>
        {
            StopAllCoroutines();
            foreach (var a in all)
            {
                a.redPoint.SetActive(b);
            }
        });

        movement = player.GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (var a in all)
        {
            a.redPoint.transform.position = player.position + (a.ball.transform.position - player.position) .normalized* 3f;

            Vector3 dir2D = a.ball.transform.position - a.redPoint.transform.position;
            float angle = Mathf.Atan2(dir2D.y, dir2D.x) * Mathf.Rad2Deg;
            a.redPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
        }


        if (t.isOn)
        {
            foreach (var b in all)
            {
                if (b.ball.transform.GetChild(0).GetChild(0).gameObject.activeSelf)
                {
                    if (movement.hearingSound[i])
                    {
                        b.redPoint.SetActive(true);
                    }
                    else
                    {
                        b.redPoint.SetActive(false);
                    }
                }
                else
                {
                    b.redPoint.SetActive(false);
                }

                i++;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                foreach (var b in all)
                {
                    if (b.ball.transform.GetChild(0).GetChild(0).gameObject.activeSelf)
                    {
                        if (movement.hearingSound[i])
                        {
                            b.redPoint.SetActive(true);
                        }
                        else
                        {
                            b.redPoint.SetActive(false);
                        }
                    }
                    else
                    {
                        b.redPoint.SetActive(false);
                    }

                    i++;
                }
                StopAllCoroutines();
                StartCoroutine(Fade());
            }
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(2f);
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
