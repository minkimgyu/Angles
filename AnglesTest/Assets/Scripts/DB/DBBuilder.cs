using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skill;

public class DBBuilder : MonoBehaviour
{
    #region SKIN 데이터

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

    #region STAT 데이터

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

    #region SKILL 데이터

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

    HashSet<BaseSkill.Name> _tutorialUpgradeableSkills = new HashSet<BaseSkill.Name>
    {
        BaseSkill.Name.Statikk,
        BaseSkill.Name.Knockback,
        BaseSkill.Name.SpawnRifleShooter,
    };

    Dictionary<BaseSkill.Name, IUpgradeVisitor> _skillUpgrader = new Dictionary<BaseSkill.Name, IUpgradeVisitor>
    {
        {
            BaseSkill.Name.Statikk,
            new StatikkUpgrader
            (
                new List<StatikkUpgrader.UpgradableData>
                {
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0), // 2 될 떄
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1), // 3 될 떄
                    new StatikkUpgrader.UpgradableData(10, 0, 0, 0), // 4 될 떄
                    new StatikkUpgrader.UpgradableData(0, 0, 1, 1), // 5 될 떄
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

         // 기본 스텟: 4
         // 추가 업그레이드 수치: 1

         // 특수 사각형(빨간색) --> LV.2, 일반 사각형(노란색) --> LV.1
         // 데미지, 발사 속도
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
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1, 10, 1, 1f, 3, 3, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1, 20, 1, 1, 3, new SerializableVector2(5.5f, 3), new SerializableVector2(1.5f, 0), 20, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.Impact, new ImpactData(5, 0.2f, 20, 1, 1, 2, 1, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ReviveImpact, new ReviveImpactData(5, 2, DamageUtility.Damage.InstantDeathDamage, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 0.1f, 0, 1, 0.3f, 3f, 1f, new List<ITarget.Type> { ITarget.Type.Red })},

        { BaseSkill.Name.SpawnRifleShooter, new SpawnShooterData(5, BaseWeapon.Name.RifleShooter, 1, 0.6f, 0, 0.1f, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnRocketShooter, new SpawnShooterData(5, BaseWeapon.Name.RocketShooter, 5, 3f, 0, 0.5f, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 0.3f, 10, 1, 1, 3f, 1f, 8f, new List<ITarget.Type> { ITarget.Type.Red }) },

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

        { BaseSkill.Name.ShootMultipleLaser, new ShootMultipleLaserData(0, 20f, 1f, 30f, 1f, 8, 4, 0, 2f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MultipleShockwave, new MultipleShockwaveData(0, 0.5f, 0.7f, 3, 30f, 1f, 1.7f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SpreadMultipleBullets, new SpreadMultipleBulletsData(0, 0.3f, 4, 10, 1, 0, 2f, 4f, BaseWeapon.Name.PentagonicBullet, 5f, 1f, new List<ITarget.Type> { ITarget.Type.Blue }) },
        {
            BaseSkill.Name.SpreadBullets,
            new SpreadBulletsData(
                3,
                10,
                1,
                BaseWeapon.Name.PentagonBullet,
                4f,
                4f,
                0,
                5f,
                1f,
                new List<ITarget.Type> { ITarget.Type.Blue })
        },
        {
            BaseSkill.Name.SpreadReflectableBullets,
            new SpreadBulletsData(
                1,
                10,
                1,
                BaseWeapon.Name.HexahornBullet,
                1f,
                4f,
                0,
                6f,
                1f,
                new List<ITarget.Type> { ITarget.Type.Blue })
        },
        {
            BaseSkill.Name.SpreadTrackableMissiles,
            new SpreadTrackableMissilesData(
                1,
                30,
                1,
                3,
                BaseWeapon.Name.TrackableMissile,
                6,
                1f,
                new List<ITarget.Type> { ITarget.Type.Blue })
        },

        { BaseSkill.Name.RushAttack, new ContactAttackData(0, 20, 1, 0, new List<ITarget.Type> { ITarget.Type.Blue }) },
        { BaseSkill.Name.Shockwave, new ShockwaveData(3, 30f, 1f, 5f, 1f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(3, 2f, 1f, 0.5f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(3, 20f, 1f, 5f, 1f, 3f, 0.7f, new List<ITarget.Type>(){ITarget.Type.Red, ITarget.Type.Blue}) },
        {
            BaseSkill.Name.ShootFewLaser,
            new ShootMultipleLaserData(
                0,
                20f,
                1f,
                30f,
                1f,
                8,
                3,
                0,
                1.5f,
                new List<ITarget.Type>(){ITarget.Type.Blue})
        },
    };

    #endregion

    #region WEAPON 데이터
    Dictionary<BaseWeapon.Name, WeaponData> _weaponDatas = new Dictionary<BaseWeapon.Name, WeaponData>
    {
        { BaseWeapon.Name.Blade, new BladeData(1, 1)},

        { BaseWeapon.Name.PentagonBullet, new BulletData(5)},
        { BaseWeapon.Name.PentagonicBullet, new BulletData(5)},
        { BaseWeapon.Name.HexahornBullet, new BulletData(5)},

        { BaseWeapon.Name.ShooterBullet, new BulletData(5)},
        { BaseWeapon.Name.ShooterRocket, new RocketData(3, 5)},

        { BaseWeapon.Name.Blackhole, new BlackholeData(-10, 0.1f)},
        { BaseWeapon.Name.StickyBomb, new StickyBombData(3, 3)},

        { BaseWeapon.Name.RifleShooter, new ShooterData(10, 1, 18, 1.5f, new SerializableVector2(1, 1), 10.0f)},
        { BaseWeapon.Name.RocketShooter, new ShooterData(10, 1, 18, 1.5f, new SerializableVector2(-1, 1),10.0f)},

        { BaseWeapon.Name.TrackableMissile, new TrackableMissileData(5, 15)},

    };

    #endregion

    #region DROP 데이터

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
            BaseLife.Name.GreenTriangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.GreenRectangle,
            new DropData(
                3,
                new Dictionary<IInteractable.Name, float>
                {
                    { IInteractable.Name.Coin, 0.3f }
                }
            )
        },
        {
            BaseLife.Name.GreenPentagon,
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
            BaseLife.Name.GreenHexagon,
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
        },
        {
            BaseLife.Name.Hexatric,
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

    #region LIFE 데이터

    Dictionary<BaseLife.Name, LifeData> _lifeDatas = new Dictionary<BaseLife.Name, LifeData>
    {
        {
            BaseLife.Name.Player, new PlayerData(
                new UpgradeableStat<float>(100),
                ITarget.Type.Blue,
                BaseEffect.Name.HexagonDestroyEffect,
                new UpgradeableStat < float >(0f),
                new UpgradeableStat < float >(0f),

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
            new List<BaseSkill.Name> { BaseSkill.Name.ContactAttack, BaseSkill.Name.ReviveImpact })
        },

        {
            BaseLife.Name.YellowTriangle, new TriangleData(new UpgradeableStat < float >(10), ITarget.Type.Red, BaseEffect.Name.TriangleDestroyEffect, BaseLife.Size.Small,

            new Dictionary<BaseSkill.Name, int>{{BaseSkill.Name.MagneticField, 0 }}, 8)
        },

        {
            BaseLife.Name.YellowRectangle, new RectangleData(new UpgradeableStat < float >(20), ITarget.Type.Red, BaseEffect.Name.RectangleDestroyEffect, BaseLife.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 0 }
            }, 6)
        },

        {
            BaseLife.Name.YellowPentagon, new PentagonData(new UpgradeableStat < float >(40), ITarget.Type.Red, BaseEffect.Name.PentagonDestroyEffect, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 0 }
            }, 4f, 3f, 1f)
        },

        {
            BaseLife.Name.YellowHexagon, new HexagonData(new UpgradeableStat < float >(60), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseLife.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 0 }
            }, 4f, 3f, 1f)
        },



        // 스킬 업그레이드 수정
        {
            BaseLife.Name.RedTriangle, new TriangleData(new UpgradeableStat < float >(15), ITarget.Type.Red, BaseEffect.Name.TriangleDestroyEffect, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 }
            }, 11)
        },

        {
            BaseLife.Name.RedRectangle, new RectangleData(new UpgradeableStat < float >(30), ITarget.Type.Red, BaseEffect.Name.RectangleDestroyEffect, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 },
                { BaseSkill.Name.SelfDestruction, 0 }
            }, 6)
        },

        {
            BaseLife.Name.RedPentagon, new PentagonData(new UpgradeableStat < float >(75), ITarget.Type.Red, BaseEffect.Name.PentagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 1 }
            }, 4f, 3f, 1f)
        },

        {
            BaseLife.Name.RedHexagon, new HexagonData(new UpgradeableStat < float >(90), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 1 }
            }, 6f, 3f, 1f)
        },



        {
            BaseLife.Name.OperaTriangle, new TriangleData(new UpgradeableStat < float >(25), ITarget.Type.Red, BaseEffect.Name.TriangleDestroyEffect, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            }, 5)
        },

        {
            BaseLife.Name.OperaRectangle, new RectangleData(new UpgradeableStat < float >(45), ITarget.Type.Red, BaseEffect.Name.RectangleDestroyEffect, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 1 },
            }, 9)
        },

        {
            BaseLife.Name.OperaPentagon, new PentagonData(new UpgradeableStat < float >(100), ITarget.Type.Red, BaseEffect.Name.PentagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadBullets, 2 }
            }, 3f, 3f, 1f)
        },

        {
            BaseLife.Name.OperaHexagon, new OperaHexagonData(new UpgradeableStat < float >(120), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 2 }
            }, 9f, 3f, 2f, 2f, 2f, 1f)
        },


        {
            BaseLife.Name.GreenTriangle, new TriangleData(new UpgradeableStat < float >(40), ITarget.Type.Red, BaseEffect.Name.TriangleDestroyEffect, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            }, 5)
        },

        {
            BaseLife.Name.GreenRectangle, new RectangleData(new UpgradeableStat < float >(70), ITarget.Type.Red, BaseEffect.Name.RectangleDestroyEffect, BaseEnemy.Size.Small,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.Shockwave, 0 },
            }, 9)
        },

        {
            BaseLife.Name.GreenPentagon, new GreenPentagonData(new UpgradeableStat < float >(140), ITarget.Type.Red, BaseEffect.Name.PentagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.RushAttack, 0 }
            }, 30f, 3f, 5f)
        },

        {
            BaseLife.Name.GreenHexagon, new HexagonData(new UpgradeableStat < float >(180), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.ShootFewLaser, 0 }
            }, 9f, 3f, 1f)
        },



         {
            BaseLife.Name.Tricon, new TriconData(new UpgradeableStat < float >(300), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MagneticField, 2 }
            }, 15f, 3f, 1f, 2f, 4f)
        },

         {
            BaseLife.Name.Rhombus, new RhombusData(new UpgradeableStat < float >(400), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.MultipleShockwave, 0 },
                { BaseSkill.Name.MagneticField, 2 }
            }, 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Pentagonic, new PentagonicData(new UpgradeableStat < float >(500), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadMultipleBullets, 0 },
            }, 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Hexahorn, new HexahornData(new UpgradeableStat < float >(600), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadReflectableBullets, 0 },
            }, 6f, 3f, 1f)
        },

         {
            BaseLife.Name.Octavia, new OctaviaData(new UpgradeableStat < float >(800), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.ShootMultipleLaser, 0 },
            }, 6f, 3f, 1f)
        },

          {
            BaseLife.Name.Hexatric, new HexatricData(new UpgradeableStat < float >(800), ITarget.Type.Red, BaseEffect.Name.HexagonDestroyEffect, BaseEnemy.Size.Medium,

            new Dictionary<BaseSkill.Name, int>
            {
                { BaseSkill.Name.SpreadTrackableMissiles, 0 },
            }, 6f, 3f, 1f)
        }

    };

    #endregion

    #region CARD 데이터
    Dictionary<BaseSkill.Name, CardInfoData> _cardDatas = new Dictionary<BaseSkill.Name, CardInfoData>
    {
        { BaseSkill.Name.SpawnBlackhole, new CardInfoData(30) },
        { BaseSkill.Name.SpawnBlade, new CardInfoData(30) },
        { BaseSkill.Name.Impact, new CardInfoData(30) },
        { BaseSkill.Name.Knockback, new CardInfoData(30) },

        { BaseSkill.Name.SpawnRifleShooter, new CardInfoData(20) },
        { BaseSkill.Name.SpawnRocketShooter, new CardInfoData(20) },

        { BaseSkill.Name.Statikk, new CardInfoData(30) },
        { BaseSkill.Name.SpawnStickyBomb, new CardInfoData(20) },

        { BaseSkill.Name.UpgradeShooting, new CardInfoData(10) },
        { BaseSkill.Name.UpgradeDamage, new CardInfoData(10) },
        { BaseSkill.Name.UpgradeCooltime, new CardInfoData(10) },
    };

    #endregion

    #region INTERACTABLE 데이터

    Dictionary<IInteractable.Name, BaseInteractableObjectData> _interactableObjectDatas = new Dictionary<IInteractable.Name, BaseInteractableObjectData>
    {
        { IInteractable.Name.CardTable, new CardTableData(3, 1) },
        { IInteractable.Name.Coin, new CoinData(5, 25) },
        { IInteractable.Name.Heart, new HeartData(20, 25) },
        { IInteractable.Name.SkillBubble, new SkillBubbleData(1, 25) },
        { IInteractable.Name.Shop, new ShopData(3, 2) }
    };

    #endregion

    #region LEVEL 데이터

    Dictionary<GameMode.Level, ILevelInfo> _levelDatas = new Dictionary<GameMode.Level, ILevelInfo>()
    {
        { GameMode.Level.TriconChapter, new ChapterInfo(20, GameMode.Level.RhombusChapter) },
        { GameMode.Level.RhombusChapter, new ChapterInfo(20, GameMode.Level.PentagonicChapter) },
        { GameMode.Level.PentagonicChapter, new ChapterInfo(20, GameMode.Level.HexahornChapter) },
        { GameMode.Level.HexahornChapter, new ChapterInfo(20, GameMode.Level.OctaviaChapter) },
        { GameMode.Level.OctaviaChapter, new ChapterInfo(20, GameMode.Level.HexatricChapter) },
        { GameMode.Level.HexatricChapter, new ChapterInfo(30, GameMode.Level.GearChapter) },
        { GameMode.Level.GearChapter, new ChapterInfo(30) },

        { GameMode.Level.PyramidSurvival, new SurvivalInfo(300, GameMode.Level.CubeSurvival) },
        { GameMode.Level.CubeSurvival, new SurvivalInfo(300, GameMode.Level.PrismSurvival) },
        { GameMode.Level.PrismSurvival, new SurvivalInfo(300, GameMode.Level.DodecaSurvival) },
        { GameMode.Level.DodecaSurvival, new SurvivalInfo(300, GameMode.Level.IcosaSurvival) },
        { GameMode.Level.IcosaSurvival, new SurvivalInfo(300) },

        { GameMode.Level.MainTutorial, new TutorialInfo(1) },
    };

    #endregion

    #region AD 데이터

    const int _lobbyAdCoinCount = 100;
    const string _lobbyAdSaveKeyName = "lobbyAd";
    const int _lobbyAdDelay = 30; // 수정 필요

    const string _inGameAdSaveKeyName = "inGameAd";
    const int _inGameAdDelay = 30; // 수정 필요

    AdData _adData = new AdData(
                        _lobbyAdCoinCount,
                        _lobbyAdSaveKeyName,
                        _lobbyAdDelay,
                        _inGameAdSaveKeyName,
                        _inGameAdDelay);

    #endregion

    #region CoinGauge 데이터

    List<int> _coinGaugeData = new List<int>() { 50, 100, 150, 200, 250, 300, 350, 400, 500, 600 }; // 100랩 까지

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
            _skinModifiers,
            _skinDatas,
            _statModifiers,
            _statDatas,

            _upgradeableSkills,
            _tutorialUpgradeableSkills,

            _skillUpgrader,
            _skillDatas,
            _weaponDatas,
            _chapterDropDatas,
            _lifeDatas,
            _cardDatas,
            _interactableObjectDatas,
            _levelDatas,
            _coinGaugeData,
            _adData
        );

        fileIO.SaveData(database, fileLocation, fileName, true);
    }
}