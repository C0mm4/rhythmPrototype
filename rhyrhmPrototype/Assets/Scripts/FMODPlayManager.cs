using FMOD;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class FMODPlayManager : MonoBehaviour
{
    public static FMODPlayManager Instance { get; private set; }

    [SerializeField] private EventReference musicEvent;
    private EventInstance musicInstance;
    public bool isPlaying = false;

    public Slider slider;
    public float songLength;
    private int currentTime;

    private float defaultOffset = 3; // 카운트다운 지연 시간 (초)
    private double musicStartOffset = 0; // 음악이 실제 시작된 DSP 시간 기준 offset (초 단위)
    private bool hasStarted = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentTime = 0;
        musicInstance.getDescription(out EventDescription description);

        description.getLength(out int lengthMs);
        songLength = lengthMs / 1000f;
    }

    void Update()
    {
        if (isPlaying)
        {
            musicInstance.getTimelinePosition(out currentTime);
            float currLength = currentTime / 1000f;
            slider.value = currLength / songLength;
        }
    }

    private FMOD.Channel channel;
    private int sampleRate;
    private double musicStartDSPTime = 0; // 시스템 기준 시작 시점
    public double MusicStartDSPTime => musicStartDSPTime;

    // 음악 재생 예약 및 DSP 기준 시작 offset 계산
    public void StartMusic()
    {
        StartCoroutine(PlayAfterDelayDSP(defaultOffset));
    }
    private IEnumerator PlayAfterDelayDSP(float delaySeconds)
    {
        RuntimeManager.CoreSystem.getSoftwareFormat(out int sampleRate, out _, out _);
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        musicInstance.start();

        PLAYBACK_STATE state;
        int frameCount = 0;
        do
        {
            musicInstance.getPlaybackState(out state);
            frameCount++;
            yield return null;
        } while (state != PLAYBACK_STATE.PLAYING && frameCount < 60);

        musicInstance.getChannelGroup(out ChannelGroup group);

        // DSP Clock 가져오기
        group.getDSPClock(out ulong dspclock, out ulong parentClock);
        ulong delayDSPClock = parentClock + (ulong)(delaySeconds * sampleRate);

        // 그룹에 지연 재생 설정
        group.setPaused(true);
        var result = group.setDelay(delayDSPClock, 0, true);
        if (result != RESULT.OK)
        {
            UnityEngine.Debug.LogError($"setDelay 실패: {result}");
            yield break;
        }

        group.setPaused(false);
        UnityEngine.Debug.Log($"{delaySeconds}초 후 그룹 예약 재생 완료");
    }

    public void PauseMusic()
    {
        musicInstance.setPaused(true);
        isPlaying = false;
    }

    public void ResumeMusic()
    {
        musicInstance.setPaused(false);
        isPlaying = true;
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }

    public void SetMusicTime(float normalizedTime)
    {
        int millis = (int)(normalizedTime * songLength * 1000f);

        musicInstance.getPlaybackState(out PLAYBACK_STATE state);

        if (state != PLAYBACK_STATE.PLAYING &&
            state != PLAYBACK_STATE.STARTING)
        {
            musicInstance.start();
        }

        musicInstance.setTimelinePosition(millis);

        if (!isPlaying)
        {
            PauseMusic();
        }
    }

    // 현재 재생 중인 음악 시간 (초)
    public float GetCurrentTime()
    {

        musicInstance.getTimelinePosition(out currentTime);

        return currentTime / 1000f;
    }
}


/*


public class FMODPlayManager : MonoBehaviour
{
    public static FMODPlayManager Instance { get; private set; }

    [SerializeField] private EventReference musicEvent;
    private EventInstance musicInstance;
    public bool isPlaying = false;

    public Slider slider;
    public float songLength;
    public int currentTime;

    private float defaultOffset = 3;
    public int offset;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        currentTime = 0;
        musicInstance.getDescription(out EventDescription description);

        description.getLength(out int lengthMs);
        songLength = lengthMs / 1000f;

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            musicInstance.getTimelinePosition(out currentTime);
            float currLength = currentTime / 1000f;
            slider.value = currLength / songLength;
        }
    }

    public void StartMusic()
    {
        StartCoroutine(PlayAfterDelay(defaultOffset));
    }

    private IEnumerator PlayAfterDelay(float delay)
    {
        Debug.Log("Delay Play");
        yield return new WaitForSeconds(delay);
        musicInstance.start();
        isPlaying = true;
        Debug.Log("Delay Play End");
    }

    public void PauseMusic()
    {
        musicInstance.setPaused(true);
        isPlaying = false;
    }

    public void ResumeMusic()
    {
        musicInstance.setPaused(false);
        isPlaying = true;
    }

    private void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }

    public void SetMusicTime(float T)
    {
        int millis = (int)(T * songLength * 1000f);

        // 음악이 아직 시작되지 않았으면 시작시점에 시킹
        musicInstance.getPlaybackState(out PLAYBACK_STATE state);

        if (state != PLAYBACK_STATE.PLAYING &&
            state != PLAYBACK_STATE.STARTING)
        {
            musicInstance.start();
        }

        // 시간 이동은 재생 상태 이후에만 적용 가능
        musicInstance.setTimelinePosition(millis);

        if (!isPlaying)
        {
            PauseMusic();
        }
    }

    public float GetCurrentTime()
    {
        musicInstance.getTimelinePosition(out currentTime);
        
        return currentTime / 1000f;
    }
}
*/