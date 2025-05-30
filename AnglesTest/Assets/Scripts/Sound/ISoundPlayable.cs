using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundPlayable
{
    public enum SoundName
    {
        Die,
        Dash,
        Bounce,
        EnemyDie,
        GetCoin,
        GetHeart,
        Shooting,

        SpreadBullets,
        Shockwave,
        ShooterFire,

        Upgrade,
        Click,

        GameClear,
        GameOver,
        StageClear,
        Hit,

        Explosion,

        Blade,
        Impact,
        Knockback,
        Statikk,
        Blackhole,

        LobbyBGM,

        BlueBGM,
        BlueBossBGM,

        RedBGM,
        RedBossBGM,

        GreenBGM,
        GreenBossBGM,

        WhiteBGM,
        WhiteBossBGM,

        OrangeBGM,
        OrangeBossBGM,

        DeepRedBGM,
        DeepRedBossBGM,

        CyanBGM,
        CyanBossBGM,

        Reroll,
        LevelClear,
        LevelFail,
    }

    void Initialize(Dictionary<SoundName, AudioClip> clipDictionary);

    void MuteBGM(bool nowMute);
    void MuteSFX(bool nowMute);

    bool GetBGMMute();
    bool GetSFXMute();

    void PlayBGM(SoundName name, float volumn = 1);
    void PlaySFX(SoundName name, float volumn = 1);
    void PlaySFX(SoundName name, Vector3 pos, float volumn = 1);

    void StopBGM();
    void StopAllSound();
}
