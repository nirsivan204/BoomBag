using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class SoundType_And_Ref
    {
        public SoundTypes SoundType;
        public AudioClip AudioClipRef;
    }

    [Serializable]
    public class SoundSource_And_Ref
    {
        public AudioSourceTypes SourceType;
        public AudioSource AudioSourceRef;
    }


    [SerializeField]
    private List<SoundType_And_Ref> SoundType_And_Ref_List = new List<SoundType_And_Ref>();

    [SerializeField]
    private List<SoundSource_And_Ref> SoundSource_And_Ref_List = new List<SoundSource_And_Ref>();


    public enum SoundTypes
    {
        None = 0,

        ______Player_Sounds____ = 1,
        Movement = 2,
        Player_Spawn = 3,
        Player_Death = 4,
        PowerUp = 5,
        Ghost = 6,
        Rigid = 7,
        Jump = 8,
        Dash = 9,
        // UI_Sounds
        Click_01 = 100,

        // Gameplay Sounds
        Bump =200,
        Shrink = 201,
        Grow = 202,
        ColorChange = 203,
        // Music
        BG_Music = 300,
        Countdown = 301,
    }

    public enum AudioSourceTypes
    {
        None,
        UI,
        Player1,
        Player2,
        Player3,
        Player4,
        Gameplay,
        Music,
        Atmosphere,
        GameEvents,
        Player1Movement,
        Player2Movement,
        Player3Movement,
        Player4Movement
        
    }

    internal void Init(GameManager gameManager)
    {
        
    }

    public void Play_Sound(SoundTypes soundType, bool isLoop = false, int player_index = 0)
    {
        AudioClip clip = Get_AudioClip_Of(soundType);

        AudioSource source = Get_AudioSource_Of(soundType, player_index);

        source.loop = isLoop;

        Play_Sound(clip, source);

    }

    private void Play_Sound(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.pitch = 1;

        source.Play();
    }


    private AudioSource Get_AudioSource_Of(SoundTypes soundType ,int player_index)
    {
        switch (soundType)
        {
            case SoundTypes.None:
                break;
            case SoundTypes.Movement:
                return Get_AudioSource_By_Type(AudioSourceTypes.Player1Movement + player_index);
            case SoundTypes.Player_Spawn:
            case SoundTypes.Player_Death:
            case SoundTypes.Ghost:
            case SoundTypes.Rigid:
            case SoundTypes.Jump:
            case SoundTypes.Dash:
                return Get_AudioSource_By_Type(AudioSourceTypes.Player1 + player_index);
            case SoundTypes.Click_01:
                return Get_AudioSource_By_Type(AudioSourceTypes.UI);
            case SoundTypes.Bump:
            case SoundTypes.Shrink:
            case SoundTypes.Grow:
                return Get_AudioSource_By_Type(AudioSourceTypes.Gameplay);
            case SoundTypes.ColorChange:
            case SoundTypes.Countdown:
                return Get_AudioSource_By_Type(AudioSourceTypes.GameEvents);
            case SoundTypes.BG_Music:
                return Get_AudioSource_By_Type(AudioSourceTypes.Music);
            default:
                return Get_AudioSource_By_Type(AudioSourceTypes.Gameplay);
        }

        return null;
    }

    private AudioSource Get_AudioSource_By_Type(AudioSourceTypes audioSourceType)
    {
        for (int i = 0; i < SoundSource_And_Ref_List.Count; i++)
        {
            if (SoundSource_And_Ref_List[i].SourceType == audioSourceType)
            {
                return SoundSource_And_Ref_List[i].AudioSourceRef;
            }
        }

        return null;
    }

    private AudioClip Get_AudioClip_Of(SoundTypes soundType)
    {
        for (int i = 0; i < SoundType_And_Ref_List.Count; i++)
        {
            if (SoundType_And_Ref_List[i].SoundType == soundType)
                return SoundType_And_Ref_List[i].AudioClipRef;
        }

        return null;
    }

}
