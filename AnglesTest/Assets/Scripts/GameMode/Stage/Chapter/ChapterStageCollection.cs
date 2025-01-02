using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    public virtual SurvivalStage SurvivalStageLevel { get { return null; } }

    public virtual StartStage StartStage { get { return null; } }
    public virtual BonusStage BonusStage { get { return null; } }
    public virtual BossStage BossStage { get { return null; } }
    public virtual List<MobStage> MobStages { get { return null; } }
}

public class ChapterStageCollection : MonoBehaviour, ILevel
{
    [SerializeField] StartStage _startStage;
    [SerializeField] BonusStage _bonusStage;
    [SerializeField] BossStage _bossStage;
    [SerializeField] List<MobStage> _mobStages;

    public StartStage StartStage { get => _startStage; }
    public BonusStage BonusStage { get => _bonusStage; }
    public BossStage BossStage { get => _bossStage; }
    public List<MobStage> MobStages { get => _mobStages; }
}
