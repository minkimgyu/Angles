using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Database
{
    #region INFO ������

    // lock, description
    Dictionary<SkinInfoModel.State, string> _btnInfo = new Dictionary<SkinInfoModel.State, string>
    {
        { SkinInfoModel.State.Lock, "�����ϱ�" },
        { SkinInfoModel.State.UnSelected, "�����ϱ�" },
        { SkinInfoModel.State.Selected, "���� ��" },
    };

    public Dictionary<SkinInfoModel.State, string> BtnInfos { get { return _btnInfo; } }

    Dictionary<PopUpViewer.State, string> _alarmInfos = new Dictionary<PopUpViewer.State, string>
    {
        { PopUpViewer.State.ShortOfGold, "��尡 �����մϴ�." },
        { PopUpViewer.State.NowMaxUpgrade, "�ִ� ���׷��̵� �����Դϴ�." },
    };

    public Dictionary<PopUpViewer.State, string> AlarmInfos { get { return _alarmInfos; } }

    #endregion

    #region SKIN ������

    Dictionary<SkinData.Key, List<IStatModifier>> _skinModifiers = new Dictionary<SkinData.Key, List<IStatModifier>>
    {
        {
            SkinData.Key.Normal, 
            new List<IStatModifier>{ }
        },

        {
            SkinData.Key.BloodEater,
            new List<IStatModifier>
            {
                new DrainStatModifier(0.1f),
            }
        },

        {
            SkinData.Key.Guard,
            new List<IStatModifier>
            {
                new DamageReductionStatModifier(0.1f),
            }
        },
    };
    public Dictionary<SkinData.Key, List<IStatModifier>> SkinModifiers { get { return _skinModifiers; } }

    Dictionary<SkinData.Key, SkinData> _skinDatas = new Dictionary<SkinData.Key, SkinData>
    {
        {
            SkinData.Key.Normal, new SkinData(
            "�⺻",
            0,
            "���� ��ü")
        },

        {
            SkinData.Key.BloodEater, new SkinData(
            "���� ����",
            300,
            "���� 10%")
        },

        {
            SkinData.Key.Guard, new SkinData(
            "����",
            300,
            "������ ���� 10%")
        },
    };
    public Dictionary<SkinData.Key, SkinData> SkinDatas { get { return _skinDatas; } }

    #endregion

    #region STAT ������

    Dictionary<StatData.Key, IStatModifier> _statModifiers = new Dictionary<StatData.Key, IStatModifier>
    {
        {
            StatData.Key.AttackDamage,
            new AttackDamageStatModifier
            (
                new List<float>{ 5f, 5f, 5f, 5f, 10f }
            )
        },

        {
            StatData.Key.MoveSpeed,
            new MoveSpeedStatModifier
            (
                new List<float>{ 1f, 1f, 1f }
            )
        },

        {
            StatData.Key.AutoHpRecovery,
             new AutoHpRecoveryStatModifier
            (
                new List<float>{ 2, 3, 2, 3, 5 }
            )
        },

        {
            StatData.Key.MaxHp,
             new HealthStatModifier
            (
                new List<float>{ 10, 5, 10, 10, 15 }
            )
        },

        {
            StatData.Key.DamageReduction,
             new DamageReductionStatModifier
            (
                new List<float>{ 0.05f, 0.05f, 0.05f, 0.05f, 0.1f }
            )
        },
    };
    public Dictionary<StatData.Key, IStatModifier> StatModifiers { get { return _statModifiers; } }

    Dictionary<StatData.Key, StatData> _statDatas = new Dictionary<StatData.Key, StatData>
    {
        {
            StatData.Key.AttackDamage, new StatData(
            "���ݷ�",
            new List<int>{ 100, 200, 300, 400, 500},
            5,
            new List<string>{ "���ݷ� 5 ����", "���ݷ� 10 ����", "���ݷ� 15 ����", "���ݷ� 20 ����", "���ݷ� 30 ����"})
        },

        { 
            StatData.Key.MoveSpeed, new StatData(
            "�̵� �ӵ�",
            new List<int>{ 100, 200, 300},
            3,
            new List<string>{ "�̵� �ӵ� 1 ����", "�̵� �ӵ� 2 ����", "�̵� �ӵ� 3 ����"}) 
        },

        {
            StatData.Key.AutoHpRecovery, new StatData(
            "ü�� ȸ��",
            new List<int>{ 100, 200, 300, 400, 500 },
            5,
            new List<string>
            { 
                "���� �ð����� ü�� 2 ȸ��", 
                "���� �ð����� ü�� 5 ȸ��", 
                "���� �ð����� ü�� 7 ȸ��", 
                "���� �ð����� ü�� 10 ȸ��", 
                "���� �ð����� ü�� 15 ȸ��" 
            })
        },

        {
            StatData.Key.MaxHp, new StatData(
            "�ִ� ü��",
            new List<int>{ 100, 200, 300, 400, 500 },
            5,
            new List<string>{ "ü�� 10 ����", "ü�� 15 ����", "ü�� 25 ����", "ü�� 35 ����", "ü�� 50 ����" })
        },

        {
            StatData.Key.DamageReduction, new StatData(
            "�޴� ���� ����",
            new List<int>{ 100, 200, 300, 400, 500 },
            5,
            new List<string>{ "�޴� ���� ���� 5%", "�޴� ���� ���� 10%", "�޴� ���� ���� 15%", "�޴� ���� ���� 20%", "�޴� ���� ���� 30%" })
        },
    };
    public Dictionary<StatData.Key, StatData> StatDatas { get { return _statDatas; } }

    #endregion

    #region SKILL ������

    List<BaseSkill.Name> _upgradeableSkills = new List<BaseSkill.Name>
    {
        BaseSkill.Name.Statikk,
        BaseSkill.Name.Knockback,
        BaseSkill.Name.Impact,
        BaseSkill.Name.SpawnBlackhole,
        BaseSkill.Name.SpawnStickyBomb,
        BaseSkill.Name.SpawnBlade,

        BaseSkill.Name.SpawnRifleShooter,
        BaseSkill.Name.SpawnRocketShooter,

        BaseSkill.Name.UpgradeDamage,
        BaseSkill.Name.UpgradeCooltime,
        BaseSkill.Name.UpgradeDash,
        BaseSkill.Name.UpgradeShooting,
    };

    public List<BaseSkill.Name> UpgradeableSkills { get { return _upgradeableSkills; } }

    Dictionary<BaseSkill.Name, IUpgradeVisitor> _upgrader = new Dictionary<BaseSkill.Name, IUpgradeVisitor>
    {
        {
            BaseSkill.Name.Statikk,
            new StatikkUpgrader
            (
                new List<StatikkUpgrader.UpgradableData>
                {
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0), // 2 �� ��
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1), // 3 �� ��
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0), // 4 �� ��
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1), // 5 �� ��
                }
            )
        },
        {
            BaseSkill.Name.Knockback,
            new KnockbackUpgrader
            (
                new List<KnockbackUpgrader.UpgradableData>
                {
                    new KnockbackUpgrader.UpgradableData(0, 0.2f),
                    new KnockbackUpgrader.UpgradableData(10, 0.0f),
                    new KnockbackUpgrader.UpgradableData(0, 0.3f),
                    new KnockbackUpgrader.UpgradableData(10, 0.3f),
                }
            )
        },
        {
            BaseSkill.Name.Impact,
            new ImpactUpgrader
            (
                new List<ImpactUpgrader.UpgradableData>
                {
                    new ImpactUpgrader.UpgradableData(10, 0.2f),
                    new ImpactUpgrader.UpgradableData(10, 0.2f),
                    new ImpactUpgrader.UpgradableData(10, 0.3f),
                    new ImpactUpgrader.UpgradableData(10, 0.3f),
                }
            )
        },
        {
            BaseSkill.Name.SpawnBlackhole,
            new SpawnBlackholeUpgrader
            (
                new List<SpawnBlackholeUpgrader.UpgradableData>
                {
                    new SpawnBlackholeUpgrader.UpgradableData(1, 0.1f),
                    new SpawnBlackholeUpgrader.UpgradableData(0, 0.1f),
                    new SpawnBlackholeUpgrader.UpgradableData(1, 0.1f),
                    new SpawnBlackholeUpgrader.UpgradableData(1, 0.2f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnBlade,
            new SpawnBladeUpgrader
            (
                new List<SpawnBladeUpgrader.UpgradableData>
                {
                    new SpawnBladeUpgrader.UpgradableData(10, 0, 0.1f),
                    new SpawnBladeUpgrader.UpgradableData(0, 1, 0.1f),
                    new SpawnBladeUpgrader.UpgradableData(10, 0, 0.1f),
                    new SpawnBladeUpgrader.UpgradableData(10, 1, 0.2f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnStickyBomb,
            new SpawnStickyBombUpgrader
            (
                new List<SpawnStickyBombUpgrader.UpgradableData>
                {
                    new SpawnStickyBombUpgrader.UpgradableData(10, 1),
                    new SpawnStickyBombUpgrader.UpgradableData(0, 1),
                    new SpawnStickyBombUpgrader.UpgradableData(10, 0),
                    new SpawnStickyBombUpgrader.UpgradableData(10, 1),
                }
            )
        },
         {
            BaseSkill.Name.SpawnRocketShooter,
            new SpawnShooterUpgrader
            (
                new List<SpawnShooterUpgrader.UpgradableData>
                {
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(10, -0.5f),
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(10, -0.5f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnRifleShooter,
            new SpawnShooterUpgrader
            (
                new List<SpawnShooterUpgrader.UpgradableData>
                {
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(0, -0.2f),
                    new SpawnShooterUpgrader.UpgradableData(10, 0),
                    new SpawnShooterUpgrader.UpgradableData(0, -0.3f),
                }
            )
        },

         // �⺻ ����: 4
         // �߰� ���׷��̵� ��ġ: 1

         // Ư�� �簢��(������) --> LV.2, �Ϲ� �簢��(�����) --> LV.1
         // ������, �߻� �ӵ�
         {
            BaseSkill.Name.SpreadBullets,
            new SpreadBulletsUpgrader
            (
                new List<SpreadBulletsUpgrader.UpgradableData>
                {
                    new SpreadBulletsUpgrader.UpgradableData(-1, 5, 2),
                    new SpreadBulletsUpgrader.UpgradableData(-1, 5, 2),
   
                }
            )
        },
         {
            BaseSkill.Name.Shockwave,
            new ShockwaveUpgrader
            (
                new List<ShockwaveUpgrader.UpgradableData>
                {
                    new ShockwaveUpgrader.UpgradableData(10, -0.5f, 0.2f),
                    new ShockwaveUpgrader.UpgradableData(10, -0.5f, 0.3f),
                }
            )
        },
         {
            BaseSkill.Name.MagneticField,
            new MagneticFieldUpgrader
            (
                new List<MagneticFieldUpgrader.UpgradableData>
                {
                    new MagneticFieldUpgrader.UpgradableData(5, 0),
                    new MagneticFieldUpgrader.UpgradableData(5, 0),
                }
            )
        },
         {
            BaseSkill.Name.SelfDestruction,
            new SelfDestructionUpgrader
            (
                new List<SelfDestructionUpgrader.UpgradableData>
                {
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                    new SelfDestructionUpgrader.UpgradableData(1, 8, 3),
                }
            )
        },
    };

    public Dictionary<BaseSkill.Name, IUpgradeVisitor> Upgraders { get { return _upgrader; } }

    Dictionary<BaseSkill.Name, SkillData> CopySkillDatas = new Dictionary<BaseSkill.Name, SkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1, 10, 1f, 3, 3, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1, 20, 1, 3, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Impact, new ImpactData(5, 0.2f, 20, 1, 2, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 0.1f, 0, 1, 0.3f, 3, new List<ITarget.Type> { ITarget.Type.Red })},

        { BaseSkill.Name.SpawnRifleShooter, new SpawnShooterData(5, BaseWeapon.Name.RifleShooter, 10, 1, 1, 1, BaseWeapon.Name.ShooterBullet, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnRocketShooter, new SpawnShooterData(5, BaseWeapon.Name.RocketShooter, 20, 1, 1, 3, BaseWeapon.Name.Rocket, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 0.3f, 10, 1, 1, 3f, 8f, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(5, 5f, 1, 30, 1, 1, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ContactAttack, new ContactAttackData(1, 10, 1, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        {
            BaseSkill.Name.UpgradeDamage,
            new UpgradeDamageData(
                3,
                new DamageRatioStatModifier(
                    new List<float>
                    {
                        0.1f,
                        0.15f,
                        0.25f,
                    }
                )
            )
        },

        {
            BaseSkill.Name.UpgradeCooltime,
            new UpgradeCooltimeData(
                3,
                new CooltimeStatModifier(
                    new List<float>
                    {
                        -0.1f,
                        -0.1f,
                        -0.1f,
                    }
                )
            )
        },

         {
            BaseSkill.Name.UpgradeShooting,
            new UpgradeShootingData(
                3,
                new ShootingStatModifier(
                    new List<ShootingStatModifier.ShootingStat>
                    {
                        new ShootingStatModifier.ShootingStat(0.5f, -0.3f),
                        new ShootingStatModifier.ShootingStat(0.5f, -0.3f),
                        new ShootingStatModifier.ShootingStat(1, -0.4f),
                    }
                )
            )
        },

        {
            BaseSkill.Name.UpgradeDash,
            new UpgradeDashData(
                3,
                new DashStatModifier(
                    new List<DashStatModifier.DashStat>
                    {
                        new DashStatModifier.DashStat(2f, -1f),
                        new DashStatModifier.DashStat(3f, -1f),
                        new DashStatModifier.DashStat(5f, -1f),
                    }
                )
            )
        },

        { BaseSkill.Name.MultipleShockwave, new MultipleShockwaveData(1, 0.5f, 0.7f, 3, 30f, 1f, 1.7f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SpreadMultipleBullets, new SpreadMultipleBulletsData(1, 0.3f, 4, 10, 1, 1, 2f, 4f, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue }) },

        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(3, 10, 1, 1, 4f, 4f, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue })},
        { BaseSkill.Name.Shockwave, new ShockwaveData(3, 30f, 1f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(3, 5f, 1f, 1f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(3, 20f, 1f, 5f, 3f, 0.7f, new List<ITarget.Type>(){ITarget.Type.Red, ITarget.Type.Blue}) },
    };

    public Dictionary<BaseSkill.Name, SkillData> SkillDatas { get { return CopySkillDatas; } }

    #endregion

    #region WEAPON ������

    Dictionary<BaseWeapon.Name, WeaponData> CopyWeaponDatas = new Dictionary<BaseWeapon.Name, WeaponData>
    {
        { BaseWeapon.Name.Blade, new BladeData(1, 1)},

        { BaseWeapon.Name.PentagonicBullet, new BulletData(5)},
        { BaseWeapon.Name.ShooterBullet, new BulletData(5)},
        { BaseWeapon.Name.PentagonBullet, new BulletData(5)},
        { BaseWeapon.Name.Rocket, new RocketData(3, 5)},

        { BaseWeapon.Name.Blackhole, new BlackholeData(100, 0.1f)},
        { BaseWeapon.Name.StickyBomb, new StickyBombData(3, 3)},

        { BaseWeapon.Name.RifleShooter, new ShooterData(10, 1, 18, 4.0f, 10.0f)},
        { BaseWeapon.Name.RocketShooter, new ShooterData(10, 1, 18, 4.0f, 10.0f)},

    };
    public Dictionary<BaseWeapon.Name, WeaponData> WeaponDatas { get { return CopyWeaponDatas; } }

    #endregion

    #region LIFE ������

    Dictionary<BaseLife.Name, LifeData> CopyLifeDatas = new Dictionary<BaseLife.Name, LifeData>
    {
        { 
            BaseLife.Name.Player, new PlayerData(
                100, 
                ITarget.Type.Blue, 
                0f, 
                0f,

                10,
                1,
                1,
                1,

                0f,
                10f,

                1.1f,

                8,

                0.4f, 

                2f,

                15f,
                0.2f, 
                3, 
                1, 

                5f, 

                0.15f,
                0.25f,
            new List<BaseSkill.Name> { BaseSkill.Name.ContactAttack })
        },

        { 
            BaseLife.Name.YellowTriangle, new TriangleData(10, ITarget.Type.Red, BaseLife.Size.Small,
                
            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 0 }
            },

            new DropData(3, 
                new List<Tuple<IInteractable.Name, float>>
                { 
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 8) 
        },

        { 
            BaseLife.Name.YellowRectangle, new RectangleData(20, ITarget.Type.Red, BaseLife.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 6)
        },

        { 
            BaseLife.Name.YellowPentagon, new PentagonData(40, ITarget.Type.Red, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 4f, 3f, 1f)
        },

        { 
            BaseLife.Name.YellowHexagon, new HexagonData(60, ITarget.Type.Red, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f),
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Heart, 0.1f),
                    //new Tuple<IInteractable.Name, float>( IInteractable.Name.SkillBubble, 0.1f)
                }
            ), 4f, 3f, 1f)
        },



        // ��ų ���׷��̵� ����
        {
            BaseLife.Name.RedTriangle, new TriangleData(15, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 11)
        },

        {
            BaseLife.Name.RedRectangle, new RectangleData(30, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 },
                { BaseSkill.Name.SelfDestruction, 0 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6)
        },

        {
            BaseLife.Name.RedPentagon, new PentagonData(75, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 4f, 3f, 1f)
        },

        {
            BaseLife.Name.RedHexagon, new HexagonData(90, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 1 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Tricon, new TriconData(200, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 15f, 3f, 1f, 2f, 4f)
        },

         {
            BaseLife.Name.Rhombus, new RhombusData(200, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MultipleShockwave, 0 },
                { BaseSkill.Name.MagneticField, 2 }
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Pentagonic, new PentagonicData(200, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadMultipleBullets, 0 },
            },

            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 6f, 3f, 1f)
        },

    };
    public Dictionary<BaseLife.Name, LifeData> LifeDatas { get { return CopyLifeDatas; } }

    #endregion

    #region CARD ������

    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas = new Dictionary<BaseSkill.Name, CardInfoData>
    {
        { BaseSkill.Name.SpawnBlackhole, new CardInfoData(BaseSkill.Name.SpawnBlackhole, "Spawn Blackhole In My Position", 20) },
        { BaseSkill.Name.SpawnBlade, new CardInfoData(BaseSkill.Name.SpawnBlade, "Spawn Blade", 20) },
        { BaseSkill.Name.Impact, new CardInfoData(BaseSkill.Name.Impact, "Spawn Impact", 20) },
        { BaseSkill.Name.Knockback, new CardInfoData(BaseSkill.Name.Knockback, "Spawn Knockback", 20) },

        { BaseSkill.Name.SpawnRifleShooter, new CardInfoData(BaseSkill.Name.SpawnRifleShooter, "Spawn Rifle Shooter", 20) },
        { BaseSkill.Name.SpawnRocketShooter, new CardInfoData(BaseSkill.Name.SpawnRocketShooter, "Spawn Rocket Shooter", 20) },

        { BaseSkill.Name.Statikk, new CardInfoData(BaseSkill.Name.Statikk, "Spawn Statikk", 20) },
        { BaseSkill.Name.SpawnStickyBomb, new CardInfoData(BaseSkill.Name.SpawnStickyBomb, "Spawn StickyBomb", 20) },

        { BaseSkill.Name.UpgradeDash, new CardInfoData(BaseSkill.Name.UpgradeDash, "Create DashBuff", 20) },
        { BaseSkill.Name.UpgradeShooting, new CardInfoData(BaseSkill.Name.UpgradeShooting, "Create ShootingBuff", 20) },
        { BaseSkill.Name.UpgradeDamage, new CardInfoData(BaseSkill.Name.UpgradeDamage, "Create TotalDamageBuff", 20) },
        { BaseSkill.Name.UpgradeCooltime, new CardInfoData(BaseSkill.Name.UpgradeCooltime, "Create TotalCooltimeBuff", 20) },
    };
    public Dictionary<BaseSkill.Name, CardInfoData> CardDatas { get { return _cardDatas; } }

    #endregion

    #region INTERACTABLE ������

    Dictionary<IInteractable.Name, BaseInteractableObjectData> _interactableObjectDatas = new Dictionary<IInteractable.Name, BaseInteractableObjectData>
    {
        { IInteractable.Name.CardTable, new CardTableData(3) },
        { IInteractable.Name.Coin, new CoinData(5, 25) },
        { IInteractable.Name.Heart, new HeartData(20, 25) },
        { IInteractable.Name.SkillBubble, new SkillBubbleData(1, 25) },
        { IInteractable.Name.Shop, new ShopData(3, 3) }
    };

    public Dictionary<IInteractable.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }

    #endregion

    #region SAVE ������

    SaveData _defaultSaveData = new SaveData
    (
        10000,
        DungeonMode.Chapter.TriconChapter,
        new Dictionary<DungeonMode.Chapter, ChapterInfo>
        {
            { DungeonMode.Chapter.TriconChapter, new ChapterInfo("������ ����", 0, 20, false) },
            { DungeonMode.Chapter.RhombusChapter, new ChapterInfo("���� ����", 0, 20, true) },
            { DungeonMode.Chapter.PentagonicChapter, new ChapterInfo("�ʹ� �����", 0, 20, true) },
        },
        new Dictionary<StatData.Key, int>
        {
            { StatData.Key.AttackDamage, 0 },
            { StatData.Key.MaxHp, 0 },
            { StatData.Key.AutoHpRecovery, 0 },
            { StatData.Key.MoveSpeed, 0 },
            { StatData.Key.DamageReduction, 0 },
        },

        SkinData.Key.Normal,
        new Dictionary<SkinData.Key, bool>
        {
            { SkinData.Key.Normal, false },
            { SkinData.Key.BloodEater, true },
            { SkinData.Key.Guard, true },
        }
    );

    public SaveData DefaultSaveData { get { return _defaultSaveData; } }

    #endregion
}
