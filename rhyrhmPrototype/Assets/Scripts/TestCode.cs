using Palmmedia.ReportGenerator.Core.Reporting.Builders;
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
    public GameObject notePrefab;
    public GameObject flipNotePrefab1;
    public GameObject flipNotePrefab2;
    public GameObject taptapNotePrefab;

    public List<Note> inRangeNotes;

    public Note[] notes;
    public int[] judges;
    public double currentT;


    public void Start()
    {
        Application.targetFrameRate = 60;

        notes = new Note[10000];
        judges = new int[notes.Length];


        for(int i = 0; i < notes.Length; i++)
        {
            GameObject go = Instantiate(flipNotePrefab1);
            notes[i] = go.GetComponent<Note>();
            notes[i].judgeSystem = this;
            judges[i] = -1;
        }

        inRangeNotes = new List<Note>();

        currentT = 0;

        audioSource = GetComponent<AudioSource>();
        startDspTime = AudioSettings.dspTime + offset + 1;
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetData((i + 1) * 0.5, startDspTime + offset, i * (float)(Math.PI / 4), i);
            judges[i] = -1;
        }

        audioSource.PlayScheduled(startDspTime + 1 + soundOffset);
        Debug.Log($"[Init] Audio scheduled at dspTime = {startDspTime:F5}");

    }

    public void Update()
    {
        double now = AudioSettings.dspTime - startDspTime;


    }

    public void OnInput(float dir)
    {
        double now = AudioSettings.dspTime - startDspTime;

        for (int i = 0; i < inRangeNotes.Count; i++)
        {
            int judge = inRangeNotes[i].Judge(now, dir);
            if(judge != -1)
            {
                break;
            }
        }

    }
}
