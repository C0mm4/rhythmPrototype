using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public InGame game;
    public AudioClip testSong;
    public FMODPlayManager FMOD;
    private Song song;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);

        }
        Application.targetFrameRate = 60;

        song = new Song();
        song.song = testSong;
        DontDestroyOnLoad(gameObject);
    }

    public void StartSong(Song song)
    {
        FMOD.StartMusic();
        game.PlayGame(song);
    }

    public void StartButton()
    {
        StartSong(song);

    }

    public void PauseButton()
    {
        if(FMOD.isPlaying)
            FMOD.PauseMusic();
        else
            FMOD.ResumeMusic();
    }

    public void MusicSlider(float value)
    {
        FMOD.SetMusicTime(value);
    }
}
