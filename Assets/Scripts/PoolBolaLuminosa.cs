using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBolaLuminosa : MonoBehaviour
{
    public GameObject prefab; // Prefab a instanciar
    public int poolSize = 10; // Número de prefabs a generar
    private List<GameObject> prefabPool = new List<GameObject>(); // Lista de objetos instanciados

    void Start()
    {
        GeneratePrefabs();
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
                return obj; // Devuelve el primero que esté inactivo
            }
        }
        return null; // Si no hay inactivos, devuelve null
    }
}
