using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ControlMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public GameObject cargar;
    public GameObject cargarControls;
    public GameObject buttonBack;

    public GameObject controls;
    public GameObject quit;

    public SoundManager audio;

    public Image imageContinue;

    public Transform canvas;

    public void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 0)
        {
            if (Gamepad.current != null)
                UnityEngine.Cursor.visible = true;

            audio.AudioMenu();
            if (!GameManager.instance.continueGame)
                imageContinue.color = new Color(0.33f, 0.33f, 0.33f);
            else
                imageContinue.color = new Color(1f, 1f, 1f);
        }
        else if (index == 1)
            audio.AudioPlay();

        if (cargar != null)
        {
            DontDestroyOnLoad(cargar);
            cargar.SetActive(false);
            GameManager.instance.cargar = cargar;
        }

    }

    public void Play()
    {
        if (GameManager.instance.continueGame)
        {
            cargar.SetActive(true);
            GameManager.instance.newGame = false;
            //SceneManager.LoadScene(1);
            StartCoroutine(GameManager.instance.LoadSceneAsync("Level"));
        }
    }

    public void PlayNewGame()
    {
        cargar.SetActive(true);
        GameManager.instance.newGame = true;
        //SceneManager.LoadScene(1);
        StartCoroutine(GameManager.instance.LoadSceneAsync("Level"));
    }

    

    public void ShowOptions()
    {
        mainMenu.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    public void HideOptions()
    {
        mainMenu.gameObject.SetActive(true);
        options.gameObject.SetActive(false);

        GameManager.instance.SaveMusicMenu();
    }

    public void HideOptionsPlay()
    {
        if (controls.activeInHierarchy)
        {
            controls.SetActive(false);
            quit.SetActive(true);
        }
        else
        {
            foreach (Transform hijo in canvas)
            {
                if (hijo.name == "CirculoNota(Clone)")
                {
                    hijo.gameObject.SetActive(true);
                }
            }

            Time.timeScale = 1;
            GameManager.instance.canMove = true;
            options.gameObject.SetActive(false);
        }
    }

    public void Exit()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void GoToMenu()
    {
        GameManager.instance.DisableCargar();
        SceneManager.LoadScene(0);
    }

    public void ShowControl()
    {
        controls.SetActive(true);
        quit.SetActive(false);
    }

    public void ShowCargar()
    {
        cargarControls.SetActive(true);
        buttonBack.SetActive(true);
        options.SetActive(false);
    }

    public void HideControls()
    {
        cargarControls.SetActive(false);
        buttonBack.SetActive(false);
        options.SetActive(true);
    }

}
