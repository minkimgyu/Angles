using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatModifier
{
    void Visit<T>(T data, int level) where T : PlayerData;
    void Visit<T>(T data) where T : PlayerData;
}