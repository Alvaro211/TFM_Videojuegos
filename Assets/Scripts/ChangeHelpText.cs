using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeHelpText : MonoBehaviour
{
    public TextMeshPro textF;
    public TextMeshPro[] textMouse;
    public SpriteRenderer iconLB;

    public SpriteRenderer iconStickR;

    public TextMeshPro[] textHelpBall1;
    public TextMeshPro[] textLearnToSee;

    public GameObject options;
    public GameObject diary;

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (options.activeInHierarchy || diary.activeInHierarchy)
                Cursor.visible = true;
            else
                Cursor.visible = false;
            textF.gameObject.SetActive(false);
            iconLB.gameObject.SetActive(true);

            iconStickR.gameObject.SetActive(true);

            textMouse[0].gameObject.SetActive(false);
            textMouse[1].gameObject.SetActive(false);
            textMouse[2].gameObject.SetActive(false);

            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                
                textHelpBall1[0].transform.localPosition = new Vector3(564.64f, 31.30f, -7.36f);

                textLearnToSee[0].transform.localPosition = new Vector3(570.17f, 33.43f, -7.36f);

                iconLB.transform.localPosition = new Vector3(601.96f, 8.8f, 0f);

                iconStickR.transform.localPosition = new Vector3(612f, 8.8f, 0f);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                textHelpBall1[1].transform.localPosition = new Vector3(564.64f, 31.30f, -7.36f);
                textLearnToSee[1].transform.localPosition = new Vector3(570.17f, 33.43f, -7.36f);

                iconLB.transform.localPosition = new Vector3(601.96f, 8.8f, 0f);

                iconStickR.transform.localPosition = new Vector3(612f, 8.8f, 0f);
            }
            else
            {
                textHelpBall1[2].transform.localPosition = new Vector3(564.64f, 31.30f, -7.36f);
                textLearnToSee[2].transform.localPosition = new Vector3(570.17f, 33.43f, -7.36f);


                iconLB.transform.localPosition = new Vector3(602.82f, 8.8f, 0f);

                iconStickR.transform.localPosition = new Vector3(614f, 8.8f, 0f);
            }
        }
        else
        {
            Cursor.visible = true;
            textF.gameObject.SetActive(true);
            iconLB.gameObject.SetActive(false);

            iconStickR.gameObject.SetActive(false);

            textHelpBall1[0].transform.localPosition = new Vector3(564.64f, 31.30f, -7.36f);
            textHelpBall1[1].transform.localPosition = new Vector3(564.64f, 31.30f, -7.36f);
            textHelpBall1[2].transform.localPosition = new Vector3(564.64f, 31.30f, -7.36f);

            textLearnToSee[0].transform.localPosition = new Vector3(570.17f, 33.43f, -7.36f);
            textLearnToSee[1].transform.localPosition = new Vector3(570.17f, 33.43f, -7.36f);
            textLearnToSee[2].transform.localPosition = new Vector3(570.17f, 33.43f, -7.36f);

            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                textMouse[0].gameObject.SetActive(true);
                textMouse[1].gameObject.SetActive(false);
                textMouse[2].gameObject.SetActive(false);

                textF.transform.localPosition = new Vector3(561.77f, 32.12f, -7.36f);

                textLearnToSee[0].gameObject.SetActive(true);
                textLearnToSee[1].gameObject.SetActive(false);
                textLearnToSee[2].gameObject.SetActive(false);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                textMouse[0].gameObject.SetActive(false);
                textMouse[1].gameObject.SetActive(true);
                textMouse[2].gameObject.SetActive(false);

                textF.transform.localPosition = new Vector3(562.60f, 32.12f, -7.36f);

                textLearnToSee[0].gameObject.SetActive(false);
                textLearnToSee[1].gameObject.SetActive(true);
                textLearnToSee[2].gameObject.SetActive(false);
            }
            else
            {
                textMouse[0].gameObject.SetActive(false);
                textMouse[1].gameObject.SetActive(false);
                textMouse[2].gameObject.SetActive(true);

                textF.transform.localPosition = new Vector3(563.59f, 32.12f, -7.36f);

                textLearnToSee[0].gameObject.SetActive(false);
                textLearnToSee[1].gameObject.SetActive(false);
                textLearnToSee[2].gameObject.SetActive(true);

            }        
        }
    }
}
