using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

public class Database
{
    const float _dropSpreadOffset = 2;
    public float DropSpreadOffset { get { return _dropSpreadOffset; } }

    const int _maxSkilCount = 8;
    public int MaxSkilCount { get { return _maxSkilCount; } }

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
    };

    public List<BaseSkill.Name> UpgradeableSkills { get { return _upgradeableSkills; } }


    Dictionary<BaseSkill.Name, BaseSkillData> _skillDatas = new Dictionary<BaseSkill.Name, BaseSkillData>
    {
        { BaseSkill.Name.Statikk, new StatikkData(5, 1, 1,
            
        new List<StatikkUpgradableData>
        {
            new StatikkUpgradableData(10, 3, 3),
            new StatikkUpgradableData(20, 3, 3),
            new StatikkUpgradableData(20, 3, 4),
            new StatikkUpgradableData(20, 3, 4),
            new StatikkUpgradableData(30, 3, 5)
        },
        new List<ITarget.Type> { ITarget.Type.Red }) },


        { BaseSkill.Name.Knockback, new KnockbackData(5, 3, 1,

             new List<KnockbackUpgradableData>
            {
                new KnockbackUpgradableData(20, 1),
                new KnockbackUpgradableData(20, 1.2f),
                new KnockbackUpgradableData(20, 1.2f),
                new KnockbackUpgradableData(20, 1.5f),
                new KnockbackUpgradableData(30, 2f)
            }, 
            new SerializableVector2(5.5f, 3), 
            new SerializableVector2(1.5f, 0), new List<ITarget.Type> { ITarget.Type.Red }) },


        { BaseSkill.Name.Impact, new ImpactData(5, 1, 
            
            new List<ImpactUpgradableData>
            {
                new ImpactUpgradableData(20, 1),
                new ImpactUpgradableData(30, 1.2f),
                new ImpactUpgradableData(40, 1.4f),
                new ImpactUpgradableData(50, 1.7f),
                new ImpactUpgradableData(60, 2f),
            }
            
            , new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnBlackhole, new SpawnBlackholeData(5, 1,

            new BlackholeData
            (
                new List<BlackholeUpgradableData>
                {
                    new BlackholeUpgradableData(3, 50, 4, 1),
                    new BlackholeUpgradableData(4, 50, 5, 1),
                    new BlackholeUpgradableData(4, 60, 5, 1.2f),
                    new BlackholeUpgradableData(5, 60, 6, 1.2f),
                    new BlackholeUpgradableData(6, 80, 7, 1.5f),
                }
                , 0.1f
            ),

            new List<ITarget.Type> { ITarget.Type.Red }) 
        },

        { BaseSkill.Name.SpawnRifleShooter, new SpawnShooterData(5,

            BaseWeapon.Name.RifleShooter,
            new ShooterData
            (
                new List<ShooterUpgradableData>
                {
                    new ShooterUpgradableData(10, 1),
                    new ShooterUpgradableData(10, 1),
                    new ShooterUpgradableData(10, 0.5f),
                    new ShooterUpgradableData(10, 0.5f),
                    new ShooterUpgradableData(10, 0.3f),
                },
                new BulletData(
                    new List<BulletUpgradableData>
                    {
                        new BulletUpgradableData(10),
                        new BulletUpgradableData(20),
                        new BulletUpgradableData(20),
                        new BulletUpgradableData(30),
                        new BulletUpgradableData(30),
                    },
                    5
                ),
                10, 4.0f, 10.0f, BaseWeapon.Name.Bullet
            ),

            new List<ITarget.Type> { ITarget.Type.Red }) },


       { BaseSkill.Name.SpawnRocketShooter, new SpawnShooterData(5,

            BaseWeapon.Name.RocketShooter,
            new ShooterData
            (
                new List<ShooterUpgradableData>
                {
                    new ShooterUpgradableData(8, 3),
                    new ShooterUpgradableData(8, 3),
                    new ShooterUpgradableData(8, 2.5f),
                    new ShooterUpgradableData(8, 2.5f),
                    new ShooterUpgradableData(8, 2),
                },
                new RocketData
                (
                    new List<RocketUpgradableData>
                    {
                        new RocketUpgradableData(20, 20, 5),
                        new RocketUpgradableData(30, 30, 5),
                        new RocketUpgradableData(40, 40, 5),
                        new RocketUpgradableData(50, 50, 5),
                        new RocketUpgradableData(60, 60, 5),
                    },
                    5
                ),
                10, 4.0f, 10.0f, BaseWeapon.Name.Rocket
            ),

            new List<ITarget.Type> { ITarget.Type.Red }) },


        { BaseSkill.Name.SpawnBlade, new SpawnBladeData(5, 1, 8f, 
            
            new BladeData(
                new List<BladeUpgradableData>
                {
                    new BladeUpgradableData(20, 1, 1),
                    new BladeUpgradableData(30, 1, 1),
                    new BladeUpgradableData(30, 1, 1.2f),
                    new BladeUpgradableData(40, 1, 1.2f),
                    new BladeUpgradableData(50, 1, 1.5f)
                },  
                5f
            ),

        new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpawnStickyBomb, new SpawnStickyBombData(
            5, 5f, 3, 

            new StickyBombData(
                new List<StickyBombUpgradableData>()
                {
                    new StickyBombUpgradableData(3, 1, 30),
                    new StickyBombUpgradableData(3, 1, 40),
                    new StickyBombUpgradableData(3, 1.2f, 50),
                    new StickyBombUpgradableData(3, 1.2f, 60),
                    new StickyBombUpgradableData(3, 1.5f, 70),
                }
            ),

        new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.ContactAttack, new ContactAttackData(1, 30, new List<ITarget.Type> { ITarget.Type.Red }) },

        { BaseSkill.Name.SpreadBullets, new SpreadBulletsData(1, 5f, 3f, 3f, 5, 

            new BulletData(
                
                new List<BulletUpgradableData>
                {
                    new BulletUpgradableData(10),
                    new BulletUpgradableData(10),
                    new BulletUpgradableData(10),
                    new BulletUpgradableData(10),
                    new BulletUpgradableData(10),
                },
                5f
            ),


            new List<ITarget.Type> { ITarget.Type.Blue })},



        { BaseSkill.Name.Shockwave, new ShockwaveData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.MagneticField, new MagneticFieldData(1, 20f, 5f, 1.5f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
        { BaseSkill.Name.SelfDestruction, new SelfDestructionData(1, 20f, 5f, 3f, new List<ITarget.Type>(){ITarget.Type.Blue}) },
    };

    public Dictionary<BaseSkill.Name, BaseSkillData> SkillDatas { get { return _skillDatas; } }


    Dictionary<BaseLife.Name, BaseLifeData> _lifeDatas = new Dictionary<BaseLife.Name, BaseLifeData>
    {
        { 
            BaseLife.Name.Player, new PlayerData(100, ITarget.Type.Blue, 10, 2, 3, 15, 0.5f, 15, 2f, 0.2f, 3, 1, 1.5f, 0.15f, 0.3f, 
            new List<BaseSkill.Name> { BaseSkill.Name.ContactAttack })
        },

        { 
            BaseLife.Name.Triangle, new TriangleData(5, ITarget.Type.Red, new List<BaseSkill.Name> { BaseSkill.Name.MagneticField }, 
            new DropData(3, 
                new List<Tuple<IInteractable.Name, float>>
                { 
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5) 
        },

        { 
            BaseLife.Name.Rectangle, new RectangleData(10, ITarget.Type.Red, new List<BaseSkill.Name> { BaseSkill.Name.MagneticField },
            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5)
        },

        { 
            BaseLife.Name.Pentagon, new PentagonData(20, ITarget.Type.Red, new List<BaseSkill.Name> { BaseSkill.Name.SpreadBullets },
            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 4f, 2f)
        },

        { 
            BaseLife.Name.Hexagon, new HexagonData(40, ITarget.Type.Red, new List<BaseSkill.Name> { BaseSkill.Name.Shockwave },
            new DropData(3,
                new List<Tuple<IInteractable.Name, float>>
                {
                    new Tuple<IInteractable.Name, float>( IInteractable.Name.Coin, 0.3f)
                }
            ), 5f, 4f, 2f)
        },

    };
    public Dictionary<BaseLife.Name, BaseLifeData> LifeDatas { get { return _lifeDatas; } }


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
    };
    public Dictionary<BaseSkill.Name, CardInfoData> CardDatas { get { return _cardDatas; } }


    Dictionary<IInteractable.Name, BaseInteractableObjectData> _interactableObjectDatas = new Dictionary<IInteractable.Name, BaseInteractableObjectData>
    {
        { IInteractable.Name.CardTable, new CardTableData(3) },
        { IInteractable.Name.Coin, new CoinData(5, 25) },
        { IInteractable.Name.SkillBubble, new SkillBubbleData(1, 25) },
        { IInteractable.Name.Shop, new ShopData(3, 3) }
    };

    public Dictionary<IInteractable.Name, BaseInteractableObjectData> InteractableObjectDatas { get { return _interactableObjectDatas; } }
}
