using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBuff : BaseBuff
{
    List<ShootingBuffUpgradeable> _upgradeableDatas;
    ShootingBuffUpgradeable UpgradeableData { get { return _upgradeableDatas[UpgradePoint]; } }

    public ShootingBuff(ShootingBuffData data) : base() 
    {
        _upgradeableDatas = data._shootingBuffUpgradeableDatas;
    }

    public override void Initialize(Dictionary<Type, BuffCommand> buffCommands)
    {
        _buffCommands = buffCommands;
    }

    // Ŀ�ǵ� ������ Ȱ���ؼ� Undo ����
    // ���� ���α׷��� ���� ���� ����

    protected override void ApplyBuff()
    {
        BuffCommand command;

        _buffCommands[Type.ShootingDuration].Execute(UpgradeableData._additionalDuration, out command);
        _buffHistories.Add(command);

        _buffCommands[Type.ShootingChargeDuration].Execute(UpgradeableData._additionalChargeDuration, out command);
        _buffHistories.Add(command);
    }
}
