using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector3 _offsetFromCenter;

    Dictionary<BaseStage.Type, GameObject> _stagePrefabs;
    Dictionary<BaseStage.Type, List<BaseStage>> _stageObjects;

    const int _maxRow = 3;
    const int _offset = 50;

    public void Initialize()
    {
        _stagePrefabs = new Dictionary<BaseStage.Type, GameObject>();
        _stageObjects = new Dictionary<BaseStage.Type, List<BaseStage>>();
    }

    public Dictionary<BaseStage.Type, List<BaseStage>> ReturnStageObjects() { return _stageObjects; }

    public void CreateMap()
    {
        GameObject startStage = AddressableManager.Instance.PrefabAssetDictionary[StageType.StartStage.ToString()];
        _stagePrefabs.Add(StageType.StartStage, startStage);

        GameObject bonusStage = AddressableManager.Instance.PrefabAssetDictionary[StageType.BonusStage.ToString()];
        _stagePrefabs.Add(StageType.BonusStage, bonusStage);

        int index = 0;
        while (AddressableManager.Instance.PrefabAssetDictionary.ContainsKey(StageType.FightStage.ToString() + index))
        {
            GameObject stage = AddressableManager.Instance.PrefabAssetDictionary[_stageString + index];
            _stagePrefabs.Add(StageType.BonusStage, stage);
            index++;
        }

        int xPos = 0;
        int rowCount = 0;

        for (int i = 0; i < _stagePrefabs.Count; i++)
        {
            Vector3 stagePosition = new Vector3(xPos, rowCount * _offset) + _offsetFromCenter;



            GameObject stageObject = Instantiate(_stagePrefabs[i], transform);
            stageObject.transform.position = stagePosition;

            BaseStage stage = stageObject.GetComponent<BaseStage>();
            _stageObjects.Add(stage);

            rowCount++;
            if (rowCount == _maxRow)
            {
                xPos += _offset;
                rowCount = 0;
            }
        }
    }
}
