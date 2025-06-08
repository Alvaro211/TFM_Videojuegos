using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool helpControls = true;
    public bool canMove = true;
    public bool playerMovePlatform = false;
    public bool defeatBoss = false;

    public float musicVol;
    public float effectVol;

    public PlayerMovement playerMovement;
    private MyData sharedData;

    public bool continueGame = false;
    public bool isOpenDoorGreen = false;
    public bool isOpenDoorGreenYellow = false;
    public bool isOpenDoorBoss = false;

    public bool newGame = false;

    private void Awake()
    {
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
            sharedData.player.positiónX = position.x;
            sharedData.player.positiónY = position.y;
            sharedData.player.positiónZ = position.z;

            sharedData.player.noteRed = playerMovement.GetNoteRed();
            sharedData.player.noteBlue = playerMovement.GetNoteBlue();
            sharedData.player.noteGreen = playerMovement.GetNoteGreen();
            sharedData.player.noteYellow = playerMovement.GetNoteYellow();
            sharedData.isOpenDoorGreen = isOpenDoorGreen;
            sharedData.isOpenDoorGreenYellow = isOpenDoorGreenYellow;
            sharedData.isOpenDoorBoss = isOpenDoorBoss;
            sharedData.continueGame = true;
            continueGame = true;
        }else
            sharedData.continueGame = false;

        sharedData.helpControls = helpControls;
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
                playerMovement.SetStartPosition(new Vector3(sharedData.player.positiónX, sharedData.player.positiónY, sharedData.player.positiónZ));
                playerMovement.SetNoteGreen(sharedData.player.noteGreen);
                playerMovement.SetNoteRed(sharedData.player.noteRed);
                playerMovement.SetNoteBlue(sharedData.player.noteBlue);
                playerMovement.SetNoteYellow(sharedData.player.noteYellow);
                isOpenDoorGreen = sharedData.isOpenDoorGreen;
                isOpenDoorGreenYellow = sharedData.isOpenDoorGreenYellow;
                isOpenDoorBoss = sharedData.isOpenDoorBoss;
                continueGame = sharedData.continueGame;
            }

            if(sharedData != null && sharedData.continueGame != null)
            {
                continueGame = sharedData.continueGame;
            }

            helpControls = sharedData.helpControls;
            musicVol = sharedData.musicVolume;
            effectVol = sharedData.effectVolume;

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
        sharedData.player.positiónX = position.x;
        sharedData.player.positiónY = position.y;
        sharedData.player.positiónZ = position.z;

        sharedData.player.noteRed = playerMovement.GetNoteRed();
        sharedData.player.noteBlue = playerMovement.GetNoteBlue();
        sharedData.player.noteGreen = playerMovement.GetNoteGreen();
        sharedData.player.noteYellow = playerMovement.GetNoteYellow();
        sharedData.isOpenDoorGreen = isOpenDoorGreen;
        sharedData.isOpenDoorGreenYellow = isOpenDoorGreenYellow;
        sharedData.isOpenDoorBoss = isOpenDoorBoss;
        sharedData.continueGame = true;
        
        sharedData.helpControls = helpControls;
        sharedData.musicVolume = musicVol;
        sharedData.effectVolume = effectVol;
        bf.Serialize(file, sharedData);
        file.Close();
    }

    public void SaveMusicMenu()
    {
        Load();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +
        "/MyGameData.dat");
        MyData sharedData2 = new MyData();

        if (sharedData != null && sharedData.continueGame && playerMovement != null)
        {
            Vector3 position = playerMovement.GetStartPosition();
            sharedData2.player.positiónX = position.x;
            sharedData2.player.positiónY = position.y;
            sharedData2.player.positiónZ = position.z;

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
            sharedData2.player.positiónX = 0f;
            sharedData2.player.positiónY = 0f;
            sharedData2.player.positiónZ = 0f;

            sharedData2.player.noteRed = false;
            sharedData2.player.noteBlue = false;
            sharedData2.player.noteGreen = false;
            sharedData2.player.noteYellow = false;
            sharedData2.isOpenDoorGreen = false;
            sharedData2.isOpenDoorGreenYellow = false;
            sharedData2.isOpenDoorBoss = false;
            sharedData2.continueGame = false;
        }

        sharedData2.helpControls = helpControls;
        sharedData2.musicVolume = musicVol;
        sharedData2.effectVolume = effectVol;
        bf.Serialize(file, sharedData2);
        file.Close();
    }
}



