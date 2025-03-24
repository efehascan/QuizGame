using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    float time = 30;


    private void Update()
    {
        Timer();
        
    }

    private void Timer()
    {
        while (time >= 0)
        {
            time -= Time.deltaTime;
            Debug.Log(time);
        }
    }
}
