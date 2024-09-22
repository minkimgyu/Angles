using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISizeModifyStat
{
    public float SizeMultiplier { get; set; }
}

public class BaseSizeModifyComponent
{
    public virtual void ResetSize() {  }
}

public class NoSizeModifyComponent : BaseSizeModifyComponent
{
}

public class SizeModifyComponent : BaseSizeModifyComponent
{
    Transform _transform;
    ISizeModifyStat _sizeStat;

    public SizeModifyComponent(Transform transform, ISizeModifyStat sizeStat) { _transform = transform; _sizeStat = sizeStat; }

    public override void ResetSize() { _transform.localScale *= _sizeStat.SizeMultiplier; }
}
