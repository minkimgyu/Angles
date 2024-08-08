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

public class DropHandler
{
    float _spreadOffset;
    IFactory _factory;
    DungeonSystem.CommandCollection _commandCollection;

    public DropHandler(float spreadOffset, IFactory factory, DungeonSystem.CommandCollection commandCollection)
    {
        _spreadOffset = spreadOffset;
        _factory = factory;
        _commandCollection = commandCollection;
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
            if (data.ItemDatas[i].Item2 <= random)
            {
                IInteractable interactableObject = _factory.Create(data.ItemDatas[i].Item1);
                Vector3 pos = ReturnSpreadOffset(position, _spreadOffset);
                interactableObject.ResetPosition(pos);

                switch (data.ItemDatas[i].Item1)
                {
                    case IInteractable.Name.SkillBubble:
                        interactableObject.AddCommand(_commandCollection.CreateCardsCommand);
                        break;
                    case IInteractable.Name.Coin:
                        interactableObject.AddCommand(_commandCollection.ChangeCoin);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
