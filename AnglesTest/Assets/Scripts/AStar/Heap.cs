using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem<T> : IComparable<T>
{
    public int StoredIndex { get; set; }
    public void Dispose();
}

public class Heap<T> where T : IItem<T>
{
    public Heap(int maxItemCount)
    {
        _items = new T[maxItemCount];
        _count = 0;
    }

    int GetLastIndexHavingChild()
    {
        return (_count - 2) / 2;
    }

    int GetParentIndex(int index)
    {
        return (index - 1) / 2;
    }

    int GetChild(int index, bool isLeft = true)
    {
        if (isLeft) return 2 * index + 1;
        else return 2 * index + 2;
    }

    void Swap(int index1, int index2)
    {
        T tmp = _items[index2];
        _items[index2] = _items[index1];
        _items[index1] = tmp;

        _items[index1].StoredIndex = index1;
        _items[index2].StoredIndex = index2;
    }


    void PercolateUp(int index)
    {
        int currentIndex = index;
        int parentIndex = GetParentIndex(index);

        while (currentIndex > 0 && _items[parentIndex].CompareTo(_items[currentIndex]) > 0)
        {
            Swap(parentIndex, currentIndex);

            currentIndex = parentIndex;
            parentIndex = GetParentIndex(parentIndex);
        }
    }

    void PercolateDown(int index)
    {
        int currentIndex = index;
        int childIndex = ReturnMinIndexInChildren(index);

        while (childIndex < _count && _items[childIndex].CompareTo(_items[currentIndex]) < 0)
        {
            Swap(childIndex, currentIndex);

            if (childIndex > GetLastIndexHavingChild()) break; // 자식이 존재하지 않는 경우 탈출

            currentIndex = childIndex;
            childIndex = ReturnMinIndexInChildren(childIndex); // 조건 봐주기
        }
    }

    int ReturnMinIndexInChildren(int index)
    {
        int leftChildIndex = GetChild(index);
        int rightChildIndex = GetChild(index, false);

        T leftChild = _items[leftChildIndex];
        T rightChild = _items[rightChildIndex];

        // right가 더 작은 경우
        if (rightChild != null && rightChild.CompareTo(leftChild) < 0) return rightChildIndex;
        return leftChildIndex;
    }

    public void Clear()
    {
        for (int i = 0; i < _count; i++)
        {
            _items[i].Dispose();
        }
        Array.Clear(_items, 0, Count);
        _count = 0;
    }

    public bool Contain(T item)
    {
        if (item.StoredIndex == -1) return false; // 초기화가 안 되어있을 경우

        return Equals(_items[item.StoredIndex], item);
    }

    public void Insert(T item)
    {
        item.StoredIndex = _count;
        _items[_count] = item;
        PercolateUp(_count);
        _count++;
    }

    public void DeleteMin()
    {
        if (_count == 0) return;
        else if (_count == 1)
        {
            _items[0].StoredIndex = -1;
            _count--;
            return;
        }

        T lastItem = _items[_count - 1];
        lastItem.StoredIndex = 0;

        _items[0].StoredIndex = -1;
        _count--;

        _items[0] = lastItem; // 첫번째 아이템으로 바꿔줌

        PercolateDown(0);
    }

    public T ReturnMin() { return _items[0]; }

    T[] _items;
    int _count = 0;
    public int Count { get { return _count; } }
}