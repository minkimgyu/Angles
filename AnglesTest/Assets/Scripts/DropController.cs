using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[System.Serializable]
public struct DropData
{
    [JsonProperty] int _maxCount; // 최대 드랍 개수
    [JsonIgnore] public int MaxCount { get { return _maxCount; } }

    [JsonProperty] Dictionary<IInteractable.Name, float> _itemDatas;
    [JsonIgnore] public Dictionary<IInteractable.Name, float> ItemDatas { get { return _itemDatas; } }

    public DropData(int maxCount, Dictionary<IInteractable.Name, float> itemDatas)
    {
        _maxCount = maxCount;
        _itemDatas = itemDatas;
    }
}

public class DropController
{
    const float _spreadOffset = 1.5f;
    BaseFactory _interactableFactory;

    public DropController( BaseFactory interactableFactory)
    {
        _interactableFactory = interactableFactory;
        EventBusManager.Instance.SubEventBus.Register(SubEventBus.State.DropItem, new DropItemCommand(OnDropRequested));
    }

    Vector3 ReturnSpreadOffset(Vector3 position, float spreadOffset)
    {
        return position + UnityEngine.Random.Range(-spreadOffset, spreadOffset) * new Vector3(1, 1, 0);
    }

    public void OnDropRequested(DropData data, Vector3 position)
    {
        foreach (var item in data.ItemDatas)
        {
            float random = UnityEngine.Random.Range(0, 1f);
            if (random <= item.Value)
            {
                IInteractable interactableObject = _interactableFactory.Create(item.Key);
                Vector3 pos = ReturnSpreadOffset(position, _spreadOffset);
                interactableObject.ResetPosition(pos);
            }
        }
    }
}
