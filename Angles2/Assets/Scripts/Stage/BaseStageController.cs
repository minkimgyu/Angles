using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 테스트 모드와 인 게임 모드를 나누기 위해서 BaseStageController를 구현함
// 이 클래스를 상속받아서 다른 모드를 제작한다.
abstract public class BaseStageController : MonoBehaviour
{
    public virtual void OnStageClearRequested() { }
    public virtual void OnMoveToNextStageRequested() { }
}
