using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public interface ILocalization
{
    public enum Language
    {
        English,
        Korean
    }

    // 보스 등장
    // 스테이지 클리어

    public enum Key
    {
        Start,
        End,

        Yes,
        No,

        ForLeftAd,
        Left,

        Setting,
        BGMMenu,
        SFXMenu,
        ExitMenu,
        ResumeMenu,

        Skin,
        Stat,

        Upgrade,

        Buy,
        Equip,
        Equipped,

        OutOfGold,
        MaximumUpgradeStatus,

        StageCount,
        SurvivalTime,
        Progress,

        StageClear,
        BossClear,
        BossIncoming,

        RecordTime,
        TabToContinue,
        TabToRevive,
        TabToGetGold,

        LeftCount,
        PickAgain,

        TriconChapterName,
        RhombusChapterName,
        PentagonicChapterName,
        HexahornChapterName,
        OctaviaChapterName,
        PyramidSurvivalName,
        CubeSurvivalName,
        PrismSurvivalName,
        MainTutorialName,
        DodecaSurvivalName,
        IcosaSurvivalName,

        TriconChapterDescription,
        RhombusChapterDescription,
        PentagonicChapterDescription,
        HexahornChapterDescription,
        OctaviaChapterDescription,
        PyramidSurvivalDescription,
        CubeSurvivalDescription,
        PrismSurvivalDescription,
        MainTutorialDescription,
        DodecaSurvivalDescription,
        IcosaSurvivalDescription,

        NormalSkinName,
        BloodEaterSkinName,
        GuardSkinName,

        NormalSkinDescription,
        BloodEaterSkinDescription,
        GuardSkinDescription,

        AttackDamageName,
        MoveSpeedName,
        MaxHpName,
        DamageReductionName,

        AttackDamageDescription0,
        AttackDamageDescription1,
        AttackDamageDescription2,
        AttackDamageDescription3,
        AttackDamageDescription4,
        AttackDamageDescription5,

        MoveSpeedDescription0,
        MoveSpeedDescription1,
        MoveSpeedDescription2,
        MoveSpeedDescription3,

        MaxHpDescription0,
        MaxHpDescription1,
        MaxHpDescription2,
        MaxHpDescription3,
        MaxHpDescription4,
        MaxHpDescription5,

        DamageReductionDescription0,
        DamageReductionDescription1,
        DamageReductionDescription2,
        DamageReductionDescription3,
        DamageReductionDescription4,
        DamageReductionDescription5,

        StatikkCardName,
        KnockbackCardName,
        ImpactCardName,
        UpgradeShootingCardName,
        UpgradeDamageCardName,
        UpgradeCooltimeCardName,
        SpawnRifleShooterCardName,
        SpawnRocketShooterCardName,
        SpawnBlackholeCardName,
        SpawnBladeCardName,
        SpawnStickyBombCardName,

        StatikkCardDescription,
        KnockbackCardDescription,
        ImpactCardDescription,
        UpgradeShootingCardDescription,
        UpgradeDamageCardDescription,
        UpgradeCooltimeCardDescription,
        SpawnRifleShooterCardDescription,
        SpawnRocketShooterCardDescription,
        SpawnBlackholeCardDescription,
        SpawnBladeCardDescription,
        SpawnStickyBombCardDescription,

        Completed,

        MoveQuestTitle,
        ShootingQuestTitle,
        CollisionQuestTitle,
        CancelShootingQuestTitle,
        GetSkillQuestTitle,
        StageClearQuestTitle,
        EnterPotalQuestTitle,

        MoveQuestContent,
        ShootingQuestContent,
        CollisionQuestContent,
        CancelShootingQuestContent,
        GetSkillQuestContent,
        StageClearQuestContent,
        EnterPotalQuestContent,

        UploadToCloud,
        RetrieveToLocal,
    }

    string GetWord(Key key);
    string GetWord(string key);
}

[Serializable]
public struct Localization
{
    [JsonProperty] Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> _word;
    [JsonIgnore] public Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> Word { get => _word; }

    public Localization(Dictionary<ILocalization.Key, Dictionary<ILocalization.Language, string>> word)
    {
        _word = word;
    }
}

public class NullLocalizationHandler : ILocalization
{
    public string GetWord(ILocalization.Key key) { return default; }
    public string GetWord(string key) { return default; }
}

public class LocalizationHandler : ILocalization
{
    Localization _localization;

    public LocalizationHandler(Localization localization)
    {
        _localization = localization;
    }

    public string GetWord(ILocalization.Key key)
    {
        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        ILocalization.Language language = saveData._language;

        if (_localization.Word.ContainsKey(key) == false || _localization.Word[key].ContainsKey(language) == false) return string.Empty;
        return _localization.Word[key][language];
    }

    public string GetWord(string key)
    {
        ILocalization.Key nameKey;

        bool canParse = Enum.TryParse<ILocalization.Key>(key, out nameKey);
        if(canParse == false) return string.Empty;

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        ILocalization.Language language = saveData._language;

        if (_localization.Word.ContainsKey(nameKey) == false || _localization.Word[nameKey].ContainsKey(language) == false) return string.Empty;
        return _localization.Word[nameKey][language];
    }
}
