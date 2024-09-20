using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectMaster : MonoBehaviour
{
    

    public List<AudioSource> Channels = new();
    public List<AudioPlayed> currentAudio = new();
    void Start()
    {

    }
    public AudioSource GetFreeChanel()
    {
        for (int i = 0; i < Channels.Count; i++)
        {
            if (Channels[i].clip == null) return Channels[i];
        }
        return null;
    }
    public AudioSource GetFreeOrAudioChanel(AudioClip audio)
    {
        for (int i = 0; i < Channels.Count; i++)
        {
            if ((Channels[i].clip == null) || (Channels[i].clip == audio)) return Channels[i];
        }
        return null;
    }
    public AudioPlayed GetAudioPlayed(AudioClip clip)
    {
        for (int i = 0; i < currentAudio.Count; i++)
        {
            if (currentAudio[i].currentAS.clip == clip) return currentAudio[i];
        }
        return null;
    }

    public void AudioAwake(AudioClip audio, bool loop, bool replace = false)
    {
        AudioSource freechanel;
        if (!replace)
        {
            freechanel = GetFreeChanel();
        }
        else
        {
            freechanel = GetFreeOrAudioChanel(audio);
        }
        if (freechanel != null)
        {
            freechanel.loop = loop;
            currentAudio.Add(new(audio, true, freechanel, this));
        }
    }
    public void AudioAwakeDefault(AudioClip audio)
    {
        AudioSource freechanel;
       
        
        
            freechanel = GetFreeOrAudioChanel(audio);
        
        if (freechanel != null)
        {
            freechanel.loop = false;
            currentAudio.Add(new(audio, true, freechanel, this));
        }
    }
    public void AudioStop(AudioClip audio)
    {
        AudioPlayed ap = GetAudioPlayed(audio);
        if (ap != null)
        {
            ap.Stop();
        }
    }
    void Update()
    {
        for (int i = 0; i < currentAudio.Count; i++)
        {
            currentAudio[i].Action();
        }
    }
}
public class AudioPlayed
{
    public AudioClip audioClip;
    public bool IsPlay;
    public AudioSource currentAS;
    public AudioEffectMaster nae;
    public AudioPlayed(AudioClip audioClip, bool isPlay, AudioSource currentAS, AudioEffectMaster nae)
    {
        this.nae = nae;
        this.audioClip = audioClip;
        this.IsPlay = isPlay;
        this.currentAS = currentAS;
        currentAS.clip = audioClip;
        if (isPlay)
        {

            currentAS.Play();
        }

    }
    public void Stop()
    {
        currentAS.clip = null;
        nae.currentAudio.Remove(this);
    }
    public void Action()
    {
        IsPlay = currentAS.isPlaying;
        if (!IsPlay)
        {
            Stop();
        }
    }
}
