using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBBuilder : MonoBehaviour
{
    #region INFO ������

    Dictionary<SkinInfoModel.State, string> _buyInfo = new Dictionary<SkinInfoModel.State, string>
    {
        { SkinInfoModel.State.Lock, "�����ϱ�" },
        { SkinInfoModel.State.UnSelected, "�����ϱ�" },
        { SkinInfoModel.State.Selected, "���� ��" },
    };

    Dictionary<PopUpViewer.State, string> _popUpInfos = new Dictionary<PopUpViewer.State, string>
    {
        { PopUpViewer.State.ShortOfGold, "��尡 �����մϴ�." },
        { PopUpViewer.State.NowMaxUpgrade, "�ִ� ���׷��̵� �����Դϴ�." },
    };

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

    Dictionary<SkinData.Key, SkinData> _skinDatas = new Dictionary<SkinData.Key, SkinData>
    {
        {
            SkinData.Key.Normal, new SkinData(0)
        },

        {
            SkinData.Key.BloodEater, new SkinData(2000)
        },

        {
            SkinData.Key.Guard, new SkinData(500)
        },
    };

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

    Dictionary<StatData.Key, StatData> _statDatas = new Dictionary<StatData.Key, StatData>
    {
        {
            StatData.Key.AttackDamage, new StatData(
            5,
            new List<int>{ 100, 200, 400, 700, 1000})
        },

        {
            StatData.Key.MoveSpeed, new StatData(
            3,
            new List<int>{ 100, 500, 1000 })
        },
        {
            StatData.Key.MaxHp, new StatData(
            5,
            new List<int>{ 100, 200, 400, 700, 1000 })
        },

        {
            StatData.Key.DamageReduction, new StatData(
            5,
            new List<int>{ 100, 200, 400, 700, 1000 })
        },
    };

    #endregion

    #region SKILL ������

    HashSet<BaseSkill.Name> _upgradeableSkills = new HashSet<BaseSkill.Name>
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
        BaseSkill.Name.UpgradeShooting,
    };

    Dictionary<BaseSkill.Name, IUpgradeVisitor> _skillUpgrader = new Dictionary<BaseSkill.Name, IUpgradeVisitor>
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
                    new SpawnShooterUpgrader.UpgradableData(5, -0.2f),
                    new SpawnShooterUpgrader.UpgradableData(5, -0.2f),
                    new SpawnShooterUpgrader.UpgradableData(5, -0.2f),
                    new SpawnShooterUpgrader.UpgradableData(5, -0.2f),
                }
            )
        },
         {
            BaseSkill.Name.SpawnRifleShooter,
            new SpawnShooterUpgrader
            (
                new List<SpawnShooterUpgrader.UpgradableData>
                {
                    new SpawnShooterUpgrader.UpgradableData(1, -0.1f),
                    new SpawnShooterUpgrader.UpgradableData(2, -0.1f),
                    new SpawnShooterUpgrader.UpgradableData(2, -0.1f),
                    new SpawnShooterUpgrader.UpgradableData(4, -0.1f),
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
                    new SpreadBulletsUpgrader.UpgradableData(-1, 5, 2), // Lv.2
                    new SpreadBulletsUpgrader.UpgradableData(-1, 5, 2), // Lv.3
                }
            )
        },
         {
            BaseSkill.Name.Shockwave,
            new ShockwaveUpgrader
            (
                new List<ShockwaveUpgrader.UpgradableData>
                {
                    new ShockwaveUpgrader.UpgradableData(10, -0.5f, 0.2f), // Lv.2
                    new ShockwaveUpgrader.UpgradableData(10, -0.5f, 0.3f), // Lv.3
                }
            )
        },
         {
            BaseSkill.Name.MagneticField,
            new MagneticFieldUpgrader
            (
                new List<MagneticFieldUpgrader.UpgradableData>
                {
                    new MagneticFieldUpgrader.UpgradableData(5, 0), // Lv.2
                    new MagneticFieldUpgrader.UpgradableData(5, 0), // Lv.3
                }
            )
        },
         {
            BaseSkill.Name.SelfDestruction,
            new SelfDestructionUpgrader
            (
                new List<SelfDestructionUpgrader.UpgradableData>
                {
                    new SelfDestructionUpgrader.UpgradableData(10, 0, 0),
                    new SelfDestructionUpgrader.UpgradableData(20, 10, 0),
                }
            )
        },
    };

    Dictionary<BaseSkill.Name, SkillData> _skillDatas = new Dictionary<BaseSkill.Name, SkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1, 10, 1f, 3, 3, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1, 20, 1, 3, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Impact, new ImpactData(5, 0.2f, 20, 1, 2, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 0.1f, 0, 1, 0.3f, 3, new List<ITarget.Type> { ITarget.Type.Red })},

        { BaseSkill.Name.SpawnRifleShooter, new SpawnShooterData(5, BaseWeapon.Name.RifleShooter, 1, 0.1f, 0, 0.6f, BaseWeapon.Name.ShooterBullet, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnRocketShooter, new SpawnShooterData(5, BaseWeapon.Name.RocketShooter, 5, 0.5f, 0, 3, BaseWeapon.Name.Rocket, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 0.3f, 10, 1, 1, 3f, 8f, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(5, 5f, 1, 30, 1, 1, 3, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ContactAttack, new ContactAttackData(0, 5, 1, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

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

        { BaseSkill.Name.ShootMultipleLaser, new ShootMultipleLaserData(0, 20f, 1f, 30f, 1f, 8, 4, 2f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MultipleShockwave, new MultipleShockwaveData(0, 0.5f, 0.7f, 3, 30f, 1f, 1.7f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SpreadMultipleBullets, new SpreadMultipleBulletsData(0, 0.3f, 4, 10, 1, 1, 2f, 4f, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue }) },

        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(3, 10, 1, BaseWeapon.Name.PentagonBullet, 4f, 4f, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue })},
        { BaseSkill.Name.SpreadReflectableBullets, new SpreadBulletsData(1, 10, 1, BaseWeapon.Name.HexahornBullet, 1f, 4f, 6f, 1f, new List<ITarget.Type> { ITarget.Type.Blue })},

        { BaseSkill.Name.Shockwave, new ShockwaveData(3, 30f, 1f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(3, 2f, 1f, 0.5f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(3, 20f, 1f, 5f, 3f, 0.7f, new List<ITarget.Type>(){ITarget.Type.Red, ITarget.Type.Blue}) },
    };

    #endregion

    #region WEAPON ������
    Dictionary<BaseWeapon.Name, WeaponData> _weaponDatas = new Dictionary<BaseWeapon.Name, WeaponData>
    {
        { BaseWeapon.Name.Blade, new BladeData(1, 1)},

        { BaseWeapon.Name.ShooterBullet, new BulletData(5)},
        { BaseWeapon.Name.PentagonBullet, new BulletData(5)},
        { BaseWeapon.Name.PentagonicBullet, new BulletData(5)},
        { BaseWeapon.Name.HexahornBullet, new BulletData(5)},

        { BaseWeapon.Name.Rocket, new RocketData(3, 5)},

        { BaseWeapon.Name.Blackhole, new BlackholeData(-10, 0.1f)},
        { BaseWeapon.Name.StickyBomb, new StickyBombData(3, 3)},

        { BaseWeapon.Name.RifleShooter, new ShooterData(10, 1, 18, 1.5f, new SerializableVector2(1, 1), 10.0f)},
        { BaseWeapon.Name.RocketShooter, new ShooterData(10, 1, 18, 1.5f, new SerializableVector2(-1, 1),10.0f)},

    };

    #endregion

    #region DROP ������

    Dictionary<BaseLife.Name, DropData> _chapterDropDatas = new Dictionary<BaseLife.Name, DropData>
    {
        {
            BaseLife.Name.YellowTriangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.YellowRectangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.YellowPentagon,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f },
                    { IInteractable.Name.Heart, 0.1f },
                }
            )
        },
        {
            BaseLife.Name.YellowHexagon,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f },
                    { IInteractable.Name.Heart, 0.1f },
                }
            )
        },

        {
            BaseLife.Name.RedTriangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.RedRectangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.RedPentagon,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f },
                    { IInteractable.Name.Heart, 0.1f },
                }
            )
        },
        {
            BaseLife.Name.RedHexagon,
            new DropData(
                3,
               new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f },
                    { IInteractable.Name.Heart, 0.1f },
                }
            )
        },


        {
            BaseLife.Name.OperaTriangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.OperaRectangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.OperaPentagon,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f },
                    { IInteractable.Name.Heart, 0.1f },
                }
            )
        },
        {
            BaseLife.Name.OperaHexagon,
            new DropData(
                3,
               new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f },
                    { IInteractable.Name.Heart, 0.1f },
                }
            )
        },

        {
            BaseLife.Name.Tricon,
            new DropData(
                1,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.Rhombus,
            new DropData(
                1,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.Pentagonic,
            new DropData(
                1,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.Hexahorn,
            new DropData(
                1,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.Octavia,
            new DropData(
                1,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        }
    };

    #endregion

    #region LIFE ������

    Dictionary<BaseLife.Name, LifeData> _lifeDatas = new Dictionary<BaseLife.Name, LifeData>
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
                0.3f,
                10f,

                1.1f,

                8,

                0.4f,

                2f,

                20f,
                0.2f,
                3,
                1,

                5f,

                0.6f,
                1.0f,
            new List<BaseSkill.Name> { BaseSkill.Name.ContactAttack })
        },

        {
            BaseLife.Name.YellowTriangle, new TriangleData(10, ITarget.Type.Red, BaseLife.Size.Small,

            new Dictionary<BaseSkill.Name, int>{{BaseSkill.Name.MagneticField, 0 }}, 8)
        },

        {
            BaseLife.Name.YellowRectangle, new RectangleData(20, ITarget.Type.Red, BaseLife.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 0 }
            }, 6)
        },

        {
            BaseLife.Name.YellowPentagon, new PentagonData(40, ITarget.Type.Red, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 0 }
            }, 4f, 3f, 1f)
        },

        {
            BaseLife.Name.YellowHexagon, new HexagonData(60, ITarget.Type.Red, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 0 }
            }, 4f, 3f, 1f)
        },



        // ��ų ���׷��̵� ����
        {
            BaseLife.Name.RedTriangle, new TriangleData(15, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 }
            }, 11)
        },

        {
            BaseLife.Name.RedRectangle, new RectangleData(30, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 },
                { BaseSkill.Name.SelfDestruction, 0 }
            }, 6)
        },

        {
            BaseLife.Name.RedPentagon, new PentagonData(75, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 1 }
            }, 4f, 3f, 1f)
        },

        {
            BaseLife.Name.RedHexagon, new HexagonData(90, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 1 }
            }, 6f, 3f, 1f)
        },



        {
            BaseLife.Name.OperaTriangle, new TriangleData(25, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            }, 5)
        },

        {
            BaseLife.Name.OperaRectangle, new RectangleData(45, ITarget.Type.Red, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 },
            }, 9)
        },

        {
            BaseLife.Name.OperaPentagon, new PentagonData(100, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 2 }
            }, 3f, 3f, 1f)
        },

        {
            BaseLife.Name.OperaHexagon, new OperaHexagonData(120, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 2 }
            }, 9f, 3f, 2f, 2f, 2f, 1f)
        },



         {
            BaseLife.Name.Tricon, new TriconData(300, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            }, 15f, 3f, 1f, 2f, 4f)
        },

         {
            BaseLife.Name.Rhombus, new RhombusData(400, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MultipleShockwave, 0 },
                { BaseSkill.Name.MagneticField, 2 }
            }, 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Pentagonic, new PentagonicData(500, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadMultipleBullets, 0 },
            }, 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Hexahorn, new HexahornData(600, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadReflectableBullets, 0 },
            }, 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Octavia, new OctaviaData(800, ITarget.Type.Red, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.ShootMultipleLaser, 0 },
            }, 6f, 3f, 1f)
        }

    };

    #endregion

    #region CARD ������
    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas = new Dictionary<BaseSkill.Name, CardInfoData>
    {
        { BaseSkill.Name.SpawnBlackhole, new CardInfoData("��Ȧ", "���� ���Ƶ��̴� ������ �����Ѵ�.", 30) },
        { BaseSkill.Name.SpawnBlade, new CardInfoData("���̵�", "���� ƨ�ܴٴϸ� �����ϴ� Į���� �߻��Ѵ�.", 30) },
        { BaseSkill.Name.Impact, new CardInfoData("�����", "������ ������ ���� ƨ�ܳ��� ������Ų��.", 30) },
        { BaseSkill.Name.Knockback, new CardInfoData("�˹�", "���� �ָ� ���ĳ��� �����Ѵ�.", 30) },

        { BaseSkill.Name.SpawnRifleShooter, new CardInfoData("��:����", "ź�� �߻��ϴ� ���� ��ȯ�Ѵ�.", 20) },
        { BaseSkill.Name.SpawnRocketShooter, new CardInfoData("��:�չ�", "��ź�� �߻��ϴ� ���� ��ȯ�Ѵ�.", 20) },

        { BaseSkill.Name.Statikk, new CardInfoData("���� ������", "��ó�� ���� Ÿ���ϴ� �������� �߻��Ѵ�.", 30) },
        { BaseSkill.Name.SpawnStickyBomb, new CardInfoData("���� ��ź", "������ ��ź�� �ٿ� �����ð� ���� ���߽�Ų��.", 20) },

        { BaseSkill.Name.UpgradeShooting, new CardInfoData("���� ��ȭ", "������ ��¡ �ӵ��� ���ҽ�Ű�� �����ð��� ������Ų��.", 10) },
        { BaseSkill.Name.UpgradeDamage, new CardInfoData("���� ������ ����", "���� �������� ��½�Ų��.", 10) },
        { BaseSkill.Name.UpgradeCooltime, new CardInfoData("��ų ��Ÿ�� ����", "��Ÿ�� ��ų�� ���� ���ð��� ���ҽ�Ų��.", 10) },
    };

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

    #endregion

    #region LEVEL ������

    Dictionary<GameMode.Level, ILevelInfo> _levelDatas = new Dictionary<GameMode.Level, ILevelInfo>()
    {
        { GameMode.Level.TriconChapter, new ChapterInfo("Tricon", "ù �߰���", 20, GameMode.Level.RhombusChapter) },
        { GameMode.Level.RhombusChapter, new ChapterInfo("Rhombus", "��������", 20, GameMode.Level.PentagonicChapter) },
        { GameMode.Level.PentagonicChapter, new ChapterInfo("Pentagonic", "�÷�1", 20, GameMode.Level.HexahornChapter) },
        { GameMode.Level.HexahornChapter, new ChapterInfo("Hexahorn", "�÷�2", 20, GameMode.Level.OctaviaChapter) },
        { GameMode.Level.OctaviaChapter, new ChapterInfo("Octavia", "�÷�3", 20) },

        { GameMode.Level.PyramidSurvival, new SurvivalInfo("Pyramid", "ù �߰���", 300, GameMode.Level.CubeSurvival) },
        { GameMode.Level.CubeSurvival, new SurvivalInfo("Cube", "��������", 300, GameMode.Level.PrismSurvival) },
        { GameMode.Level.PrismSurvival, new SurvivalInfo("Prism", "�÷�", 300) },
    };

    #endregion

    #region CoinGauge ������

    List<int> _coinGaugeData = new List<int>() { 50, 100, 150, 200, 250, 300, 350, 400, 500 }; // 0������ ����

    #endregion

    [ContextMenu("Create DB")]
    public void CreateDBJson()
    {
        JsonParser jsonParser = new JsonParser();
        FileIO fileIO = new FileIO(jsonParser, ".txt");

        string fileName = "Database";
        string fileLocation = "JsonData";

        Database database = new Database
        (
            _buyInfo,
            _popUpInfos,
            _skinModifiers,
            _skinDatas,
            _statModifiers,
            _statDatas,
            _upgradeableSkills,
            _skillUpgrader,
            _skillDatas,
            _weaponDatas,
            _chapterDropDatas,
            _lifeDatas,
            _cardDatas,
            _interactableObjectDatas,
            _levelDatas,
            _coinGaugeData
        );

        fileIO.SaveData(database, fileLocation, fileName, true);
    }
}