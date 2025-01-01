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
                    { ILocalization.Key.Start, "시작" },
                    { ILocalization.Key.End, "종료" },

                    { ILocalization.Key.Buy, "구매하기" },
                    { ILocalization.Key.Equip, "장착하기" },
                    { ILocalization.Key.Equipped, "장착 중" },

                    { ILocalization.Key.OutOfGold, "골드가 부족합니다." },
                    { ILocalization.Key.MaximumUpgradeStatus, "최대 업그레이드 상태입니다." },

                    { ILocalization.Key.NormalSkinName, "기본" },
                    { ILocalization.Key.BloodEaterSkinName, "블러드 이터" },
                    { ILocalization.Key.GuardSkinName, "가드" },

                    { ILocalization.Key.NormalSkinInfo, "검은 구체" },
                    { ILocalization.Key.BloodEaterSkinInfo, "일정 확률로 흡혈 10%" },
                    { ILocalization.Key.GuardSkinInfo, "데미지 감소 10%" },

                    { ILocalization.Key.AttackDamageName, "공격력" },
                    { ILocalization.Key.MoveSpeedName, "이동 속도" },
                    { ILocalization.Key.MaxHpName, "최대 체력" },
                    { ILocalization.Key.DamageReductionName, "받는 피해 감소" },

                    { ILocalization.Key.AttackDamageInfo, "공격력 (R) 증가" },
                    { ILocalization.Key.MoveSpeedInfo, "이동 속도 (R) 증가" },
                    { ILocalization.Key.MaxHpInfo, "체력 (R) 증가" },
                    { ILocalization.Key.DamageReductionInfo, "받는 피해 감소 (R)%" },

                    { ILocalization.Key.StatikkCardInfo, "근처의 적을 타격하는 레이저를 발사한다." },
                    { ILocalization.Key.KnockbackCardInfo, "적을 멀리 밀쳐내며 공격한다." },

                    { ILocalization.Key.ImpactCardInfo, "폭발을 일으켜 적을 튕겨내고 경직시킨다." },
                    { ILocalization.Key.SpawnRifleShooterCardInfo, "탄을 발사하는 펫을 소환한다." },
                    { ILocalization.Key.SpawnRocketShooterCardInfo, "포탄을 발사하는 펫을 소환한다." },
                    { ILocalization.Key.SpawnBlackholeCardInfo, "적을 빨아들이는 공간을 생성한다." },
                    { ILocalization.Key.SpawnBladeCardInfo, "벽을 튕겨다니며 공격하는 칼날을 발사한다." },
                    { ILocalization.Key.SpawnStickyBombCardInfo, "적에게 폭탄을 붙여 일정시간 이후 폭발시킨다." },
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
