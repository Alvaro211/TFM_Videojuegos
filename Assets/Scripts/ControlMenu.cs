using UnityEngine;
using UnityEngine.SceneManagement;


public class ControlMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public void Play()
    {
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
    }

    public void Exit()
    {
        Application.Quit();
    }

}
