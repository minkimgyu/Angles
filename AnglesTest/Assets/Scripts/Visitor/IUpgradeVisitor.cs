using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkillUpgrader
{
    // currentUpgrade --> 2
    // maxUpgrade --> 5

    // 실제 적용되는 offset은 2
    const int _upgradeOffset = 2;
    protected int GetUpgradeDataIndex(int currentUpgrade) { return currentUpgrade - _upgradeOffset; }
}

public interface IUpgradeVisitor
{
    void Visit(ISkillUpgradable upgradable, StatikkData data) { }
    void Visit(ISkillUpgradable upgradable, KnockbackData data) { }
    void Visit(ISkillUpgradable upgradable, ImpactData data) { }

    void Visit(ISkillUpgradable upgradable, SpawnShooterData data) { }

    void Visit(ISkillUpgradable upgradable, SpawnBlackholeData blackholeData) { }
    void Visit(ISkillUpgradable upgradable, SpawnBladeData bladeData) { }
    void Visit(ISkillUpgradable upgradable, SpawnStickyBombData stickyBombData) { }

    void Visit(ISkillUpgradable upgradable, SpreadBulletsData data) { }
    void Visit(ISkillUpgradable upgradable, SpreadTrackableMissilesData data) { }
    void Visit(ISkillUpgradable upgradable, ShockwaveData data) { }
    void Visit(ISkillUpgradable upgradable, MagneticFieldData data) { }
    void Visit(ISkillUpgradable upgradable, SelfDestructionData data) { }
}