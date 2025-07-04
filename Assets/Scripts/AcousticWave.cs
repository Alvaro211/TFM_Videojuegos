using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcousticWave : MonoBehaviour
{
    [SerializeField] private float xiaoShi_shiJian = 1.5f;
    [SerializeField] private GameObject wave;


    public void xianShi()
    {
        wave.SetActive(true);
        Invoke(nameof(xiaoShi), xiaoShi_shiJian);
    }

    void xiaoShi()
    {
        wave.SetActive(false);
    }
}