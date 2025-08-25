using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicScroll : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Slider slider;
    public FMODPlayManager FMOD;
    public InGame inGame;
    public float value;

    private bool isDragged;
    private bool isPlayMusic;

    public void Start()
    {

    }



    public void OnPointerUp(PointerEventData eventData)
    {
        valueChanged();
        if (isPlayMusic)
        {
            FMOD.ResumeMusic();
        }
        else
        {
            FMOD.PauseMusic();
        }
        isDragged = false;
    }

    public void valueChanged()
    {
        value = slider.value;
        if (isDragged)
        {
            GameManager.Instance.MusicSlider(value);
            inGame.RestorePastNotes();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPlayMusic = FMOD.isPlaying;
        FMOD.PauseMusic();
        isDragged = true;
    }
}
