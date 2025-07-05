using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public Image backgroundImage;
    public GameObject credtis;

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

        SceneManager.LoadScene(0);
    }
}
