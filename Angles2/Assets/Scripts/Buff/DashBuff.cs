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

    // 커맨드 패턴을 활용해서 Undo 적용
    // 게임 프로그래밍 패턴 과제 응용

    protected override void ApplyBuff()
    {
        BuffCommand command;

        _buffCommands[Type.DashSpeed].Execute(UpgradeableData._additionalSpeed, out command);
        _buffHistories.Add(command);

        _buffCommands[Type.DashChargeDuration].Execute(UpgradeableData._additionalChargeDuration, out command);
        _buffHistories.Add(command);
    }
}
