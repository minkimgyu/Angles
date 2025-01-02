using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

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

        Setting,
        BGMMenu,
        SFXMenu,
        ExitMenu,
        ResumeMenu,

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

        LeftCount,
        PickAgain,

        GameClear,
        GameOver,

        TriconChapterName,
        RhombusChapterName,
        PentagonicChapterName,
        HexahornChapterName,
        OctaviaChapterName,
        PyramidSurvivalName,
        CubeSurvivalName,
        PrismSurvivalName,

        TriconChapterDescription,
        RhombusChapterDescription,
        PentagonicChapterDescription,
        HexahornChapterDescription,
        OctaviaChapterDescription,
        PyramidSurvivalDescription,
        CubeSurvivalDescription,
        PrismSurvivalDescription,

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
        MoveSpeedDescription4,
        MoveSpeedDescription5,

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
        SpawnStickyBombCardDescription
    }

    string GetWord(Key key);
    string GetWord(string key);
}

public struct Localization
{
    Dictionary<ILocalization.Language, Dictionary<ILocalization.Key, string>> _word;
    public Dictionary<ILocalization.Language, Dictionary<ILocalization.Key, string>> Word { get => _word; }

    public Localization(Dictionary<ILocalization.Language, Dictionary<ILocalization.Key, string>> word)
    {
        _word = word;
    }
}

public class NULLLocalizationHandler : ILocalization
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

        if (_localization.Word.ContainsKey(language) == false || _localization.Word[language].ContainsKey(key) == false) return string.Empty;
        return _localization.Word[language][key];
    }

    public string GetWord(string key)
    {
        ILocalization.Key nameKey;

        bool canParse = Enum.TryParse<ILocalization.Key>(key, out nameKey);
        if(canParse == false) return string.Empty;

        SaveData saveData = ServiceLocater.ReturnSaveManager().GetSaveData();
        ILocalization.Language language = saveData._language;

        if (_localization.Word.ContainsKey(language) == false || _localization.Word[language].ContainsKey(nameKey) == false) return string.Empty;
        return _localization.Word[language][nameKey];
    }
}
