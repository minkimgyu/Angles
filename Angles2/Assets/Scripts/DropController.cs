using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct DropData
{
    int _maxCount; // 최대 드랍 개수
    public int MaxCount { get { return _maxCount; } }

    List<Tuple<IInteractable.Name, float>> _itemDatas; // 드랍 가능성이 있는 아이템 이름, 드랍 확률
    public List<Tuple<IInteractable.Name, float>> ItemDatas { get { return _itemDatas; } }

    public DropData(int maxCount, List<Tuple<IInteractable.Name, float>> itemDatas)
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
        for (int i = 0; i < data.ItemDatas.Count; i++)
        {
            float random = UnityEngine.Random.Range(0, 1f);
            if (random <= data.ItemDatas[i].Item2)
            {
                IInteractable interactableObject = _interactableFactory.Create(data.ItemDatas[i].Item1);
                Vector3 pos = ReturnSpreadOffset(position, _spreadOffset);
                interactableObject.ResetPosition(pos);
            }
        }
    }
}
