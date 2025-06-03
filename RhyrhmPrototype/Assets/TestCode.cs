using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class TestCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        notes = new double[10000];
        judges = new int[notes.Length];

        for(int i = 0; i < notes.Length; i++)
        {
            notes[i] = i * 0.5;
            judges[i] = -1;
        }
        currentT = 0;
        noteIndex = 0;

        judgeT = new double[4];
        judgeT[0] = 0.05;
        judgeT[1] = 0.1;
        judgeT[2] = 0.15;
        judgeT[3] = 0.2;
        Application.targetFrameRate = 120; 
        timeOffset = InputState.currentTime - Time.time;
    }

    void Update()
    {
        double now = InputState.currentTime;

        // 판정 유효 시간 초과 시 Miss 처리
        if (noteIndex < notes.Length &&
            judges[noteIndex] == -1 &&
            now > notes[noteIndex] + judgeT[3])
        {
            judges[noteIndex++] = 0;
            Debug.Log($"[{now:F3}s] Miss by timeout");
        }
    }

    public string OnInput(double inputTime)
    {
        if (noteIndex >= notes.Length || judges[noteIndex] != -1)
            return "";

        double noteTime = notes[noteIndex];
        double diff = Math.Abs(noteTime - inputTime);

        if (diff <= judgeT[0])
        {
            judges[noteIndex++] = 4;
            return "Perfect";
        }
        else if (diff <= judgeT[1])
        {
            judges[noteIndex++] = 3;
            return "Great";
        }
        else if (diff <= judgeT[2])
        {
            judges[noteIndex++] = 2;
            return "Good";
        }
        else if (diff <= judgeT[3])
        {
            judges[noteIndex++] = 1;
            return "Miss";
        }

        return "";
    }

    public double[] notes;
    public int[] judges;
    public double currentT;
    public int noteIndex;
    public double[] judgeT;

    public double currentNote;
    double timeOffset;
}
