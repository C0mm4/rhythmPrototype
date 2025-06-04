using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class TestCode : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audioSource;
    private double startDspTime;
    private double offset = 0f;
    private double soundOffset = -0.6f;
    Lane[] lanes;
    float[] x = { 1, 0, -1, 0 };
    float[] z = { 0, 1, 0, -1 };

    public void Start()
    {
        Application.targetFrameRate = 60;

        notes = new Note[10000];
        judges = new int[notes.Length];
        lanes = new Lane[4];

        for(int i = 0; i < lanes.Length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
            lanes[i] = go.AddComponent<Lane>();
            lanes[i].dir = new Vector2(x[i % 4], z[i % 4]);
        }

        for(int i = 0; i < notes.Length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.localScale = Vector3.one * 0.3f;
            notes[i] = go.AddComponent<Note>();
            judges[i] = -1;
        }

        currentT = 0;
        noteIndex = 0;

        audioSource = GetComponent<AudioSource>();
        startDspTime = AudioSettings.dspTime + offset + 1;
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetData((i + 1) * 0.5, startDspTime + offset, lanes[i % 4]);
            judges[i] = -1;
        }

        audioSource.PlayScheduled(startDspTime + 1 + soundOffset);
        Debug.Log($"[Init] Audio scheduled at dspTime = {startDspTime:F5}");

        currentNote = notes[0];
    }

    public void Update()
    {
        double now = AudioSettings.dspTime - startDspTime;

        while (noteIndex < notes.Length &&
               currentNote.isLateMiss(now)) // 가장 넓은 판정 시간 넘어감
        {
            if (judges[noteIndex] == -1)
            {
                Debug.Log($"Missed Note {noteIndex} {now}");
                judges[noteIndex] = 0;
                Destroy(currentNote.gameObject);
            }
            currentNote = notes[noteIndex++];
        }

    }

    public void OnInput()
    {
        double now = AudioSettings.dspTime - startDspTime;
        if (judges[noteIndex] == -1)
        {
            int judge = currentNote.Judge(now);
            if (judge != -1)
            {
                judges[noteIndex] = judge;
                Debug.Log("Time : " + now);
            }
        }
    }

    public Note[] notes;
    public int[] judges;
    public double currentT;
    public int noteIndex;

    public Note currentNote;
    double timeOffset;
}
