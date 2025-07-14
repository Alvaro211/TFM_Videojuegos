using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
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

    public GameObject firstSelectedContinue;
    public GameObject firstSelectedNewGame;
    public InputActionAsset inputActions;

    public GameObject[] menuPrincipal;
    public GameObject[] menuOpciones;
    public GameObject[] playOpciones;

    public Vector3[] scaleMenuPrincipal;
    public Vector3[] scaleMenuOpciones;

    private Coroutine corutine;
    private string corutineText = "";

    public void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 0)
        {
            if (Gamepad.current != null)
                UnityEngine.Cursor.visible = false;

            audio.AudioMenu();

            EventSystem.current.SetSelectedGameObject(null);

            inputActions.FindActionMap("UI").Enable();

            if (!GameManager.instance.continueGame)
            {
                imageContinue.color = new Color(0.33f, 0.33f, 0.33f);
                
                if(Gamepad.current != null)
                    EventSystem.current.SetSelectedGameObject(firstSelectedNewGame);
                else
                    EventSystem.current.SetSelectedGameObject(firstSelectedNewGame);
            }
            else
            {
                imageContinue.color = new Color(1f, 1f, 1f);

                if (Gamepad.current != null)
                    EventSystem.current.SetSelectedGameObject(firstSelectedContinue);
                else
                    EventSystem.current.SetSelectedGameObject(firstSelectedNewGame);
            }
        }
        else if (index == 1)
            audio.AudioPlay();

        if (cargar != null)
        {
            DontDestroyOnLoad(cargar);
            cargar.SetActive(false);
            GameManager.instance.cargar = cargar;
        }

        scaleMenuPrincipal = new Vector3[menuPrincipal.Length];

        for(int i = 0; i < scaleMenuPrincipal.Length; i++)
        {
            scaleMenuPrincipal[i] = menuPrincipal[i].transform.localScale;
        }

        scaleMenuOpciones = new Vector3[menuOpciones.Length];

        for (int i = 0; i < scaleMenuOpciones.Length; i++)
        {
            scaleMenuOpciones[i] = menuOpciones[i].transform.localScale;
        }
    }

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (Gamepad.current != null)
            Cursor.visible = false;
        else
            Cursor.visible = true;

        if (selected != null)
            Debug.Log(selected.name);
        else
            Debug.Log("null");

        if (selected != null && Gamepad.current != null)
        {

            if (selected.name == "Options")
            {
                if (corutine == null || corutineText != "Options")
                {
                    if(corutineText != "Options" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuPrincipal[0], scaleMenuPrincipal[0]));
                    corutineText = "Options";

                    menuPrincipal[0].GetComponent<RawImage>().color = new Color(0.3f, 0.98f, 0.98f, 1);
                    menuPrincipal[1].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[2].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "Creditos")
            {
                if (corutine == null || corutineText != "Creditos")
                {
                    if(corutineText != "Creditos" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuPrincipal[1], scaleMenuPrincipal[1]));
                    corutineText = "Creditos";

                    menuPrincipal[0].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[1].GetComponent<RawImage>().color =  new Color(0.3f, 0.98f, 0.98f, 1);
                    menuPrincipal[2].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "Exit")
            {
                if (corutine == null || corutineText != "Exit")
                {
                    if (corutineText != "Exit" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuPrincipal[2], scaleMenuPrincipal[2]));
                    corutineText = "Exit";

                    menuPrincipal[0].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[1].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[2].GetComponent<RawImage>().color = new Color(0.3f, 0.98f, 0.98f, 1);
                }
            }
            else if (selected.name == "ButtonPlay")
            {
                if (corutine == null || corutineText != "ButtonPlay")
                {
                    if (corutineText != "ButtonPlay" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuPrincipal[3], scaleMenuPrincipal[3]));
                    corutineText = "ButtonPlay";

                    menuPrincipal[0].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[1].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[2].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "ButtonNewGame")
            {
                if (corutine == null || corutineText != "ButtonNewGame")
                {
                    if (corutineText != "ButtonNewGame" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuPrincipal[4], scaleMenuPrincipal[4]));
                    corutineText = "ButtonNewGame";

                    menuPrincipal[0].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[1].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuPrincipal[2].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "Tree")
            {
                if (corutine == null || corutineText != "Tree")
                {
                    if (corutineText != "Tree" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuOpciones.Length; i++)
                    {
                        menuOpciones[i].transform.localScale = scaleMenuOpciones[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuOpciones[6], scaleMenuOpciones[6]));
                    corutineText = "Tree";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(0.3f, 0.98f, 0.98f, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "SliderMusic")
            {
                if (corutine == null || corutineText != "SliderMusic")
                {
                    if (corutineText != "SliderMusic" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    //corutine = StartCoroutine(ScaleLoop(menuPrincipal[4], scaleMenuPrincipal[4]));
                    corutineText = "SliderMusic";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 0, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "SliderEffect")
            {
                if (corutine == null || corutineText != "SliderEffect")
                {
                    if (corutineText != "SliderEffect" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuPrincipal.Length; i++)
                    {
                        menuPrincipal[i].transform.localScale = scaleMenuPrincipal[i];
                    }

                    //corutine = StartCoroutine(ScaleLoop(menuPrincipal[4], scaleMenuPrincipal[4]));
                    corutineText = "SliderEffect";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 0, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "ButtonLeft")
            {
                if (corutine == null || corutineText != "ButtonNewGame")
                {
                    if (corutineText != "ButtonNewGame" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuOpciones.Length; i++)
                    {
                        menuOpciones[i].transform.localScale = scaleMenuOpciones[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuOpciones[3], scaleMenuOpciones[3]));
                    corutineText = "ButtonNewGame";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 0, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "ButtonRight")
            {
                if (corutine == null || corutineText != "ButtonRight")
                {
                    if (corutineText != "ButtonRight" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuOpciones.Length; i++)
                    {
                        menuOpciones[i].transform.localScale = scaleMenuOpciones[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuOpciones[4], scaleMenuOpciones[4]));
                    corutineText = "ButtonRight";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 0, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "ButtonControls")
            {
                if (corutine == null || corutineText != "ButtonControls")
                {
                    if (corutineText != "ButtonControls" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuOpciones.Length; i++)
                    {
                        menuOpciones[i].transform.localScale = scaleMenuOpciones[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuOpciones[5], scaleMenuOpciones[5]));
                    corutineText = "ButtonControls";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else if (selected.name == "CargarControlesAtras")
            {
                if (corutine == null || corutineText != "CargarControlesAtras")
                {
                    if (corutineText != "CargarControlesAtras" && corutine != null)
                        StopCoroutine(corutine);

                    for (int i = 0; i < scaleMenuOpciones.Length; i++)
                    {
                        menuOpciones[i].transform.localScale = scaleMenuOpciones[i];
                    }

                    corutine = StartCoroutine(ScaleLoop(menuOpciones[10], scaleMenuOpciones[10]));
                    corutineText = "CargarControlesAtras";

                    menuOpciones[3].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[4].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[6].GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                    menuOpciones[7].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[8].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    menuOpciones[10].GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.98f, 0.98f, 1);
                }
            }
        }
        else if (Gamepad.current != null && selected == null)
        {
            if(mainMenu.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(firstSelectedNewGame);
            else if(options.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(menuOpciones[0]);
            else if(cargarControls)
                EventSystem.current.SetSelectedGameObject(menuOpciones[8]);
        }
        else if (corutine != null)
            StopCoroutine(corutine);
    }

    IEnumerator ScaleLoop(GameObject selected, Vector3 originalScale)
    {
        float duration = 1f;
        float scale = 1.1f;

        if (selected.name == "ButtonLeft" || selected.name == "ButtonRight")
            scale = 1.5f;

        Vector3 targetScale = selected.transform.localScale * scale;


        while (true)
        {
            // Escala hacia arriba
            yield return StartCoroutine(ScaleTo(targetScale, duration, selected));

            // Escala hacia abajo
            yield return StartCoroutine(ScaleTo(originalScale, duration, selected));
        }
    }

    IEnumerator ScaleTo(Vector3 target, float time, GameObject selected)
    {
        Vector3 start = selected.transform.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            selected.transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }

        selected.transform.localScale = target; // Asegurarse de que acaba en el valor exacto
    }

    public void Play()
    {
        if (GameManager.instance.continueGame)
        {
            cargar.SetActive(true);
            GameManager.instance.newGame = false;
            //SceneManager.LoadScene(1);
            //StartCoroutine(GameManager.instance.LoadSceneAsync("Level"));
            StartCoroutine(GameManager.instance.StartProggresBar(cargar));
        }
    }

    public void PlayNewGame()
    {
        cargar.SetActive(true);
        GameManager.instance.newGame = true;
        //SceneManager.LoadScene(1);
        //StartCoroutine(GameManager.instance.LoadSceneAsync("Level"));
        StartCoroutine(GameManager.instance.StartProggresBar(cargar));
    }

    

    public void ShowOptions()
    {
        mainMenu.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(menuOpciones[0]);
    }

    public void HideOptions()
    {
        mainMenu.gameObject.SetActive(true);
        options.gameObject.SetActive(false);

        GameManager.instance.SaveMusicMenu();

        EventSystem.current.SetSelectedGameObject(menuPrincipal[0]);
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


        EventSystem.current.SetSelectedGameObject(menuOpciones[10]);
    }

    public void HideControls()
    {
        cargarControls.SetActive(false);
        buttonBack.SetActive(false);
        options.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuOpciones[6]);
    }

}
