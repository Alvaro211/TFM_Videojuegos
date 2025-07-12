using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdiomasPlay : MonoBehaviour
{
    public GameObject options;
    public List<Texture> spriteOptions;

    public GameObject idomImage;
    public List<Sprite> spriteIdiom;

    public GameObject controlsTextImage;
    public List<Sprite> spriteTextControls;

    public GameObject controlsImage;
    public List<Sprite> spriteControls;

    public TextMeshProUGUI creditos;

    public TextMeshPro[] textHelpBall1;
    public TextMeshPro[] textHelpBall2;

    public TextMeshPro[] textHelpBallRed1;
    public TextMeshPro[] textHelpBallRed2;

    public TextMeshPro[] textHelpEnemy;

    public Book book;

    private void Start()
    {
        ChangeIdiom();
    }

    public void ChangeIdiom()
    {
        if (GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[0];
            options.GetComponent<RawImage>().texture = spriteOptions[0];

            controlsTextImage.GetComponent<Image>().sprite = spriteTextControls[0];
            controlsImage.GetComponent<Image>().sprite = spriteControls[0];

            textHelpBall1[0].gameObject.SetActive(true);
            textHelpBall1[1].gameObject.SetActive(false);
            textHelpBall1[2].gameObject.SetActive(false);

            textHelpBall2[0].gameObject.SetActive(true);
            textHelpBall2[1].gameObject.SetActive(false);
            textHelpBall2[2].gameObject.SetActive(false);

            textHelpBallRed1[0].gameObject.SetActive(true);
            textHelpBallRed1[1].gameObject.SetActive(false);
            textHelpBallRed1[2].gameObject.SetActive(false);

            textHelpBallRed2[0].gameObject.SetActive(true);
            textHelpBallRed2[1].gameObject.SetActive(false);
            textHelpBallRed2[2].gameObject.SetActive(false);

            textHelpEnemy[0].gameObject.SetActive(true);
            textHelpEnemy[1].gameObject.SetActive(false);
            textHelpEnemy[2].gameObject.SetActive(false);

            creditos.text = "Realizado por:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nEn agradecimientos a nuestros tutores:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nMuchas gracias por jugarlo";
        
            for(int i = 0; i < book.bookPages.Length-1; i++)
            {
                if (i == 0)
                {
                    book.bookPages[i] = book.pagina0[0];
                }
                else if ((i == 1 && GameManager.instance.isTakeColeccionable1) || (i == 3 && GameManager.instance.isTakeColeccionable1) || (i == 5 && GameManager.instance.isTakeColeccionable1))
                {
                    book.bookPages[i] = book.bookPageWrittenEsp[i-1];
                    book.bookPages[i+1] = book.bookPageWrittenEsp[i];
                }
            }
        }
        else if (GameManager.instance.idiom == GameManager.Language.Valencian)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[1];
            options.GetComponent<RawImage>().texture = spriteOptions[1];

            controlsTextImage.GetComponent<Image>().sprite = spriteTextControls[1];
            controlsImage.GetComponent<Image>().sprite = spriteControls[1];

            textHelpBall1[0].gameObject.SetActive(false);
            textHelpBall1[1].gameObject.SetActive(true);
            textHelpBall1[2].gameObject.SetActive(false);

            textHelpBall2[0].gameObject.SetActive(false);
            textHelpBall2[1].gameObject.SetActive(true);
            textHelpBall2[2].gameObject.SetActive(false);

            textHelpBallRed1[0].gameObject.SetActive(false);
            textHelpBallRed1[1].gameObject.SetActive(true);
            textHelpBallRed1[2].gameObject.SetActive(false);

            textHelpBallRed2[0].gameObject.SetActive(false);
            textHelpBallRed2[1].gameObject.SetActive(true);
            textHelpBallRed2[2].gameObject.SetActive(false);

            textHelpEnemy[0].gameObject.SetActive(false);
            textHelpEnemy[1].gameObject.SetActive(true);
            textHelpEnemy[2].gameObject.SetActive(false);

            creditos.text = "Realitzat per:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nAgraïments als nostres tutors:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nMoltes gràcies per jugar";

            for (int i = 0; i < book.bookPages.Length - 1; i++)
            {
                if (i == 0)
                {
                    book.bookPages[i] = book.pagina0[1];
                }
                else if ((i == 1 && GameManager.instance.isTakeColeccionable1) || (i == 3 && GameManager.instance.isTakeColeccionable1) || (i == 5 && GameManager.instance.isTakeColeccionable1))
                {
                    book.bookPages[i] = book.bookPageWrittenVal[i-1];
                    book.bookPages[i + 1] = book.bookPageWrittenVal[i];
                }
            }
        }
        else if (GameManager.instance.idiom == GameManager.Language.English)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[2];
            options.GetComponent<RawImage>().texture = spriteOptions[2];

            controlsTextImage.GetComponent<Image>().sprite = spriteTextControls[2];
            controlsImage.GetComponent<Image>().sprite = spriteControls[2];

            textHelpBall1[0].gameObject.SetActive(false);
            textHelpBall1[1].gameObject.SetActive(false);
            textHelpBall1[2].gameObject.SetActive(true);

            textHelpBall2[0].gameObject.SetActive(false);
            textHelpBall2[1].gameObject.SetActive(false);
            textHelpBall2[2].gameObject.SetActive(true);

            textHelpBallRed1[0].gameObject.SetActive(false);
            textHelpBallRed1[1].gameObject.SetActive(false);
            textHelpBallRed1[2].gameObject.SetActive(true);

            textHelpBallRed2[0].gameObject.SetActive(false);
            textHelpBallRed2[1].gameObject.SetActive(false);
            textHelpBallRed2[2].gameObject.SetActive(true);

            textHelpEnemy[0].gameObject.SetActive(false);
            textHelpEnemy[1].gameObject.SetActive(false);
            textHelpEnemy[2].gameObject.SetActive(true);

            creditos.text = "Made by:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nSpecial thanks to our tutors:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nThank you very much for playing";

            for (int i = 0; i < book.bookPages.Length - 1; i++)
            {
                if (i == 0)
                {
                    book.bookPages[i] = book.pagina0[2];
                }
                else if ((i == 1 && GameManager.instance.isTakeColeccionable1) || (i == 3 && GameManager.instance.isTakeColeccionable1) || (i == 5 && GameManager.instance.isTakeColeccionable1))
                {
                    book.bookPages[i] = book.bookPageWrittenEng[i-1];
                    book.bookPages[i + 1] = book.bookPageWrittenEng[i];
                }
            }
        }else if (GameManager.instance.idiom == GameManager.Language.Chino)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[3];
            options.GetComponent<RawImage>().texture = spriteOptions[3];

            controlsTextImage.GetComponent<Image>().sprite = spriteTextControls[3];
            controlsImage.GetComponent<Image>().sprite = spriteControls[3];

            textHelpBall1[0].gameObject.SetActive(false);
            textHelpBall1[1].gameObject.SetActive(false);
            textHelpBall1[2].gameObject.SetActive(true);

            textHelpBall2[0].gameObject.SetActive(false);
            textHelpBall2[1].gameObject.SetActive(false);
            textHelpBall2[2].gameObject.SetActive(true);

            textHelpBallRed1[0].gameObject.SetActive(false);
            textHelpBallRed1[1].gameObject.SetActive(false);
            textHelpBallRed1[2].gameObject.SetActive(true);

            textHelpBallRed2[0].gameObject.SetActive(false);
            textHelpBallRed2[1].gameObject.SetActive(false);
            textHelpBallRed2[2].gameObject.SetActive(true);

            textHelpEnemy[0].gameObject.SetActive(false);
            textHelpEnemy[1].gameObject.SetActive(false);
            textHelpEnemy[2].gameObject.SetActive(true);

            creditos.text = "Made by:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nSpecial thanks to our tutors:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nThank you very much for playing";

            for (int i = 0; i < book.bookPages.Length - 1; i++)
            {
                if (i == 0)
                {
                    book.bookPages[i] = book.pagina0[3];
                }
                else if ((i == 1 && GameManager.instance.isTakeColeccionable1) || (i == 3 && GameManager.instance.isTakeColeccionable1) || (i == 5 && GameManager.instance.isTakeColeccionable1))
                {
                    book.bookPages[i] = book.bookPageWrittenChi[i - 1];
                    book.bookPages[i + 1] = book.bookPageWrittenChi[i];
                }
            }
        }
    }

    public void IdiomLeft()
    {
        if(GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            GameManager.instance.idiom = GameManager.Language.English;
        }
        else if (GameManager.instance.idiom == GameManager.Language.Valencian)
        {
            GameManager.instance.idiom = GameManager.Language.Chino;
        }
        else if (GameManager.instance.idiom == GameManager.Language.English)
        {
            GameManager.instance.idiom = GameManager.Language.Valencian;
        }
        else
        {
            GameManager.instance.idiom = GameManager.Language.Spanish;
        }

        ChangeIdiom();
    }

    public void IdiomRight()
    {
        if (GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            GameManager.instance.idiom = GameManager.Language.English;
        }
        else if (GameManager.instance.idiom == GameManager.Language.English)
        {
            GameManager.instance.idiom = GameManager.Language.Valencian;
        }
        else if (GameManager.instance.idiom == GameManager.Language.Valencian)
        {
            GameManager.instance.idiom = GameManager.Language.Chino;
        }
        else
        {
            GameManager.instance.idiom = GameManager.Language.Spanish;
        }

        ChangeIdiom();
    }
}
