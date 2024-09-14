using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    public int UpgradePoint { get; }
    public int MaxUpgradePoint { get; }

    bool CanUpgrade();
    void Upgrade();
}
