using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStatModifier : IStatModifier
{
    public struct DashStat
    {
        public float _additionalSpeed; // ���󰡴� �ð� ����
        public float _additionalRestoreDuration; // ���� �ð� ����

        public DashStat(float additionalSpeed, float additionalRestoreDuration)
        {
            _additionalSpeed = additionalSpeed;
            _additionalRestoreDuration = additionalRestoreDuration;
        }
    }

    List<DashStat> _dashStats;
    DashStat _dashStat;

    public DashStatModifier(List<DashStat> dashStats)
    {
        _dashStats = dashStats;
    }

    public DashStatModifier(DashStat dashStat)
    {
        _dashStat = dashStat;
    }

    public void Visit<T>(T data, int level) where T : PlayerData
    {
        data.DashSpeed += _dashStats[level]._additionalSpeed;
        data.DashRestoreDuration += _dashStats[level]._additionalRestoreDuration;
    }

    public void Visit<T>(T data) where T : PlayerData
    {
        data.DashSpeed += _dashStat._additionalSpeed;
        data.DashRestoreDuration += _dashStat._additionalRestoreDuration;
    }
}