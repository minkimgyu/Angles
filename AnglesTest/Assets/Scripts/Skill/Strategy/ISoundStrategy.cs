using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill.Strategy
{
    public interface ISoundStrategy
    {
        void PlaySound() { }
    }

    public class NoSoundStrategy : ISoundStrategy
    {
    }

    public class PlaySoundStrategy : ISoundStrategy
    {
        ISoundPlayable.SoundName _soundName;
        float _soundVolumn;

        public PlaySoundStrategy(ISoundPlayable.SoundName soundName, float soundVolumn = 1)
        {
            _soundName = soundName;
            _soundVolumn = soundVolumn;
        }

        void PlaySound() 
        {
            ServiceLocater.ReturnSoundPlayer().PlaySFX(_soundName, _soundVolumn);
        }
    }
}