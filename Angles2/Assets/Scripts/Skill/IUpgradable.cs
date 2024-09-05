using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    bool CanUpgrade();
    void Upgrade();
    void Upgrade(int step);
}
