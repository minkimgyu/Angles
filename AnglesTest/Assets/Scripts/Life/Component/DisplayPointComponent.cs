using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPointComponent
{
    ITarget.Type _targetType;
    BaseFactory _effectFactory;

    Color _blueDamageColor = Color.red; // --> �÷��̾ �������� ���� �� ������ ��

    Color _redDamageColor = Color.white; // --> ���� �������� ���� �� ������ ��

    public DisplayPointComponent(ITarget.Type targetType, BaseFactory effectFactory)
    {
        _targetType = targetType;
        _effectFactory = effectFactory;
    }

    public void SpawnHealTxt(float healPoint, Vector3 pos)
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.DamageTextEffect);

        effect.ResetPosition(pos);
        effect.ResetText(healPoint);
        effect.ResetColor(Color.green);
        effect.Play();
    }

    public void SpawnDamageTxt(float finalDamage, Vector3 pos)
    {
        if (finalDamage == 0) return; // �������� 0�̸� ������� ����
        Color damageColor = Color.white;

        switch (_targetType)
        {
            case ITarget.Type.Blue:
                damageColor = _blueDamageColor;
                break;
            case ITarget.Type.Red:
                damageColor = _redDamageColor;
                break;
            default:
                break;
        }

        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.DamageTextEffect);

        effect.ResetPosition(pos);
        effect.ResetText(finalDamage);
        effect.ResetColor(damageColor);
        effect.Play();
    }
}