using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolBolaLuminosa : MonoBehaviour
{
    public GameObject prefab; // Prefab a instanciar
    public int poolSize = 10; // N�mero de prefabs a generar
    private List<GameObject> prefabPool = new List<GameObject>(); // Lista de objetos instanciados

    void Start()
    {
        GeneratePrefabs();
    }

    private void Update()
    {
        if (transform.position.y < -2)
            gameObject.SetActive(false);

        CheckBall();
    }

    void GeneratePrefabs()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false); // Desactivar al inicio
            prefabPool.Add(obj);
        }
    }

    public GameObject GetInactivePrefab()
    {
        foreach (GameObject obj in prefabPool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj; // Devuelve el primero que est?inactivo
            }
        }
        return null; // Si no hay inactivos, devuelve null
    }

    private void CheckBall()
    {
        foreach (GameObject obj in prefabPool)
        {
            if (obj.activeInHierarchy)
            {
                if (obj.transform.position.y < -3)
                    obj.gameObject.SetActive(false);
            }
        }
    }
}
