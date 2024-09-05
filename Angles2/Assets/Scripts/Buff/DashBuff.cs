using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBuff : BaseBuff
{
    List<DashBuffUpgradeableData> _upgradeableDatas;
    DashBuffUpgradeableData UpgradeableData { get { return _upgradeableDatas[UpgradePoint]; } }


    public DashBuff(DashBuffData dashBuffData) : base()
    {
        _upgradeableDatas = dashBuffData._upgradeableDatas;
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

        _buffCommands[Type.DashSpeed].Execute(UpgradeableData._additionalSpeed, out command);
        _buffHistories.Add(command);

        _buffCommands[Type.DashChargeDuration].Execute(UpgradeableData._additionalChargeDuration, out command);
        _buffHistories.Add(command);
    }
}
