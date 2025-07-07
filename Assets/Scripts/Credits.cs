using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public Image backgroundImage;
    public GameObject credtis;

    public Transform canvas;

    public GameObject[] enemy;

    public float floatDuration = 20f; // tiempo de subida
    public float floatDistance = 1000f;  // distancia que sube

    public void ShowFloatingMessage()
    {
        StartCoroutine(AnimateFloatingText());
    }

    private IEnumerator AnimateFloatingText()
    {
        backgroundImage.gameObject.SetActive(true);
        credtis.gameObject.SetActive(true);

        foreach(GameObject enemy in enemy)
        {
            enemy.SetActive(false);
        }

        foreach (Transform hijo in canvas)
        {
            if (hijo.name == "CirculoNota(Clone)")
            {
                hijo.gameObject.SetActive(false);
            }
        }

        GameManager.instance.canMove = false;

        RectTransform rt = credtis.GetComponent<RectTransform>();
        Vector3 startPos = rt.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0, floatDistance, 0);

        float elapsed = 0f;
        while (elapsed < floatDuration)
        {
            rt.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsed / floatDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar posición final
        rt.anchoredPosition = endPos;

        // Opcional: desactivar después de un momento
        yield return new WaitForSeconds(0.5f);

        if(SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(0);
        else
        {
            backgroundImage.gameObject.SetActive(false);
            credtis.gameObject.SetActive(false);

            rt.anchoredPosition = startPos;
        }
    }
}
