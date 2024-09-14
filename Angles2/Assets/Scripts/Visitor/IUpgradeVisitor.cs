using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeVisitor
{
    void Visit(IUpgradable upgradable, StatikkData data) { }
    void Visit(IUpgradable upgradable, KnockbackData data) { }
    void Visit(IUpgradable upgradable, ImpactData data) { }
    //void Visit(IUpgradable upgradable, ContactAttackData data) { }


    void Visit(IUpgradable upgradable, ShooterData shooterData, BulletData data) { }
    void Visit(IUpgradable upgradable, ShooterData shooterData, RocketData data) { }
    // ShooterData를 업그레이드 할 수 있게 해주기

    void Visit(IUpgradable upgradable, BlackholeData blackholeData) { }
    void Visit(IUpgradable upgradable, BladeData bladeData) { }
    void Visit(IUpgradable upgradable, StickyBombData stickyBombData) { }

    void Visit(IUpgradable upgradable, ShooterData shooterData) { }

    void Visit(SpreadBulletsData data) { }
    void Visit(ShockwaveData data) { }
    void Visit(MagneticFieldData data) { }
    void Visit(IUpgradable upgradable, SelfDestructionData data) { }
}