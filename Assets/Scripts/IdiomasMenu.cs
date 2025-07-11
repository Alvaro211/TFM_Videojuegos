using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdiomasMenu : MonoBehaviour
{
    public GameObject newGame;
    public List<Sprite> spritenewGame;

    public GameObject continueGame;
    public List<Sprite> spriteContinue;

    public GameObject options;
    public List<Texture> spriteOptions;

    public GameObject idomImage;
    public List<Sprite> spriteIdiom;

    public GameObject cargarImage;
    public List<Sprite> spriteCargar;

    public TextMeshProUGUI creditos;

    private void Start()
    {
        GameManager.instance.Load();
        IdiomLeft();
    }
    public void ChangeIdiom()
    {
        if (GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[0];
            newGame.GetComponent<Image>().sprite = spritenewGame[0];

            Color color1 = newGame.GetComponent<Image>().color;
            color1.a = 0.6f;
            newGame.GetComponent<Image>().color = color1;


            continueGame.GetComponent<Image>().sprite = spriteContinue[0];

            Color color2 = continueGame.GetComponent<Image>().color;
            color2.a = 0.6f;
            continueGame.GetComponent<Image>().color = color2;

            options.GetComponent<RawImage>().texture = spriteOptions[0];
            cargarImage.GetComponent<SpriteRenderer>().sprite = spriteCargar[0];

            creditos.text = "Realizado por:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nEn agradecimientos a nuestros tutores:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nMuchas gracias por jugarlo";
        }
        else if (GameManager.instance.idiom == GameManager.Language.Valencian)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[1];
            newGame.GetComponent<Image>().sprite = spritenewGame[1];

            Color color1 = newGame.GetComponent<Image>().color;
            color1.a = 0.6f;
            newGame.GetComponent<Image>().color = color1;


            continueGame.GetComponent<Image>().sprite = spriteContinue[1];

            Color color2 = continueGame.GetComponent<Image>().color;
            color2.a = 0.6f;
            continueGame.GetComponent<Image>().color = color2;

            options.GetComponent<RawImage>().texture = spriteOptions[1];
            cargarImage.GetComponent<SpriteRenderer>().sprite = spriteCargar[1];
            
            creditos.text = "Realitzat per:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nAgraïments als nostres tutors:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nMoltes gràcies per jugar";

        }
        else if (GameManager.instance.idiom == GameManager.Language.English)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[2];
            newGame.GetComponent<Image>().sprite = spritenewGame[2];

            Color color1 = newGame.GetComponent<Image>().color;
            color1.a = 0.6f;
            newGame.GetComponent<Image>().color = color1;


            continueGame.GetComponent<Image>().sprite = spriteContinue[2];

            Color color2 = continueGame.GetComponent<Image>().color;
            color2.a = 0.6f;
            continueGame.GetComponent<Image>().color = color2;

            options.GetComponent<RawImage>().texture = spriteOptions[2];
            cargarImage.GetComponent<SpriteRenderer>().sprite = spriteCargar[2];

            creditos.text = "Made by:\r\n\r\n\tJulia Martinez Campos\r\n\tAlvaro Perez Martinez\r\n\tYihang Sun\r\n\tJuan David Acevedo\r\n\r\nSpecial thanks to our tutors:\r\n\t\r\n\tSergio Gonzalez Jimenez\r\n\tSergio Rodriguez Gonzalez\r\n\tSergio Garcia Cabezas\r\n\r\nThank you very much for playing";
        }
    }

    public void IdiomLeft()
    {
        if(GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            GameManager.instance.idiom = GameManager.Language.Valencian;

            continueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 350f);
            continueGame.transform.localPosition = new Vector3(212, -20, 0f);

            newGame.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 350);
            newGame.transform.localPosition = new Vector3(-309f, 15f, 0f);
        }
        else if (GameManager.instance.idiom == GameManager.Language.Valencian)
        {
            GameManager.instance.idiom = GameManager.Language.English;

            continueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 600f);
            continueGame.transform.localPosition = new Vector3(212f, 9f, 0f);

            newGame.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 517);
            newGame.transform.localPosition = new Vector3(-309f, 45f, 0f);
        }
        else
        {
            GameManager.instance.idiom = GameManager.Language.Spanish;

            continueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 350f);
            continueGame.transform.localPosition = new Vector3(212f, -20, 0f);

            newGame.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 350);
            newGame.transform.localPosition = new Vector3(-309f, 15f, 0f);
        }

        ChangeIdiom();
    }

    public void IdiomRight()
    {
        if (GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            GameManager.instance.idiom = GameManager.Language.English;

            continueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 600f);
            continueGame.transform.localPosition = new Vector3(212f, 9f, 0f);

            newGame.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 517);
            newGame.transform.localPosition = new Vector3(-309f, 45f, 0f);
        }
        else if (GameManager.instance.idiom == GameManager.Language.English)
        {
            GameManager.instance.idiom = GameManager.Language.Valencian;

            continueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 350f);
            continueGame.transform.localPosition = new Vector3(212f, -20, 0f);

            newGame.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 350);
            newGame.transform.localPosition = new Vector3(-309f, 15f, 0f);
        }
        else
        {
            GameManager.instance.idiom = GameManager.Language.Spanish;

            continueGame.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 350f);
            continueGame.transform.localPosition = new Vector3(212f, -20, 0f);

            newGame.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 350);
            newGame.transform.localPosition = new Vector3(-309f, 15f, 0f);
        }

        ChangeIdiom();
    }
}
