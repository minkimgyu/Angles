using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffUsable
{
    void AddBuff(BaseBuff.Name name, BaseBuff buff);
    void RemoveBuff(BaseBuff.Name name);
}
