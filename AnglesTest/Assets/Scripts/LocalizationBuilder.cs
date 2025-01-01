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

                    { ILocalization.Key.Buy, "Purchase" },
                    { ILocalization.Key.Equip, "Equip" },
                    { ILocalization.Key.Equipped, "Equipped" },

                    { ILocalization.Key.OutOfGold, "Insufficient gold." },
                    { ILocalization.Key.MaximumUpgradeStatus, "Maximum upgrade status reached." },

                    { ILocalization.Key.NormalSkinName, "Default" },
                    { ILocalization.Key.BloodEaterSkinName, "Blood Eater" },
                    { ILocalization.Key.GuardSkinName, "Guard" },

                    { ILocalization.Key.NormalSkinInfo, "Black Orb" },
                    { ILocalization.Key.BloodEaterSkinInfo, "10% lifesteal with a certain probability" },
                    { ILocalization.Key.GuardSkinInfo, "10% damage reduction" },

                    { ILocalization.Key.AttackDamageName, "Attack Power" },
                    { ILocalization.Key.MoveSpeedName, "Movement Speed" },
                    { ILocalization.Key.MaxHpName, "Max HP" },
                    { ILocalization.Key.DamageReductionName, "Damage Reduction" },

                    { ILocalization.Key.AttackDamageInfo, "Increases attack power by (R)" },
                    { ILocalization.Key.MoveSpeedInfo, "Increases movement speed by (R)" },
                    { ILocalization.Key.MaxHpInfo, "Increases max HP by (R)" },
                    { ILocalization.Key.DamageReductionInfo, "Reduces damage taken by (R)%" },

                    { ILocalization.Key.StatikkCardInfo, "Fires a laser that strikes nearby enemies." },
                    { ILocalization.Key.KnockbackCardInfo, "Attacks by pushing enemies far away." },

                    { ILocalization.Key.ImpactCardInfo, "Causes an explosion that knocks back and stuns enemies." },
                    { ILocalization.Key.SpawnRifleShooterCardInfo, "Summons a pet that shoots bullets." },
                    { ILocalization.Key.SpawnRocketShooterCardInfo, "Summons a pet that fires rockets." },
                    { ILocalization.Key.SpawnBlackholeCardInfo, "Creates a space that pulls in enemies." },
                    { ILocalization.Key.SpawnBladeCardInfo, "Fires a blade that bounces off walls and attacks." },
                    { ILocalization.Key.SpawnStickyBombCardInfo, "Attaches a bomb to enemies that explodes after a set time." },
                }
            },
            {
                ILocalization.Language.Korean,
                new Dictionary<ILocalization.Key, string>()
                {
                    { ILocalization.Key.Start, "����" },
                    { ILocalization.Key.End, "����" },

                    { ILocalization.Key.Buy, "�����ϱ�" },
                    { ILocalization.Key.Equip, "�����ϱ�" },
                    { ILocalization.Key.Equipped, "���� ��" },

                    { ILocalization.Key.OutOfGold, "��尡 �����մϴ�." },
                    { ILocalization.Key.MaximumUpgradeStatus, "�ִ� ���׷��̵� �����Դϴ�." },

                    { ILocalization.Key.NormalSkinName, "�⺻" },
                    { ILocalization.Key.BloodEaterSkinName, "���� ����" },
                    { ILocalization.Key.GuardSkinName, "����" },

                    { ILocalization.Key.NormalSkinInfo, "���� ��ü" },
                    { ILocalization.Key.BloodEaterSkinInfo, "���� Ȯ���� ���� 10%" },
                    { ILocalization.Key.GuardSkinInfo, "������ ���� 10%" },

                    { ILocalization.Key.AttackDamageName, "���ݷ�" },
                    { ILocalization.Key.MoveSpeedName, "�̵� �ӵ�" },
                    { ILocalization.Key.MaxHpName, "�ִ� ü��" },
                    { ILocalization.Key.DamageReductionName, "�޴� ���� ����" },

                    { ILocalization.Key.AttackDamageInfo, "���ݷ� (R) ����" },
                    { ILocalization.Key.MoveSpeedInfo, "�̵� �ӵ� (R) ����" },
                    { ILocalization.Key.MaxHpInfo, "ü�� (R) ����" },
                    { ILocalization.Key.DamageReductionInfo, "�޴� ���� ���� (R)%" },

                    { ILocalization.Key.StatikkCardInfo, "��ó�� ���� Ÿ���ϴ� �������� �߻��Ѵ�." },
                    { ILocalization.Key.KnockbackCardInfo, "���� �ָ� ���ĳ��� �����Ѵ�." },

                    { ILocalization.Key.ImpactCardInfo, "������ ������ ���� ƨ�ܳ��� ������Ų��." },
                    { ILocalization.Key.SpawnRifleShooterCardInfo, "ź�� �߻��ϴ� ���� ��ȯ�Ѵ�." },
                    { ILocalization.Key.SpawnRocketShooterCardInfo, "��ź�� �߻��ϴ� ���� ��ȯ�Ѵ�." },
                    { ILocalization.Key.SpawnBlackholeCardInfo, "���� ���Ƶ��̴� ������ �����Ѵ�." },
                    { ILocalization.Key.SpawnBladeCardInfo, "���� ƨ�ܴٴϸ� �����ϴ� Į���� �߻��Ѵ�." },
                    { ILocalization.Key.SpawnStickyBombCardInfo, "������ ��ź�� �ٿ� �����ð� ���� ���߽�Ų��." },
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
