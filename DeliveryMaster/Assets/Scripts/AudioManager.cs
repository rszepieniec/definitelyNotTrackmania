using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Playlist")]
    public AudioClip[] playlist;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("SFX")]
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
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

        PlayCurrentTrack();
    }

    private void Update()
    {
        if (!isPaused && !musicSource.isPlaying && playlist != null && playlist.Length > 0)
        {
            currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
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

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
}
