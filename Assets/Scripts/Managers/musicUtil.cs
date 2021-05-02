using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MusicUtil
{
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
    
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
    
            yield return null;
        }
    
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float endVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < endVolume)
        {
            audioSource.volume += endVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = endVolume;
    }

}
