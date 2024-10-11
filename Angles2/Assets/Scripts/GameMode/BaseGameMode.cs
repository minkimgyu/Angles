using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseGameMode : MonoBehaviour
{
    protected abstract void Initialize(); // ������ ���۵� �� ó��
    public abstract void OnGameClearRequested(); // ���� Ŭ���� �� ó��
    public abstract void OnGameOverRequested(); // ���� ���� �� ó��
}