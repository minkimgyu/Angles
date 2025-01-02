using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] AudioSource _source;

    public void Play(AudioClip clip, Vector3 pos, float volumn = 1)
    {
        _source.clip = clip;
        transform.position = pos;

        _source.volume = volumn;
        _source.Play();
    }
}
