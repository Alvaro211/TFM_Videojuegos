using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ControlMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public GameObject cargar;

    public SoundManager audio;

    public Image imageContinue;

    public Transform canvas;

    public void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 0)
        {
            audio.AudioMenu();
            if (!GameManager.instance.continueGame)
                imageContinue.color = new Color(0.33f, 0.33f, 0.33f);
            else
                imageContinue.color = new Color(1f, 1f, 1f);
        }
        else if (index == 1)
            audio.AudioPlay();

        if(cargar != null)
            DontDestroyOnLoad(cargar);
    }
    public void Play()
    {
        if (GameManager.instance.continueGame)
        {
            cargar.SetActive(true);
            GameManager.instance.newGame = false;
            //SceneManager.LoadScene(1);
            StartCoroutine(LoadSceneAsync("Level"));
        }
    }

    public void PlayNewGame()
    {
        cargar.SetActive(true);
        GameManager.instance.newGame = true;
        //SceneManager.LoadScene(1);
        StartCoroutine(LoadSceneAsync("Level"));
    }

    IEnumerator LoadSceneAsync(string scene)
    {

        yield return new WaitForSeconds(1f);
        // Empieza la carga en segundo plano
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Opcional: evitar que la escena se active automáticamente
        asyncLoad.allowSceneActivation = false;

        // Esperar hasta que la escena esté casi completamente cargada
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("Progreso: " + asyncLoad.progress);
            yield return null;
        }

        Debug.Log("Escena cargada al 90%, lista para activarse");

        // Espera un poco, puedes mostrar una animación aquí si quieres
        yield return new WaitForSeconds(1f);

        // Activar la escena cargada
        asyncLoad.allowSceneActivation = true;
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

    public void Exit()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        GameManager.instance.DisableCargar();
        SceneManager.LoadScene(0);
    }

}
