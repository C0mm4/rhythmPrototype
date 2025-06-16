using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double dspTime;
    double[] judgeTime;
    double startDspTime;
    float scrollSpeed;
    float radianDir;

    public int noteIndex;

    public TestCode judgeSystem;

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

    public void Update()
    {
        double now = AudioSettings.dspTime - startDspTime; 
        float dirX = Mathf.Cos(radianDir);
        float dirY = Mathf.Sin(radianDir);

        if(dspTime - now <= 0.4)
        {
            if(!judgeSystem.inRangeNotes.Contains(this))
                judgeSystem.inRangeNotes.Add(this);
        }

        transform.position = new Vector3((float)(dspTime - now) * scrollSpeed * dirX + 1.5f * dirX, 1, (float)(dspTime - now) * scrollSpeed * dirY + 1.5f * dirY);

        if (judgeSystem.judges[noteIndex] == -1)
        {
            if (isLateMiss(now))
            {
                if (judgeSystem.inRangeNotes.Contains(this))
                {
                    judgeSystem.inRangeNotes.Remove(this);
                    judgeSystem.judges[noteIndex] = 0;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (judgeSystem.inRangeNotes.Contains(this))
        {
            judgeSystem.inRangeNotes.Remove(this);
        }
    }

    public void SetData(double dspTime, double startT, float radianDir, int index)
    {
        this.dspTime = dspTime;
        startDspTime = startT;
        this.radianDir = NormalizeRadian(radianDir);
        float dirX = Mathf.Cos(radianDir);
        float dirY = Mathf.Sin(radianDir);
        noteIndex = index;

        transform.position = new Vector3((float)dspTime * scrollSpeed * dirX + 1.5f * dirX, 1, 
            (float)dspTime * scrollSpeed * dirY + 1.5f * dirY);
        Quaternion yRotation = Quaternion.Euler(0f, -radianDir * Mathf.Rad2Deg, 0f);
        Quaternion xCorrection = Quaternion.Euler(90f, 0f, 0f);
        transform.rotation = yRotation * xCorrection;
    }
    float NormalizeRadian(float rad)
    {
        return (rad % (2 * Mathf.PI) + (2 * Mathf.PI)) % (2 * Mathf.PI);
    }

    bool IsWithinRange(float dir, float radianDir, float range)
    {
        float diff = Mathf.Abs(NormalizeRadian(dir - radianDir));
        return diff <= range || diff >= (2 * Mathf.PI - range);
    }

    public int Judge(double inputDspTime, float dir)
    {
        double diff = inputDspTime - dspTime;

        if (!IsWithinRange(dir, radianDir, Mathf.PI / 4))
        {
            return -1;
        }

        // ~ -200ms
        // out of range
        if(diff <= judgeTime[0])
        {
            return -1;
        }
        // -200ms ~ -150ms
        // Miss
        if(diff <= judgeTime[1])
        {
            judgeSystem.inRangeNotes.Remove(this);
            judgeSystem.judges[noteIndex] = 1;
            Destroy(gameObject);
            return 1;
        }
        // -150ms ~ -100ms
        // good
        if( diff <= judgeTime[2])
        {
            judgeSystem.inRangeNotes.Remove(this);
            judgeSystem.judges[noteIndex] = 2;
            Destroy(gameObject);
            return 2;
        }
        // -100ms ~ -50ms
        // great
        if(diff <= judgeTime[3])
        {
            judgeSystem.inRangeNotes.Remove(this);
            judgeSystem.judges[noteIndex] = 3;
            Destroy(gameObject);
            return 3;
        }
        // -50ms ~ 20ms
        // perfect
        if(diff <= judgeTime[4])
        {
            judgeSystem.inRangeNotes.Remove(this);
            judgeSystem.judges[noteIndex] = 4;
            Destroy(gameObject);
            return 4;
        }
        // 20ms ~ 50ms
        // miss
        if(diff < judgeTime[5])
        {
            judgeSystem.inRangeNotes.Remove(this);
            judgeSystem.judges[noteIndex] = 1;
            Destroy(gameObject);
            return 1;
        }
        // 50ms ~
        // fail
        judgeSystem.inRangeNotes.Remove(this);
        judgeSystem.judges[noteIndex] = 0;
        Destroy(gameObject);
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
