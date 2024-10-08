using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullSoundPlayer : ISoundPlayable
{
    public void Initialize(Dictionary<ISoundPlayable.SoundName, AudioClip> clipDictionary) { }

    public void PlayBGM(ISoundPlayable.SoundName name) { }
    public void PlaySFX(ISoundPlayable.SoundName name) { }

    public void StopAllSound() { }
    public void StopBGM() { }
}
