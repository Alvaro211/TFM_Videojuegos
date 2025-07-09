using System;
using System.Collections;
using System.Collections.Generic;
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
    public void ChangeIdiom()
    {
        if (GameManager.instance.idiom == GameManager.Language.Spanish)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[0];
            newGame.GetComponent<Image>().sprite = spritenewGame[0];
            continueGame.GetComponent<Image>().sprite = spriteContinue[0];
            options.GetComponent<RawImage>().texture = spriteOptions[0];
        }
        else if (GameManager.instance.idiom == GameManager.Language.Valencian)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[1];
            newGame.GetComponent<Image>().sprite = spritenewGame[1];
            continueGame.GetComponent<Image>().sprite = spriteContinue[1];
            options.GetComponent<RawImage>().texture = spriteOptions[1];
        }
        else if (GameManager.instance.idiom == GameManager.Language.English)
        {
            idomImage.GetComponent<Image>().sprite = spriteIdiom[2];
            newGame.GetComponent<Image>().sprite = spritenewGame[2];
            continueGame.GetComponent<Image>().sprite = spriteContinue[2];
            options.GetComponent<RawImage>().texture = spriteOptions[2];
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
