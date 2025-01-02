using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalizationBuilder : MonoBehaviour
{
    [JsonProperty]
    Dictionary<ILocalization.Language, Dictionary<ILocalization.Key, string>> _word
        = new Dictionary<ILocalization.Language, Dictionary<ILocalization.Key, string>>()
        {
            {
                ILocalization.Language.English,
                new Dictionary<ILocalization.Key, string>()
                {
                    { ILocalization.Key.Start, "Start" },
                    { ILocalization.Key.End, "End" },
                    { ILocalization.Key.Setting, "Setting" },


                    { ILocalization.Key.StageCount, "Stage Count" },
                    { ILocalization.Key.SurvivalTime, "Survival Time" },
                    { ILocalization.Key.Progress, "Progress" },


                    { ILocalization.Key.StageClear, "Stage Clear" },
                    { ILocalization.Key.BossClear, "Boss Clear" },
                    { ILocalization.Key.BossIncoming, "Boss Incoming!" },



                    { ILocalization.Key.RecordTime, "Record" },
                    { ILocalization.Key.TabToContinue, "Tab To Continue" },



                    { ILocalization.Key.LeftCount, "Left Count" },
                    { ILocalization.Key.PickAgain, "Pick Again" },



                    { ILocalization.Key.BGMMenu, "BGM" },
                    { ILocalization.Key.SFXMenu, "SFX" },
                    { ILocalization.Key.ExitMenu, "Exit" },
                    { ILocalization.Key.ResumeMenu, "Resume" },

                    { ILocalization.Key.Upgrade, "Upgrade" },

                    { ILocalization.Key.Buy, "Purchase" },
                    { ILocalization.Key.Equip, "Equip" },
                    { ILocalization.Key.Equipped, "Equipped" },

                    { ILocalization.Key.OutOfGold, "Insufficient gold." },
                    { ILocalization.Key.MaximumUpgradeStatus, "Maximum upgrade status reached." },


                    { ILocalization.Key.TriconChapterName, "Tricon" },
                    { ILocalization.Key.RhombusChapterName, "Rhombus" },
                    { ILocalization.Key.PentagonicChapterName, "Pentagonic" },
                    { ILocalization.Key.HexahornChapterName, "Hexahorn" },
                    { ILocalization.Key.OctaviaChapterName, "Octavia" },
                    { ILocalization.Key.PyramidSurvivalName, "Pyramid" },
                    { ILocalization.Key.CubeSurvivalName, "Cube" },
                    { ILocalization.Key.PrismSurvivalName, "Prism" },


                    { ILocalization.Key.TriconChapterDescription, "Caution: Charge!" },
                    { ILocalization.Key.RhombusChapterDescription, "Watch out for the charge!" },
                    { ILocalization.Key.PentagonicChapterDescription, "Revolving bullet" },
                    { ILocalization.Key.HexahornChapterDescription, "One more round!" },
                    { ILocalization.Key.OctaviaChapterDescription, "Random laser Incoming!" },

                    { ILocalization.Key.PyramidSurvivalDescription, "Light Challenge" },
                    { ILocalization.Key.CubeSurvivalDescription, "Challenge of experts" },
                    { ILocalization.Key.PrismSurvivalDescription, "Series of hardships" },



                    { ILocalization.Key.NormalSkinName, "Orb" },
                    { ILocalization.Key.BloodEaterSkinName, "Blood Eater" },
                    { ILocalization.Key.GuardSkinName, "Guard" },

                    { ILocalization.Key.NormalSkinDescription, "Black Orb" },
                    { ILocalization.Key.BloodEaterSkinDescription, "10% lifesteal with a certain probability" },
                    { ILocalization.Key.GuardSkinDescription, "10% damage reduction" },

                    { ILocalization.Key.AttackDamageName, "Attack Damage" },
                    { ILocalization.Key.MoveSpeedName, "Movement Speed" },
                    { ILocalization.Key.MaxHpName, "Max HP" },
                    { ILocalization.Key.DamageReductionName, "Damage Reduction" },

                    { ILocalization.Key.AttackDamageDescription0, "" },
                    { ILocalization.Key.AttackDamageDescription1, "Increases damage by 5" },
                    { ILocalization.Key.AttackDamageDescription2, "Increases damage by 10" },
                    { ILocalization.Key.AttackDamageDescription3, "Increases damage by 15" },
                    { ILocalization.Key.AttackDamageDescription4, "Increases damage by 20" },
                    { ILocalization.Key.AttackDamageDescription5, "Increases damage by 30" },

                    { ILocalization.Key.MoveSpeedDescription0, "" },
                    { ILocalization.Key.MoveSpeedDescription1, "Increases movement speed by 1" },
                    { ILocalization.Key.MoveSpeedDescription2, "Increases movement speed by 2" },
                    { ILocalization.Key.MoveSpeedDescription3, "Increases movement speed by 3" },


                    { ILocalization.Key.MaxHpDescription0, "" },
                    { ILocalization.Key.MaxHpDescription1, "Increases max HP by 10" },
                    { ILocalization.Key.MaxHpDescription2, "Increases max HP by 15" },
                    { ILocalization.Key.MaxHpDescription3, "Increases max HP by 25" },
                    { ILocalization.Key.MaxHpDescription4, "Increases max HP by 35" },
                    { ILocalization.Key.MaxHpDescription5, "Increases max HP by 50" },


                    { ILocalization.Key.DamageReductionDescription0, "" },
                    { ILocalization.Key.DamageReductionDescription1, "Reduces received damage by 5%" },
                    { ILocalization.Key.DamageReductionDescription2, "Reduces received damage by 10%" },
                    { ILocalization.Key.DamageReductionDescription3, "Reduces received damage by 15%" },
                    { ILocalization.Key.DamageReductionDescription4, "Reduces received damage by 25%" },
                    { ILocalization.Key.DamageReductionDescription5, "Reduces received damage by 35%" },

                    { ILocalization.Key.StatikkCardName, "Guided Laser" },
                    { ILocalization.Key.KnockbackCardName, "Knockback" },
                    { ILocalization.Key.ImpactCardName, "Shockwave" },
                    { ILocalization.Key.UpgradeShootingCardName, "Shooting Upgrade" },
                    { ILocalization.Key.UpgradeDamageCardName, "Final Damage Increase" },
                    { ILocalization.Key.UpgradeCooltimeCardName, "Skill Cooldown Reduction" },
                    { ILocalization.Key.SpawnRifleShooterCardName, "Pet: Shooter" },
                    { ILocalization.Key.SpawnRocketShooterCardName, "Pet: Bomber" },
                    { ILocalization.Key.SpawnBlackholeCardName, "Blackhole" },
                    { ILocalization.Key.SpawnBladeCardName, "Blade" },
                    { ILocalization.Key.SpawnStickyBombCardName, "Timed Bomb" },

                    { ILocalization.Key.StatikkCardDescription, "Shoot Laser that hits nearby enemies." },
                    { ILocalization.Key.KnockbackCardDescription, "Attack and pushes the enemy away." },
                    { ILocalization.Key.ImpactCardDescription, "Randomly make Explosion and stun the enemies." },


                    { ILocalization.Key.UpgradeShootingCardDescription, "Reduces the charging time of shooting and increases invincibility time." },
                    { ILocalization.Key.UpgradeDamageCardDescription, "Increases final damage." },
                    { ILocalization.Key.UpgradeCooltimeCardDescription, "Reduces the cooldown time of cooldown skills." },

                    { ILocalization.Key.SpawnRifleShooterCardDescription, "Spawn a pet that fires buttlets." },
                    { ILocalization.Key.SpawnRocketShooterCardDescription, "Spawn a pet that fires cannonballs." },
                    { ILocalization.Key.SpawnBlackholeCardDescription, "Randomly create a space that sucks in enemies." },
                    { ILocalization.Key.SpawnBladeCardDescription, "Randomly shoot a bouncing blade that attacks enemies." },
                    { ILocalization.Key.SpawnStickyBombCardDescription, "Attach a bomb to an enemy and explode it after few seconds." },
                }
            },
            {
                ILocalization.Language.Korean,
                new Dictionary<ILocalization.Key, string>()
                {
                    { ILocalization.Key.Start, "����" },
                    { ILocalization.Key.End, "����" },
                    { ILocalization.Key.Setting, "����" },

                    { ILocalization.Key.StageCount, "�������� ��" },
                    { ILocalization.Key.SurvivalTime, "���� �ð�" },
                    { ILocalization.Key.Progress, "����" },


                    { ILocalization.Key.StageClear, "�������� Ŭ����" },
                    { ILocalization.Key.BossClear, "���� Ŭ����" },
                    { ILocalization.Key.BossIncoming, "���� ����!" },


                    { ILocalization.Key.RecordTime, "���� �ð�" },
                    { ILocalization.Key.TabToContinue, "���ؼ� ������" },


                    { ILocalization.Key.LeftCount, "���� Ƚ��" },
                    { ILocalization.Key.PickAgain, "�ٽ� �̱�" },

                    { ILocalization.Key.BGMMenu, "�����" },
                    { ILocalization.Key.SFXMenu, "ȿ����" },
                    { ILocalization.Key.ExitMenu, "������" },
                    { ILocalization.Key.ResumeMenu, "�ݱ�" },

                    { ILocalization.Key.Upgrade, "���׷��̵�" },

                    { ILocalization.Key.Buy, "�����ϱ�" },
                    { ILocalization.Key.Equip, "�����ϱ�" },
                    { ILocalization.Key.Equipped, "���� ��" },

                    { ILocalization.Key.OutOfGold, "��尡 �����մϴ�." },
                    { ILocalization.Key.MaximumUpgradeStatus, "�ִ� ���׷��̵� �����Դϴ�." },


                    { ILocalization.Key.TriconChapterName, "Tricon" },
                    { ILocalization.Key.RhombusChapterName, "Rhombus" },
                    { ILocalization.Key.PentagonicChapterName, "Pentagonic" },
                    { ILocalization.Key.HexahornChapterName, "Hexahorn" },
                    { ILocalization.Key.OctaviaChapterName, "Octavia" },
                    { ILocalization.Key.PyramidSurvivalName, "Pyramid" },
                    { ILocalization.Key.CubeSurvivalName, "Cube" },
                    { ILocalization.Key.PrismSurvivalName, "Prism" },



                    { ILocalization.Key.TriconChapterDescription, "���� ����!" },
                    { ILocalization.Key.RhombusChapterDescription, "�ް� �ѹ� ��" },
                    { ILocalization.Key.PentagonicChapterDescription, "���� ���� źȯ" },
                    { ILocalization.Key.HexahornChapterDescription, "���� ��!" },
                    { ILocalization.Key.OctaviaChapterDescription, "������ ������" },

                    { ILocalization.Key.PyramidSurvivalDescription, "������ ����" },
                    { ILocalization.Key.CubeSurvivalDescription, "�Ƿ����� ����" },
                    { ILocalization.Key.PrismSurvivalDescription, "���� ����" },



                    { ILocalization.Key.NormalSkinName, "��ü" },
                    { ILocalization.Key.BloodEaterSkinName, "���� ����" },
                    { ILocalization.Key.GuardSkinName, "����" },

                    { ILocalization.Key.NormalSkinDescription, "���� ��ü" },
                    { ILocalization.Key.BloodEaterSkinDescription, "���� Ȯ���� ���� 10%" },
                    { ILocalization.Key.GuardSkinDescription, "������ ���� 10%" },

                    { ILocalization.Key.AttackDamageName, "���ݷ�" },
                    { ILocalization.Key.MoveSpeedName, "�̵� �ӵ�" },
                    { ILocalization.Key.MaxHpName, "�ִ� ü��" },
                    { ILocalization.Key.DamageReductionName, "�޴� ���� ����" },

                    { ILocalization.Key.AttackDamageDescription0, "" },
                    { ILocalization.Key.AttackDamageDescription1, "���ݷ� 5 ����" },
                    { ILocalization.Key.AttackDamageDescription2, "���ݷ� 10 ����" },
                    { ILocalization.Key.AttackDamageDescription3, "���ݷ� 15 ����" },
                    { ILocalization.Key.AttackDamageDescription4, "���ݷ� 20 ����" },
                    { ILocalization.Key.AttackDamageDescription5, "���ݷ� 30 ����" },

                    { ILocalization.Key.MoveSpeedDescription0, "" },
                    { ILocalization.Key.MoveSpeedDescription1, "�̵� �ӵ� 1 ����" },
                    { ILocalization.Key.MoveSpeedDescription2, "�̵� �ӵ� 2 ����" },
                    { ILocalization.Key.MoveSpeedDescription3, "�̵� �ӵ� 3 ����" },

                    { ILocalization.Key.MaxHpDescription0, "" },
                    { ILocalization.Key.MaxHpDescription1, "ü�� 10 ����" },
                    { ILocalization.Key.MaxHpDescription2, "ü�� 15 ����" },
                    { ILocalization.Key.MaxHpDescription3, "ü�� 25 ����" },
                    { ILocalization.Key.MaxHpDescription4, "ü�� 35 ����" },
                    { ILocalization.Key.MaxHpDescription5, "ü�� 50 ����" },

                    { ILocalization.Key.DamageReductionDescription0, "" },
                    { ILocalization.Key.DamageReductionDescription1, "�޴� ���� ���� 5%" },
                    { ILocalization.Key.DamageReductionDescription2, "�޴� ���� ���� 10%" },
                    { ILocalization.Key.DamageReductionDescription3, "�޴� ���� ���� 15%" },
                    { ILocalization.Key.DamageReductionDescription4, "�޴� ���� ���� 20%" },
                    { ILocalization.Key.DamageReductionDescription5, "�޴� ���� ���� 30%" },

                    { ILocalization.Key.StatikkCardName, "���� ������" },
                    { ILocalization.Key.KnockbackCardName, "�˹�" },
                    { ILocalization.Key.ImpactCardName, "�����" },

                    { ILocalization.Key.UpgradeShootingCardName, "���� ��ȭ" },
                    { ILocalization.Key.UpgradeDamageCardName, "���� ������ ����" },
                    { ILocalization.Key.UpgradeCooltimeCardName, "��ų ��Ÿ�� ����" },

                    { ILocalization.Key.SpawnRifleShooterCardName, "��:����" },
                    { ILocalization.Key.SpawnRocketShooterCardName, "��:�չ�" },
                    { ILocalization.Key.SpawnBlackholeCardName, "��Ȧ" },
                    { ILocalization.Key.SpawnBladeCardName, "���̵�" },
                    { ILocalization.Key.SpawnStickyBombCardName, "���� ��ź" },

                    { ILocalization.Key.StatikkCardDescription, "��ó�� ���� Ÿ���ϴ� �������� �߻��Ѵ�." },
                    { ILocalization.Key.KnockbackCardDescription, "���� �ָ� ���ĳ��� �����Ѵ�." },
                    { ILocalization.Key.ImpactCardDescription, "������ ������ ���� ƨ�ܳ��� ������Ų��." },

                    { ILocalization.Key.UpgradeShootingCardDescription, "������ ��¡ �ӵ��� ���ҽ�Ű�� �����ð��� ������Ų��." },
                    { ILocalization.Key.UpgradeDamageCardDescription, "���� �������� ��½�Ų��." },
                    { ILocalization.Key.UpgradeCooltimeCardDescription, "��Ÿ�� ��ų�� ���� ���ð��� ���ҽ�Ų��." },


                    { ILocalization.Key.SpawnRifleShooterCardDescription, "ź�� �߻��ϴ� ���� ��ȯ�Ѵ�." },
                    { ILocalization.Key.SpawnRocketShooterCardDescription, "��ź�� �߻��ϴ� ���� ��ȯ�Ѵ�." },
                    { ILocalization.Key.SpawnBlackholeCardDescription, "���� ���Ƶ��̴� ������ �����Ѵ�." },
                    { ILocalization.Key.SpawnBladeCardDescription, "���� ƨ�ܴٴϸ� �����ϴ� Į���� �߻��Ѵ�." },
                    { ILocalization.Key.SpawnStickyBombCardDescription, "������ ��ź�� �ٿ� �����ð� ���� ���߽�Ų��." },
                }
            }
        };


    [ContextMenu("CreateData")]
    public void CreateData()
    {
        FileIO fileIO = new FileIO(new JsonParser(), ".txt");

        string fileName = "LocalizationAsset";
        string fileLocation = "JsonData";

        Localization localization = new Localization(_word);
        fileIO.SaveData(localization, fileLocation, fileName, true);
    }
}
