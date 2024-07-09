using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable : IPos
{
    bool CanAttach();
}
