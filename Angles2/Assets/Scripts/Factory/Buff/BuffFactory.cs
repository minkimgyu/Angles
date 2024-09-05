using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class BaseBuffData { }

abstract public class BuffCreater
{
    protected BaseBuffData _buffData;
    public BuffCreater(BaseBuffData data) { _buffData = data; }
    // �����ڿ� ����Ʈ ������ �޴� �̺�Ʈ�� �߰����ش�.

    public abstract BaseBuff Create();
}

public class BuffFactory : BaseFactory
{
    Dictionary<BaseBuff.Name, BuffCreater> _buffCreaters;

    public BuffFactory(Dictionary<BaseBuff.Name, BaseBuffData> buffDatas)
    {
        _buffCreaters = new Dictionary<BaseBuff.Name, BuffCreater>();
        _buffCreaters[BaseBuff.Name.Shooting] = new ShootingBuffCreater(buffDatas[BaseBuff.Name.Shooting]);
        _buffCreaters[BaseBuff.Name.Dash] = new DashBuffCreater(buffDatas[BaseBuff.Name.Dash]);

        _buffCreaters[BaseBuff.Name.TotalDamage] = new TotalDamageBuffCreater(buffDatas[BaseBuff.Name.TotalDamage]);
        _buffCreaters[BaseBuff.Name.TotalCooltime] = new TotalCooltimeBuffCreater(buffDatas[BaseBuff.Name.TotalCooltime]);
    }

    public override BaseBuff Create(BaseBuff.Name name)
    {
        return _buffCreaters[name].Create();
    }
}
