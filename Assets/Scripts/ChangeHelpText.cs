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

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            Cursor.visible = false;
            textF.gameObject.SetActive(false);
            iconLB.gameObject.SetActive(true);

            iconStickR.gameObject.SetActive(true);

            textMouse[0].gameObject.SetActive(false);
            textMouse[1].gameObject.SetActive(false);
            textMouse[2].gameObject.SetActive(false);

            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                textHelpBall1[0].transform.localPosition = new Vector3(610.71f, 6.59f, -7.36f);

                iconLB.transform.localPosition = new Vector3(601.96f, 8.8f, 0f);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                textHelpBall1[1].transform.localPosition = new Vector3(610.24f, 6.59f, -7.36f);

                iconLB.transform.localPosition = new Vector3(601.96f, 8.8f, 0f);
            }
            else
            {
                textHelpBall1[2].transform.localPosition = new Vector3(611.91f, 6.59f, -7.36f);

                iconLB.transform.localPosition = new Vector3(602.82f, 8.8f, 0f);
            }
        }
        else
        {
            Cursor.visible = true;
            textF.gameObject.SetActive(true);
            iconLB.gameObject.SetActive(false);

            iconStickR.gameObject.SetActive(false);

            textHelpBall1[0].transform.localPosition = new Vector3(606.76f, 6.59f, -7.36f);
            textHelpBall1[1].transform.localPosition = new Vector3(606.76f, 6.59f, -7.36f);
            textHelpBall1[2].transform.localPosition = new Vector3(606.76f, 6.59f, -7.36f);

            if (GameManager.instance.idiom == GameManager.Language.Spanish)
            {
                textMouse[0].gameObject.SetActive(true);
                textMouse[1].gameObject.SetActive(false);
                textMouse[2].gameObject.SetActive(false);

                textF.transform.localPosition = new Vector3(606.15f, 7.4f, -7.36f);
            }
            else if (GameManager.instance.idiom == GameManager.Language.Valencian)
            {
                textMouse[0].gameObject.SetActive(false);
                textMouse[1].gameObject.SetActive(true);
                textMouse[2].gameObject.SetActive(false);

                textF.transform.localPosition = new Vector3(607.64f, 7.4f, -7.36f);
            }
            else
            {
                textMouse[0].gameObject.SetActive(false);
                textMouse[1].gameObject.SetActive(false);
                textMouse[2].gameObject.SetActive(true);

                textF.transform.localPosition = new Vector3(607.64f, 7.4f, -7.36f);
            }        
        }
    }
}
