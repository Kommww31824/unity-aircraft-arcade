using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("start");
        Debug.Log("start1");
    }

    // Update is called once per frame
    void Update()
    {
        print("帧间隔时间" + Time.deltaTime);
        print("不受影响的帧间隔时间" + Time.unscaledDeltaTime);
        print("游戏开始到现在的时间" + Time.time);
        print("不受影响的游戏开始到现在的时间" + Time.unscaledTime);
        print("update");
    }
    private void Awake()
    {
        print("Awake");
    }
    private void OnEnable()
    {
        print("OnEnable");
    }
    private void OnDisable()
    {
        print("OnDisable");
    }
    private void FixedUpdate()
    {
        print("FixedUpdate");
        print(Time.fixedDeltaTime);
        print(Time.fixedUnscaledDeltaTime);
    }
    private void LateUpdate()
    {
        print("LateUpdate");
    }
    private void OnDestroy()
    {
        print("OnDestroy");
    }
}
