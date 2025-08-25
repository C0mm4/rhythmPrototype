using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InGame : MonoBehaviour
{
    // Start is called before the first frame update
    private double offset = 0f;
    private double soundOffset = -0.6f;
    public GameObject notePrefab;
    public GameObject flipNotePrefab1;
    public GameObject flipNotePrefab2;
    public GameObject taptapNotePrefab;

    public HashSet<Note> inRangeNotes;

    public Note[] notes;
    public int[] judges;

    public FMODPlayManager FMOD;


    public void Start()
    {
        inRangeNotes = new HashSet<Note>(); 
    }

    public void Update()
    {

    }

    public void OnInput(float dir, Key keyCode)
    {
        float now = FMOD.GetCurrentTime();

        for (int i = 0; i < inRangeNotes.Count; i++)
        {
            int judge = inRangeNotes.ElementAt(i).Judge(now, dir, keyCode);
            if(judge != -1)
            {
                break;
            }
        }
    }

    public void PlayGame(Song song)
    {
        Initialize();
    }

    private void Initialize()
    {

        notes = new Note[10000];
        judges = new int[notes.Length];


        for (int i = 0; i < notes.Length; i++)
        {
            GameObject go = Instantiate(flipNotePrefab1);
            notes[i] = go.GetComponent<Note>();
            notes[i].judgeSystem = this;
            judges[i] = -1;
        }

        inRangeNotes = new HashSet<Note>();

        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].SetData((i + 1) * 0.5, offset, i * (float)(Math.PI / 4), i);
            judges[i] = -1;
        }

    }

    public void RestorePastNotes()
    {
        double now = FMOD.GetCurrentTime();
        for (int i = 0; i < notes.Length; i++)
        {
            bool shouldBeActive = notes[i].dspTime >= now;
            notes[i].gameObject.SetActive(shouldBeActive);
            judges[notes[i].noteIndex] = shouldBeActive ? -1 : judges[notes[i].noteIndex];

        }
    }
}
