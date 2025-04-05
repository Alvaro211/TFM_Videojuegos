using UnityEngine;

// This script is used to control the platform's rise logic
public class PlatformRaiser : MonoBehaviour
{
 
    private bool[] keysPressed = new bool[4];
   
    private int keysPressedCount = 0;

    // Speed of platform ascent
    public float raiseSpeed = 2f;
    // Target height of platform ascent
    public float raiseHeight = 5f;
    // Target Y-coordinate of platform ascent
    private float targetY;

 
    private void Start()
    {
    
        targetY = transform.position.y + raiseHeight;
    }

    
    private void Update()
    {
        // Checks to see if the player has collected at least 4 balls.
        if (CollectibleBall.collectedCount >= 4)
        {
           
            CheckKeyPresses();
           
            if (keysPressedCount == 4)
            {
                // Call the platform up method
                RaisePlatform();
            }
        }
    }

    // 检查数字键 1、2、3、4 的按下情况
    private void CheckKeyPresses()
    {
        // 检查数字键 1 是否被按下，且之前未被按下过
        if (Input.GetKeyDown(KeyCode.Alpha1) && !keysPressed[0])
        {
            // 标记数字键 1 已被按下
            keysPressed[0] = true;
            // 增加已按下的数字键数量
            keysPressedCount++;
        }
        // 检查数字键 2 是否被按下，且之前未被按下过
        if (Input.GetKeyDown(KeyCode.Alpha2) && !keysPressed[1])
        {
            // 标记数字键 2 已被按下
            keysPressed[1] = true;
            // 增加已按下的数字键数量
            keysPressedCount++;
        }
        // 检查数字键 3 是否被按下，且之前未被按下过
        if (Input.GetKeyDown(KeyCode.Alpha3) && !keysPressed[2])
        {
            // 标记数字键 3 已被按下
            keysPressed[2] = true;
            // 增加已按下的数字键数量
            keysPressedCount++;
        }
        // 检查数字键 4 是否被按下，且之前未被按下过
        if (Input.GetKeyDown(KeyCode.Alpha4) && !keysPressed[3])
        {
            // 标记数字键 4 已被按下
            keysPressed[3] = true;
            // 增加已按下的数字键数量
            keysPressedCount++;
        }
    }

    // 平台上升的方法
    private void RaisePlatform()
    {
        // 检查平台当前的 Y 坐标是否小于目标 Y 坐标
        if (transform.position.y < targetY)
        {
            // 若小于目标 Y 坐标，则向上移动平台
            transform.Translate(Vector3.up * raiseSpeed * Time.deltaTime);
        }
    }
}