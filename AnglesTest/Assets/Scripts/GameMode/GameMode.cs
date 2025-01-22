using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class GameMode : MonoBehaviour
{
    public enum Type
    {
        Chapter,
        Survival,
        Tutorial,
    }

    public static Type GetLevelType(Level level)
    {
        foreach (Type type in System.Enum.GetValues(typeof(Type)))
        {
            if (level.ToString().Contains(type.ToString()))
            {
                return type;
            }
        }

        return default;
    }

    public static int GetLevelCount(Type type)
    {
        int count = 0;
        foreach (Level level in System.Enum.GetValues(typeof(Level)))
        {
            if (level.ToString().Contains(type.ToString())) count++;
        }

        return count;
    }

    public static List<Level> GetLevels(Type type)
    {
        List<Level> levels = new List<Level>();
        foreach (Level level in System.Enum.GetValues(typeof(Level)))
        {
            if (level.ToString().Contains(type.ToString()))
            {
                levels.Add(level);
            }
        }
        return levels;
    }

    public static int GetLevelIndex(Type type, Level level)
    {
        List<Level> levels = GetLevels(type);
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i] == level) return i;
        }

        return -1;
    }

    public static Level GetLevel(Type type, int index)
    {
        List<Level> levels = GetLevels(type);
        return levels[index];
    }

    // Chapter�� Survival�� �����ؼ� �������� �ڵ尡 �ʿ��ϴ�.
    // Index �Լ� �ʿ�

    public enum Level
    {
        TriconChapter,
        RhombusChapter,
        PentagonicChapter,
        HexahornChapter,
        OctaviaChapter,

        PyramidSurvival,
        CubeSurvival,
        PrismSurvival,

        MainTutorial
    }

    public enum LevelColor
    {
        Blue,
        Red,
        Green,
        White,
        Orange,
    }

    protected int _reviveChance = 1;
    protected bool CanRevive { get { return _reviveChance > 0; } }

    protected abstract void Initialize(GameMode.Type modeType); // ������ ���۵� �� ó��
    public abstract void OnGameClearRequested(); // ���� Ŭ���� �� ó��
    public abstract void OnGameOverRequested(); // ���� ���� �� ó��
}