using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcousticWave: MonoBehaviour
{
    [SerializeField] private float xiaoShi_shiJian = 1.5f;
    private void OnEnable()
    {
        Invoke("xiaoShi", xiaoShi_shiJian);
    }

    void xiaoShi()
    {
        gameObject.SetActive(false);
    }
}
