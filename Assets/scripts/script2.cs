using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script2 : MonoBehaviour
{
    // 计时器当前时间
    private float timer = 0f;
    // 计时器是否正在运行
    private bool isRunning = false;

    // 绑定UI文本用于显示计时器（需在Inspector面板中赋值）
    public Text timerText;

    // 初始化计时器
    void Start()
    {
        timer = 0f;
        isRunning = false;
        Debug.Log("计时器已初始化");

        if (timerText != null)
        {
            timerText.text = "0.00s";
        }
    }

    // 更新计时器显示及逻辑
    void Update()
    {
        if (isRunning)
        {
            // 累加时间
            timer += Time.deltaTime;

            // 更新UI显示
            if (timerText != null)
            {
                timerText.text = timer.ToString("F2") + "s";
            }

            // 打印调试更新计时器信息（为避免控制台刷屏，可按需取消注释）
            // Debug.Log("当前计时: " + timer.ToString("F2") + "s");
        }
    }

    // 启动计时器方法
    public void StartTimer()
    {
        isRunning = true;
        Debug.Log("计时器已启动");
    }

    // 停止计时器方法
    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("计时器已停止，最终时间: " + timer.ToString("F2") + "s");
    }

    // 清理资源
    void OnDestroy()
    {
        StopTimer();
        Debug.Log("计时器资源已清理");
    }
}