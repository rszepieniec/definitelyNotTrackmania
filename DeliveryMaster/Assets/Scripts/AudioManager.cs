using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Playlist")]
    public AudioClip[] playlist;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("SFX Clips")]
    public AudioClip sfxDeliverySuccess;
    public AudioClip sfxDeliveryFail;
    public AudioClip sfxRunComplete;
    public AudioClip sfxCoins;
    public AudioClip sfxRunFail;
    public AudioClip sfxCrash;
    public AudioClip sfxHonk;

    [Header("SFX")]
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource honkSource;
    private int currentTrackIndex = 0;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = false;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;

        honkSource = gameObject.AddComponent<AudioSource>();
        honkSource.loop = true;
        honkSource.volume = sfxVolume;
        honkSource.playOnAwake = false;

        if (playlist != null && playlist.Length > 1)
            currentTrackIndex = Random.Range(0, playlist.Length);
        PlayCurrentTrack();
    }

    private void Update()
    {
        if (!isPaused && !musicSource.isPlaying && playlist != null && playlist.Length > 0)
        {
            if (playlist.Length > 1)
            {
                int next;
                do { next = Random.Range(0, playlist.Length); } while (next == currentTrackIndex);
                currentTrackIndex = next;
            }
            PlayCurrentTrack();
        }
    }

    private void PlayCurrentTrack()
    {
        if (playlist == null || playlist.Length == 0) return;
        musicSource.clip = playlist[currentTrackIndex];
        musicSource.Play();
    }

    public void PauseMusic()
    {
        isPaused = true;
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        isPaused = false;
        musicSource.UnPause();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlaySFXThenSFX(AudioClip first, AudioClip second)
    {
        if (first == null && second == null) return;
        StartCoroutine(PlaySequence(first, second));
    }

    private IEnumerator PlaySequence(AudioClip first, AudioClip second)
    {
        if (first != null)
        {
            sfxSource.PlayOneShot(first, sfxVolume);
            yield return new WaitForSecondsRealtime(first.length);
        }
        if (second != null)
            sfxSource.PlayOneShot(second, sfxVolume);
    }

    public void StartHonk()
    {
        if (sfxHonk == null || honkSource.isPlaying) return;
        honkSource.clip = sfxHonk;
        honkSource.Play();
    }

    public void StopHonk()
    {
        honkSource.Stop();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        honkSource.volume = sfxVolume;
    }
}
