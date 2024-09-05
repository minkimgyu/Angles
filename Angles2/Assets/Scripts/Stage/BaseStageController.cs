using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �׽�Ʈ ���� �� ���� ��带 ������ ���ؼ� BaseStageController�� ������
// �� Ŭ������ ��ӹ޾Ƽ� �ٸ� ��带 �����Ѵ�.
abstract public class BaseStageController : MonoBehaviour
{
    public virtual void OnStageClearRequested() { }
    public virtual void OnMoveToNextStageRequested() { }
}
