using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ControlMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;

    public SoundManager audio;

    public Image imageContinue;

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
    }
    public void Play()
    {
        if(GameManager.instance.continueGame)
            SceneManager.LoadScene(1);
    }

    public void PlayNewGame()
    {
        GameManager.instance.continueGame = false;
        SceneManager.LoadScene(1);
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

    public void Exit()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
