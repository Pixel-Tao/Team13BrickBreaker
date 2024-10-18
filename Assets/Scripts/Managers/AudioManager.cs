using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX
}

public enum AudioClipType
{
    None,

    bgm1,

    shoot,
    brick_break,
    brick_bounce,
    wall_bounce,
    paddle_bounce,
    gameover,
}

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { Init(); return _instance; } }


    private AudioSource bgm;
    public AudioSource Bgm
    {
        get
        {
            if (bgm == null)
            {
                GameObject go = new GameObject { name = "BGM" };
                bgm = go.AddComponent<AudioSource>();
                bgm.loop = true;
            }
            return bgm;
        }
    }
    private AudioSource sfx;
    public AudioSource Sfx
    {
        get
        {
            if (sfx == null)
            {
                GameObject go = new GameObject { name = "SFX" };
                sfx = go.AddComponent<AudioSource>();
            }
            return sfx;
        }
    }

    private static void Init()
    {
        if (_instance == null)
        {
            // GameManager 동적 생성
            GameObject go = new GameObject { name = "AudioManager" };
            _instance = go.AddComponent<AudioManager>();
            DontDestroyOnLoad(go);
        }
    }

    public void Play(AudioClipType clipType, SoundType soundType, float pitch = 1.0f)
    {
        if (clipType == AudioClipType.None)
            return;

        if (soundType == SoundType.BGM) // BGM 배경음악 재생
        {
            if (Bgm.isPlaying)
                Bgm.Stop();

            AudioClip clip = Resources.Load<AudioClip>($"Sounds/bgm/{clipType}");
            if (clip == null)
            {
                Debug.Log($"bgm AudioClip is null! {clipType}");
                return;
            }

            Bgm.pitch = pitch;
            Bgm.clip = clip;
            Bgm.Play();
        }
        else // Effect 효과음 재생
        {
            AudioClip audioClip = Resources.Load<AudioClip>($"Sounds/sfx/{clipType}");
            if (audioClip == null)
            {
                Debug.Log($"sfx AudioClip is null! {clipType}");
                return;
            }
            Sfx.pitch = pitch;
            Sfx.PlayOneShot(audioClip);
        }
    }

    public void PlayBgm(AudioClipType clipType, float pitch = 1.0f)
    {
        Play(clipType, SoundType.BGM, pitch);
    }

    public void PlaySfx(AudioClipType clipType, float pitch = 1.0f)
    {
        Play(clipType, SoundType.SFX, pitch);
    }

    public void VolumeBgm(float value)
    {
        Bgm.volume = value;
    }

    public void VolumeSfx(float value)
    {
        Sfx.volume = value;
    }

    public void Pause(SoundType type)
    {
        if (type == SoundType.BGM)
            Bgm.Pause();
        else
            Sfx.Pause();
    }

    public void Resume(SoundType type)
    {
        if (type == SoundType.BGM)
            Bgm.UnPause();
        else
            Sfx.UnPause();
    }
}
