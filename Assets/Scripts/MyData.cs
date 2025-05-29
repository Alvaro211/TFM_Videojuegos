using System;
using UnityEngine;
[Serializable]
public class MyData
{
    [Serializable]
    public struct PlayerStats
    {
        public float positiónX;
        public float positiónY;
        public float positiónZ;

        public bool noteGreen;
        public bool noteYellow;
        public bool noteBlue;
        public bool noteRed;
    }
    [SerializeField]
    public PlayerStats player;

    public bool continueGame;

    public float musicVolume;
    public float effectVolume;
    public bool helpControls;

    public bool isOpenDoorGreen;
    public bool isOpenDoorGreenYellow;
    public bool isOpenDoorBoss;
}

