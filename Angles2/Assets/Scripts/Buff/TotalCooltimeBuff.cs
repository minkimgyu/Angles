using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalCooltimeBuff : BaseBuff
{
    List<TotalCooltimeBuffUpgradeableData> _upgradeableDatas;
    TotalCooltimeBuffUpgradeableData UpgradeableData { get { return _upgradeableDatas[UpgradePoint]; } }

    public TotalCooltimeBuff(TotalCooltimeBuffData totalCooltimeBuffData) : base() 
    {
        _upgradeableDatas = totalCooltimeBuffData._upgradeableDatas; 
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
        _buffCommands[Type.TotalCooltime].Execute(UpgradeableData._additionalCooltimeRatio, out command);
        _buffHistories.Add(command);
    }
}
