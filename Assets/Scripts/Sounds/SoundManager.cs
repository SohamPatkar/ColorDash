using UnityEngine;
using System;

public enum SoundType
{
    Collected,
    Death,
    BackgroundMusic,
}

[Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    [SerializeField]
    private AudioSource sfxAudio;
    [SerializeField]
    private AudioSource musicAudio;
    [SerializeField]
    private Sound[] audios;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackGroundMusic(SoundType.BackgroundMusic);
    }

    public void PlayBackGroundMusic(SoundType audioType)
    {
        AudioClip audioClipBackground = GetClip(audioType);
        musicAudio.clip = audioClipBackground;
        musicAudio.Play();
    }

    AudioClip GetClip(SoundType audioType)
    {
        Sound sound = Array.Find(audios, item => item.type == audioType);
        if (sound != null)
        {
            return sound.clip;
        }
        else
        {
            return null;
        }
    }

    public void PlaySfxSound(SoundType soundType)
    {
        AudioClip audioClipBackground = GetClip(soundType);
        sfxAudio.PlayOneShot(audioClipBackground);
    }
}