using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BuffAttribute<T>
{
    public BuffAttribute(T min, T max, T currentValue)
    {
        _min = min;
        _max = max;
        _currentValue = currentValue;
    }

    T _min;
    T _max;
    T _currentValue;

    protected abstract bool Compare(T t1, T t2);

    public T Value 
    { 
        get
        { 
            if(Compare(_currentValue, _min))
            {
                return _min;
            }
            else if(Compare(_max, _currentValue))
            {
                return _max;
            }
            else
            {
                return _currentValue;
            }
        }
        set
        {
            _currentValue = value;
        }
    }
}

public class BuffInt : BuffAttribute<int>
{
    public BuffInt(int min, int max, int currentValue) : base(min, max, currentValue) { }

    protected override bool Compare(int t1, int t2) { return t1 < t2; }
}

public class BuffFloat : BuffAttribute<float>
{
    public BuffFloat(float min, float max, float currentValue) : base(min, max, currentValue) { }

    protected override bool Compare(float t1, float t2) { return t1 < t2; }
}