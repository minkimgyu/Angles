using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public enum Name
    {
        StatikkCard,
        KnockbackCard,
        ImpactCard,
        BlackholeCard,
        ShooterCard,
        BladeCard,
        StickyBombCard,
    }

    //public CardData(BaseSkill.Name skillName, )
    //{

    //}

    [SerializeField]
    private BaseSkill.Name _skillName;
    public BaseSkill.Name SkillName { get { return _skillName; } }

    //[SerializeField]
    //private assetre

    [SerializeField]
    private Sprite _iconImage;
    public Sprite Icon { get { return _iconImage; } }

    [SerializeField]
    private string _infoText;
    public string Info { get { return _infoText; } }
}