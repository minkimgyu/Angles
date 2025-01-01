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

    public enum Key
    {
        Start,
        End,

        Buy,
        Equip,
        Equipped,

        OutOfGold,
        MaximumUpgradeStatus,

        NormalSkinName,
        BloodEaterSkinName,
        GuardSkinName,

        NormalSkinInfo,
        BloodEaterSkinInfo,
        GuardSkinInfo,

        AttackDamageName,
        MoveSpeedName,
        MaxHpName,
        DamageReductionName,

        AttackDamageInfo,
        MoveSpeedInfo,
        MaxHpInfo,
        DamageReductionInfo,

        StatikkCardInfo,
        KnockbackCardInfo,
        ImpactCardInfo,
        SpawnRifleShooterCardInfo,
        SpawnRocketShooterCardInfo,
        SpawnBlackholeCardInfo,
        SpawnBladeCardInfo,
        SpawnStickyBombCardInfo
    }

    string GetWord(Key key);
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
}
