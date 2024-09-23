using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPointComponent
{
    ITarget.Type _targetType;
    BaseFactory _effectFactory;

    Color _blueDamageColor = Color.red; // --> 플레이어가 데미지를 받을 떄 나오는 색

    Color _redDamageColor = Color.white; // --> 적이 데미지를 받을 떄 나오는 색

    public DisplayPointComponent(ITarget.Type targetType, BaseFactory effectFactory)
    {
        _targetType = targetType;
        _effectFactory = effectFactory;
    }

    public void SpawnDamageTxt(float healPoint, Vector3 pos)
    {
        BaseEffect effect = _effectFactory.Create(BaseEffect.Name.DamageTextEffect);

        effect.ResetPosition(pos);
        effect.ResetText(healPoint);
        effect.ResetColor(Color.green);
        effect.Play();
    }

    public void SpawnDamageTxt(DamageableData damageableData, Vector3 pos)
    {
        if (damageableData._damageData.Damage == 0) return; // 데미지가 0이면 출력하지 않음
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
        effect.ResetText(damageableData._damageData.Damage);
        effect.ResetColor(damageColor);
        effect.Play();
    }
}
