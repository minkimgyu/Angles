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

    // Ŀ�ǵ� ������ Ȱ���ؼ� Undo ����
    // ���� ���α׷��� ���� ���� ����

    protected override void ApplyBuff()
    {
        BuffCommand command;
        _buffCommands[Type.TotalCooltime].Execute(UpgradeableData._additionalCooltimeRatio, out command);
        _buffHistories.Add(command);
    }
}
