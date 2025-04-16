using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillUpgradable
{
    public int UpgradePoint { get; }
    public int MaxUpgradePoint { get; }

    void Upgrade();
    //void Upgrade(int level);
}

public interface IStatUpgradable
{
    void Upgrade(IStatModifier stat, int level = 0);
    void Upgrade(IStatModifier stat);
}