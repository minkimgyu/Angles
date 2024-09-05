using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    Dictionary<BaseBuff.Name, BaseBuff> _buffDictionary = new Dictionary<BaseBuff.Name, BaseBuff>();
    Dictionary<BaseBuff.Type, BuffCommand> _buffCommands;

    public void Initialize(Dictionary<BaseBuff.Type, BuffCommand> buffCommands)
    {
        _buffCommands = new Dictionary<BaseBuff.Type, BuffCommand>();
    }

    public void AddBuff(BaseBuff.Name name, BaseBuff buff)
    {
        bool alreadyHave = _buffDictionary.ContainsKey(name);
        if (alreadyHave)
        {
            _buffDictionary[name].Upgrade();
            return;
        }

        buff.OnAdd();
        _buffDictionary.Add(name, buff);
    }

    public void RemoveBuff(BaseBuff.Name name)
    {
        _buffDictionary[name].OnRemove();
        _buffDictionary.Remove(name);
    }
}
