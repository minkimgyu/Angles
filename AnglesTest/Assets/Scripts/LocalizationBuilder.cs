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
                    { ILocalization.Key.Start, "시작" },
                    { ILocalization.Key.End, "종료" },
                    { ILocalization.Key.Setting, "설정" },

                    { ILocalization.Key.StageCount, "스테이지 수" },
                    { ILocalization.Key.SurvivalTime, "생존 시간" },
                    { ILocalization.Key.Progress, "진행" },


                    { ILocalization.Key.StageClear, "스테이지 클리어" },
                    { ILocalization.Key.BossClear, "보스 클리어" },
                    { ILocalization.Key.BossIncoming, "보스 등장!" },


                    { ILocalization.Key.RecordTime, "생존 시간" },
                    { ILocalization.Key.TabToContinue, "탭해서 나가기" },


                    { ILocalization.Key.LeftCount, "남은 횟수" },
                    { ILocalization.Key.PickAgain, "다시 뽑기" },

                    { ILocalization.Key.BGMMenu, "배경음" },
                    { ILocalization.Key.SFXMenu, "효과음" },
                    { ILocalization.Key.ExitMenu, "나가기" },
                    { ILocalization.Key.ResumeMenu, "닫기" },

                    { ILocalization.Key.Upgrade, "업그레이드" },

                    { ILocalization.Key.Buy, "구매하기" },
                    { ILocalization.Key.Equip, "장착하기" },
                    { ILocalization.Key.Equipped, "장착 중" },

                    { ILocalization.Key.OutOfGold, "골드가 부족합니다." },
                    { ILocalization.Key.MaximumUpgradeStatus, "최대 업그레이드 상태입니다." },


                    { ILocalization.Key.TriconChapterName, "Tricon" },
                    { ILocalization.Key.RhombusChapterName, "Rhombus" },
                    { ILocalization.Key.PentagonicChapterName, "Pentagonic" },
                    { ILocalization.Key.HexahornChapterName, "Hexahorn" },
                    { ILocalization.Key.OctaviaChapterName, "Octavia" },
                    { ILocalization.Key.PyramidSurvivalName, "Pyramid" },
                    { ILocalization.Key.CubeSurvivalName, "Cube" },
                    { ILocalization.Key.PrismSurvivalName, "Prism" },



                    { ILocalization.Key.TriconChapterDescription, "돌진 주의!" },
                    { ILocalization.Key.RhombusChapterDescription, "받고 한번 더" },
                    { ILocalization.Key.PentagonicChapterDescription, "돌고 도는 탄환" },
                    { ILocalization.Key.HexahornChapterDescription, "한판 더!" },
                    { ILocalization.Key.OctaviaChapterDescription, "무작위 레이저" },

                    { ILocalization.Key.PyramidSurvivalDescription, "가벼운 도전" },
                    { ILocalization.Key.CubeSurvivalDescription, "실력자의 도전" },
                    { ILocalization.Key.PrismSurvivalDescription, "고난의 연속" },



                    { ILocalization.Key.NormalSkinName, "구체" },
                    { ILocalization.Key.BloodEaterSkinName, "블러드 이터" },
                    { ILocalization.Key.GuardSkinName, "가드" },

                    { ILocalization.Key.NormalSkinDescription, "검은 구체" },
                    { ILocalization.Key.BloodEaterSkinDescription, "일정 확률로 흡혈 10%" },
                    { ILocalization.Key.GuardSkinDescription, "데미지 감소 10%" },

                    { ILocalization.Key.AttackDamageName, "공격력" },
                    { ILocalization.Key.MoveSpeedName, "이동 속도" },
                    { ILocalization.Key.MaxHpName, "최대 체력" },
                    { ILocalization.Key.DamageReductionName, "받는 피해 감소" },

                    { ILocalization.Key.AttackDamageDescription0, "" },
                    { ILocalization.Key.AttackDamageDescription1, "공격력 5 증가" },
                    { ILocalization.Key.AttackDamageDescription2, "공격력 10 증가" },
                    { ILocalization.Key.AttackDamageDescription3, "공격력 15 증가" },
                    { ILocalization.Key.AttackDamageDescription4, "공격력 20 증가" },
                    { ILocalization.Key.AttackDamageDescription5, "공격력 30 증가" },

                    { ILocalization.Key.MoveSpeedDescription0, "" },
                    { ILocalization.Key.MoveSpeedDescription1, "이동 속도 1 증가" },
                    { ILocalization.Key.MoveSpeedDescription2, "이동 속도 2 증가" },
                    { ILocalization.Key.MoveSpeedDescription3, "이동 속도 3 증가" },

                    { ILocalization.Key.MaxHpDescription0, "" },
                    { ILocalization.Key.MaxHpDescription1, "체력 10 증가" },
                    { ILocalization.Key.MaxHpDescription2, "체력 15 증가" },
                    { ILocalization.Key.MaxHpDescription3, "체력 25 증가" },
                    { ILocalization.Key.MaxHpDescription4, "체력 35 증가" },
                    { ILocalization.Key.MaxHpDescription5, "체력 50 증가" },

                    { ILocalization.Key.DamageReductionDescription0, "" },
                    { ILocalization.Key.DamageReductionDescription1, "받는 피해 감소 5%" },
                    { ILocalization.Key.DamageReductionDescription2, "받는 피해 감소 10%" },
                    { ILocalization.Key.DamageReductionDescription3, "받는 피해 감소 15%" },
                    { ILocalization.Key.DamageReductionDescription4, "받는 피해 감소 20%" },
                    { ILocalization.Key.DamageReductionDescription5, "받는 피해 감소 30%" },

                    { ILocalization.Key.StatikkCardName, "유도 레이저" },
                    { ILocalization.Key.KnockbackCardName, "넉백" },
                    { ILocalization.Key.ImpactCardName, "충격파" },

                    { ILocalization.Key.UpgradeShootingCardName, "슈팅 강화" },
                    { ILocalization.Key.UpgradeDamageCardName, "최종 데미지 증가" },
                    { ILocalization.Key.UpgradeCooltimeCardName, "스킬 쿨타임 감소" },

                    { ILocalization.Key.SpawnRifleShooterCardName, "펫:슈터" },
                    { ILocalization.Key.SpawnRocketShooterCardName, "펫:붐버" },
                    { ILocalization.Key.SpawnBlackholeCardName, "블랙홀" },
                    { ILocalization.Key.SpawnBladeCardName, "블레이드" },
                    { ILocalization.Key.SpawnStickyBombCardName, "시한 폭탄" },

                    { ILocalization.Key.StatikkCardDescription, "근처의 적을 타격하는 레이저를 발사한다." },
                    { ILocalization.Key.KnockbackCardDescription, "적을 멀리 밀쳐내며 공격한다." },
                    { ILocalization.Key.ImpactCardDescription, "폭발을 일으켜 적을 튕겨내고 경직시킨다." },

                    { ILocalization.Key.UpgradeShootingCardDescription, "슈팅의 차징 속도를 감소시키고 무적시간을 증가시킨다." },
                    { ILocalization.Key.UpgradeDamageCardDescription, "최종 데미지를 상승시킨다." },
                    { ILocalization.Key.UpgradeCooltimeCardDescription, "쿨타임 스킬의 재사용 대기시간을 감소시킨다." },


                    { ILocalization.Key.SpawnRifleShooterCardDescription, "탄을 발사하는 펫을 소환한다." },
                    { ILocalization.Key.SpawnRocketShooterCardDescription, "포탄을 발사하는 펫을 소환한다." },
                    { ILocalization.Key.SpawnBlackholeCardDescription, "적을 빨아들이는 공간을 생성한다." },
                    { ILocalization.Key.SpawnBladeCardDescription, "벽을 튕겨다니며 공격하는 칼날을 발사한다." },
                    { ILocalization.Key.SpawnStickyBombCardDescription, "적에게 폭탄을 붙여 일정시간 이후 폭발시킨다." },
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
