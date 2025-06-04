using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double dspTime;
    double[] judgeTime;
    double startDspTime;
    float scrollSpeed;
    Lane lane;

    private void Start()
    {
        judgeTime = new double[6];
        judgeTime[0] = -0.2;
        judgeTime[1] = -0.15;
        judgeTime[2] = -0.1;
        judgeTime[3] = -0.05;
        judgeTime[4] = 0.02;
        judgeTime[5] = 0.05;

        scrollSpeed = 10;
    }

    private void Update()
    {
        double now = AudioSettings.dspTime - startDspTime;
        transform.position = new Vector3((float)(dspTime - now) * scrollSpeed * lane.dir.x + 1.5f * lane.dir.x, 1, 
            (float)(dspTime - now) * scrollSpeed * lane.dir.y + 1.5f * lane.dir.y);
    }

    public void SetData(double dspTime, double startT, Lane lane)
    {
        this.dspTime = dspTime;
        startDspTime = startT;
        this.lane = lane;
        transform.position = new Vector3((float)dspTime * scrollSpeed * lane.dir.x + 1.5f * lane.dir.x, 1, 
            (float)dspTime * scrollSpeed * lane.dir.y + 1.5f * lane.dir.y);
    }

    public int Judge(double inputDspTime)
    {
        double diff = inputDspTime - dspTime;

        // ~ -200ms
        // Fail
        if(diff <= judgeTime[0])
        {
            return -1;
        }
        // -200ms ~ -150ms
        // Miss
        if(diff <= judgeTime[1])
        {
            Destroy(gameObject);
            return 1;
        }
        // -150ms ~ -100ms
        // good
        if( diff <= judgeTime[2])
        {
            Destroy(gameObject);
            return 2;
        }
        // -100ms ~ -50ms
        // great
        if(diff <= judgeTime[3])
        {
            Destroy(gameObject);
            return 3;
        }
        // -50ms ~ 20ms
        // perfect
        if(diff <= judgeTime[4])
        {
            Destroy(gameObject);
            return 4;
        }
        // 20ms ~ 50ms
        // miss
        if(diff < judgeTime[5])
        {
            Destroy(gameObject);
            return 1;
        }
        // 50ms ~
        // fail
        return 0;
    }

    public bool isLateMiss(double currentTime)
    {
        if(currentTime >= dspTime + judgeTime[5])
        {
            return true;
        }
        return false;
    }

}
