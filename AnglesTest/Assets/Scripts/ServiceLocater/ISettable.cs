using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettable
{
    void ChangeLanguage();
    void Activate(bool on);
    void Initialize();
}

public class NullSettingController : ISettable
{
    public void ChangeLanguage() { }
    public void Activate(bool on) { }
    public void Initialize() { }
}