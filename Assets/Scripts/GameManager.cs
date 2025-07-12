using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Language
    {
        Spanish,
        Valencian,
        English,
        Chino
    }

    public static GameManager instance = null;

    public Language idiom = Language.English;

    public Vibration vibration;

    public bool helpControls = true;
    public bool canMove = true;
    public bool playerMovePlatform = false;
    public bool defeatBoss = false;

    public float musicVol;
    public float effectVol;

    public PlayerMovement playerMovement;
    public MyData sharedData;

    public bool continueGame = false;
    public bool isOpenDoorGreen = false;
    public bool isOpenDoorGreenYellow = false;
    public bool isOpenDoorBoss = false;


    public bool isSeenCinematic2 = false;
    public bool isSeenCinematic3 = false;
    public bool isSeenCinematic4 = false;

    public bool isTakeColeccionable1 = false;
    public bool isTakeColeccionable2 = false;
    public bool isTakeColeccionable3 = false;

    public bool newGame = true;
    public bool onAnimation = false;

    public GameObject cargar;

    private void Awake()
    {
        idiom = Language.English;

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
   
    public void ChangeHelpControls()
    {
        helpControls = !helpControls;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +
        "/MyGameData.dat");
        sharedData = new MyData();
        if (playerMovement != null) { 
            Vector3 position = playerMovement.GetStartPosition();
            sharedData.player.positionX = position.x;
            sharedData.player.positionY = position.y;
            sharedData.player.positionZ = position.z;

            sharedData.player.noteRed = playerMovement.GetNoteRed();
            sharedData.player.noteBlue = playerMovement.GetNoteBlue();
            sharedData.player.noteGreen = playerMovement.GetNoteGreen();
            sharedData.player.noteYellow = playerMovement.GetNoteYellow();
            sharedData.isOpenDoorGreen = isOpenDoorGreen;
            sharedData.isOpenDoorGreenYellow = isOpenDoorGreenYellow;
            sharedData.isOpenDoorBoss = isOpenDoorBoss;

            sharedData.isSeenCinematic2 = isSeenCinematic2;
            sharedData.isSeenCinematic3 = isSeenCinematic3;
            sharedData.isSeenCinematic4 = isSeenCinematic4;
            sharedData.isTakeColeccionable1 = isTakeColeccionable1;
            sharedData.isTakeColeccionable2 = isTakeColeccionable2;
            sharedData.isTakeColeccionable3 = isTakeColeccionable3;

            sharedData.continueGame = true;
            continueGame = true;
        }else
            sharedData.continueGame = false;

        sharedData.idiom = idiom;

        //sharedData.helpControls = helpControls;
        sharedData.musicVolume = musicVol;
        sharedData.effectVolume = effectVol;
        bf.Serialize(file, sharedData);
        file.Close();
    }
    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/MyGameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath +
            "/MyGameData.dat", FileMode.Open);
            sharedData = bf.Deserialize(fs) as MyData;
            fs.Close();

            if (playerMovement != null)
            {
                playerMovement.SetStartPosition(new Vector3(sharedData.player.positionX, sharedData.player.positionY, sharedData.player.positionZ));
                playerMovement.SetNoteGreen(sharedData.player.noteGreen);
                playerMovement.SetNoteRed(sharedData.player.noteRed);
                playerMovement.SetNoteBlue(sharedData.player.noteBlue);
                playerMovement.SetNoteYellow(sharedData.player.noteYellow);
            }

            if(sharedData != null && sharedData.continueGame != null)
            {
                continueGame = sharedData.continueGame;
            }

            idiom = sharedData.idiom;

            isOpenDoorGreen = sharedData.isOpenDoorGreen;
            isOpenDoorGreenYellow = sharedData.isOpenDoorGreenYellow;
            isOpenDoorBoss = sharedData.isOpenDoorBoss;
            continueGame = sharedData.continueGame;

           // helpControls = sharedData.helpControls;
            musicVol = sharedData.musicVolume;
            effectVol = sharedData.effectVolume;

            isSeenCinematic2 = sharedData.isSeenCinematic2;
            isSeenCinematic3 = sharedData.isSeenCinematic3;
            isSeenCinematic4 = sharedData.isSeenCinematic4;
            isTakeColeccionable1 = sharedData.isTakeColeccionable1;
            isTakeColeccionable2 = sharedData.isTakeColeccionable2;
            isTakeColeccionable3 = sharedData.isTakeColeccionable3;

            return true;
        }

        return false;
    }

    public void SaveMusic()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +
        "/MyGameData.dat");
        sharedData = new MyData();
        
        Vector3 position = playerMovement.GetStartPosition();
        sharedData.player.positionX = position.x;
        sharedData.player.positionY = position.y;
        sharedData.player.positionZ = position.z;

        sharedData.player.noteRed = playerMovement.GetNoteRed();
        sharedData.player.noteBlue = playerMovement.GetNoteBlue();
        sharedData.player.noteGreen = playerMovement.GetNoteGreen();
        sharedData.player.noteYellow = playerMovement.GetNoteYellow();
        sharedData.isOpenDoorGreen = isOpenDoorGreen;
        sharedData.isOpenDoorGreenYellow = isOpenDoorGreenYellow;
        sharedData.isOpenDoorBoss = isOpenDoorBoss;

        sharedData.isSeenCinematic2 = isSeenCinematic2;
        sharedData.isSeenCinematic3 = isSeenCinematic3;
        sharedData.isSeenCinematic4 = isSeenCinematic4;
        sharedData.isTakeColeccionable1 = isTakeColeccionable1;
        sharedData.isTakeColeccionable2 = isTakeColeccionable2;
        sharedData.isTakeColeccionable3 = isTakeColeccionable3;

        sharedData.continueGame = true;

        sharedData.idiom = idiom;

        // sharedData.helpControls = helpControls;
        sharedData.musicVolume = musicVol;
        sharedData.effectVolume = effectVol;
        bf.Serialize(file, sharedData);
        file.Close();
    }

    public void SaveMusicMenu()
    {
        //Load();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +
        "/MyGameData.dat");
        MyData sharedData2 = new MyData();

        /*if (sharedData != null && sharedData.continueGame && playerMovement != null)
        {
            Vector3 position = playerMovement.GetStartPosition();
            sharedData2.player.positionX = position.x;
            sharedData2.player.positionY = position.y;
            sharedData2.player.positionZ = position.z;

            sharedData2.player.noteRed = playerMovement.GetNoteRed();
            sharedData2.player.noteBlue = playerMovement.GetNoteBlue();
            sharedData2.player.noteGreen = playerMovement.GetNoteGreen();
            sharedData2.player.noteYellow = playerMovement.GetNoteYellow();
            sharedData.isOpenDoorGreen = isOpenDoorGreen;
            sharedData.isOpenDoorGreenYellow = isOpenDoorGreenYellow;
            sharedData.isOpenDoorBoss = isOpenDoorBoss;

            sharedData2.continueGame = true;
        }
        else
        {
            sharedData2.player.positionX = 0f;
            sharedData2.player.positionY = 0f;
            sharedData2.player.positionZ = 0f;

            sharedData2.player.noteRed = false;
            sharedData2.player.noteBlue = false;
            sharedData2.player.noteGreen = false;
            sharedData2.player.noteYellow = false;
            sharedData2.isOpenDoorGreen = false;
            sharedData2.isOpenDoorGreenYellow = false;
            sharedData2.isOpenDoorBoss = false;
            sharedData2.continueGame = false;
        }*/

        sharedData2.idiom = idiom;

        sharedData2.musicVolume = musicVol;
        sharedData2.effectVolume = effectVol;
        bf.Serialize(file, sharedData2);
        file.Close();
    }

    public void DisableCargar()
    {
        if(cargar != null)
            cargar.SetActive(false);
    }

    public void EnableCargar()
    {
        if (cargar != null)
            cargar.SetActive(true);
    }

    public IEnumerator LoadSceneAsync(string scene)
    {
        Time.timeScale = 1;
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
        yield return new WaitForSeconds(2f);

        // Activar la escena cargada
        asyncLoad.allowSceneActivation = true;
    }
}



