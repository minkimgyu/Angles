using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillUpgradable
{
    public int UpgradePoint { get; }
    public int MaxUpgradePoint { get; }

    bool CanUpgrade();
    void Upgrade();
    void Upgrade(int step);
}

public interface IStatUpgradable
{
    void Upgrade(StatUpgrader.DamageData cooltimeData);
    void Upgrade(StatUpgrader.CooltimeData cooltimeData);

    void Upgrade(StatUpgrader.DashData cooltimeData);
    void Upgrade(StatUpgrader.ShootingData cooltimeData);
}