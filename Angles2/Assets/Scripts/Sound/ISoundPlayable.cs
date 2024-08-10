using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayable
{
    public enum SoundName
    {
        Shoot,
        Dash,
        GameClear,
        GameOver,
        StageClear,
        Hit,
        Impact,
        Explosion,
        Get,



        Die
    }

    //void Initialize(Dictionary<SoundName, AudioClip> clipDictionary);
    void PlayBGM(SoundName name);
    void PlaySFX(SoundName name);

    void StopBGM();
    void StopAllSound();
}
