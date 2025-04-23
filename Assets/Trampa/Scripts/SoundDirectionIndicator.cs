using UnityEngine;
using UnityEngine.UI;

public class SoundDirectionIndicator : MonoBehaviour
{
    public Transform playerCamera; // ����������������Ļ����ת����
    public Image halo; // �⻷UI
    public Image arrow; // ��ͷUI
    public float minDistance = 2f; // �����������С����
    public float arrowSize = 0.2f; // ��ͷ����Ļ�ϵĴ�С

    private AudioSource[] soundSources; // �����е�������ƵԴ
    private Vector3 soundDirection; // ������Դ��������

    void Start()
    {
        // ���س�ʼ״̬
        halo.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);
        // ��ȡ������������ƵԴ�����Ż�Ϊ�¼�������⣩
        soundSources = FindObjectsOfType<AudioSource>();
    }

    void Update()
    {
        // �������ļ�����ƵԴ
        AudioSource nearestSource = GetNearestActiveSource();
        if (nearestSource != null)
        {
            // �����������򣨴���ҵ���Դ��
            soundDirection = nearestSource.transform.position - transform.position;
            soundDirection.y = 0; // ����Y�ᣨ�����ˮƽ����

            if (soundDirection.magnitude > minDistance)
            {
                // ��ʾ�⻷�ͼ�ͷ
                halo.gameObject.SetActive(true);
                arrow.gameObject.SetActive(true);
                // ���¼�ͷλ�ú���ת
                UpdateArrowPosition();
            }
            else
            {
                // �������ʱ���ؼ�ͷ
                arrow.gameObject.SetActive(false);
            }
        }
        else
        {
            // ����Դʱ��������UI
            halo.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
        }
    }

    // ��ȡ����ļ�����ƵԴ
    private AudioSource GetNearestActiveSource()
    {
        AudioSource nearest = null;
        float minDist = Mathf.Infinity;
        foreach (AudioSource source in soundSources)
        {
            if (source.isPlaying && source != GetComponent<AudioSource>())
            {
                float dist = Vector3.Distance(transform.position, source.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = source;
                }
            }
        }
        return nearest;
    }

    // ���¼�ͷ����Ļ�ϵ�λ��
    private void UpdateArrowPosition()
    {
        // ��3D��������ת��Ϊ��Ļ����
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + soundDirection);
        screenPos = new Vector3(screenPos.x / Screen.width * 2 - 1, screenPos.y / Screen.height * 2 - 1, 0); // ת��ΪUI����ϵ��-1��1��

        // ���Ƽ�ͷ����Ļ��
        screenPos.x = Mathf.Clamp(screenPos.x, -1 + arrowSize, 1 - arrowSize);
        screenPos.y = Mathf.Clamp(screenPos.y, -1 + arrowSize, 1 - arrowSize);

        // ���ü�ͷλ�ú���ת
        arrow.rectTransform.anchoredPosition = screenPos * new Vector2(Screen.width, Screen.height) * 0.5f; // ת��Ϊ��������
        arrow.rectTransform.sizeDelta = new Vector2(arrowSize * Screen.width, arrowSize * Screen.height); // ��̬������ͷ��С
        float angle = Mathf.Atan2(soundDirection.x, soundDirection.z) * Mathf.Rad2Deg; // ������ת�Ƕȣ�XZƽ�棩
        arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90); // ������ͷ���򣨸���ͼƬ����������
    }
}