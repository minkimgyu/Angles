using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    public bool CanUpgrade();
    public void Upgrade();
    public void Upgrade(int step);
}
