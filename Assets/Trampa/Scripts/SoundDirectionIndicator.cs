using UnityEngine;
using UnityEngine.UI;

public class SoundDirectionIndicator : MonoBehaviour
{
    public Transform playerCamera; // 玩家摄像机（用于屏幕坐标转换）
    public Image halo; // 光环UI
    public Image arrow; // 箭头UI
    public float minDistance = 2f; // 检测声音的最小距离
    public float arrowSize = 0.2f; // 箭头在屏幕上的大小

    private AudioSource[] soundSources; // 场景中的所有音频源
    private Vector3 soundDirection; // 声音来源方向向量

    void Start()
    {
        // 隐藏初始状态
        halo.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);
        // 获取场景中所有音频源（可优化为事件触发检测）
        soundSources = FindObjectsOfType<AudioSource>();
    }

    void Update()
    {
        // 检测最近的激活音频源
        AudioSource nearestSource = GetNearestActiveSource();
        if (nearestSource != null)
        {
            // 计算声音方向（从玩家到声源）
            soundDirection = nearestSource.transform.position - transform.position;
            soundDirection.y = 0; // 忽略Y轴（仅检测水平方向）

            if (soundDirection.magnitude > minDistance)
            {
                // 显示光环和箭头
                halo.gameObject.SetActive(true);
                arrow.gameObject.SetActive(true);
                // 更新箭头位置和旋转
                UpdateArrowPosition();
            }
            else
            {
                // 距离过近时隐藏箭头
                arrow.gameObject.SetActive(false);
            }
        }
        else
        {
            // 无声源时隐藏所有UI
            halo.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
        }
    }

    // 获取最近的激活音频源
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

    // 更新箭头在屏幕上的位置
    private void UpdateArrowPosition()
    {
        // 将3D方向向量转换为屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + soundDirection);
        screenPos = new Vector3(screenPos.x / Screen.width * 2 - 1, screenPos.y / Screen.height * 2 - 1, 0); // 转换为UI坐标系（-1到1）

        // 限制箭头在屏幕内
        screenPos.x = Mathf.Clamp(screenPos.x, -1 + arrowSize, 1 - arrowSize);
        screenPos.y = Mathf.Clamp(screenPos.y, -1 + arrowSize, 1 - arrowSize);

        // 设置箭头位置和旋转
        arrow.rectTransform.anchoredPosition = screenPos * new Vector2(Screen.width, Screen.height) * 0.5f; // 转换为像素坐标
        arrow.rectTransform.sizeDelta = new Vector2(arrowSize * Screen.width, arrowSize * Screen.height); // 动态调整箭头大小
        float angle = Mathf.Atan2(soundDirection.x, soundDirection.z) * Mathf.Rad2Deg; // 计算旋转角度（XZ平面）
        arrow.rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90); // 调整箭头朝向（根据图片朝向修正）
    }
}