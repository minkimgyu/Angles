using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettable
{
    void Activate(bool on);
    void Initialize();
}

public class NULLSettingController : ISettable
{
    public void Activate(bool on) { }
    public void Initialize() { }
}