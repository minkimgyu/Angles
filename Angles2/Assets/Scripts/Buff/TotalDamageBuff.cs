using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalDamageBuff : BaseBuff
{
    List<TotalDamageBuffUpgradeableData> _upgradeableDatas;
    TotalDamageBuffUpgradeableData UpgradeableData { get { return _upgradeableDatas[UpgradePoint]; } }

    public TotalDamageBuff(TotalDamageBuffData data) : base()
    {
        _upgradeableDatas = data._totalDamageBuffUpgradeableDatas;
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
        _buffCommands[Type.TotalDamage].Execute(UpgradeableData._additionalDamageRatio, out command);
        _buffHistories.Add(command);
    }
}