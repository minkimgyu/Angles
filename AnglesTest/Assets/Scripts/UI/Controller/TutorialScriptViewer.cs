using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TutorialScriptViewer : MonoBehaviour
{
    [SerializeField] Image _leftFinger;
    [SerializeField] Image _rightFinger;

    [SerializeField] Image _scriptContent;
    [SerializeField] TMP_Text _titleTxt;
    [SerializeField] TMP_Text _contentTxt;

    public void Initialize()
    {
        _scriptContent.gameObject.SetActive(false);

        _leftFinger.color = new Color(1, 1, 1, 0);
        _rightFinger.color = new Color(1, 1, 1, 0);
    }

    public void FadeInOutLeftFinger(float fadeDuration, int fadeCount)
    {
        _leftFinger.DOFade(1f, fadeDuration)
        .SetLoops(fadeCount, LoopType.Yoyo);
        //.onComplete += () => { _leftFinger.DOFade(0f, fadeDuration); };
    }

    public void StopLeftFinger()
    {
        _leftFinger.DOKill();
        _leftFinger.color = new Color(1, 1, 1, 0);
    }


    public void DragRightFinger(Vector2 drawPosition, float moveDuration, float fadeDuration)
    {
        _rightFinger.rectTransform.localPosition = Vector2.zero;

        _rightFinger.DOFade(1f, fadeDuration)
        .onComplete += () => 
        {
            _rightFinger.transform.DOLocalMove(drawPosition, moveDuration)
            .onComplete += () =>
            {
                _rightFinger.DOFade(0f, fadeDuration);
            };
        };
    }

    public void StopRightFinger()
    {
        _rightFinger.DOKill();
        _leftFinger.color = new Color(1, 1, 1, 0);
    }

    public void ActivateScript(bool active, float delay)
    {
        if (active)
        {
            _scriptContent.gameObject.SetActive(true);

            _scriptContent.DOFade(1, delay);
            _titleTxt.DOFade(1, delay);
            _contentTxt.DOFade(1, delay);
        }
        else
        {
            // 완료되면 꺼주기
            _scriptContent.DOFade(0, delay).onComplete += () => { _scriptContent.gameObject.SetActive(false); };
            _titleTxt.DOFade(0, delay);
            _contentTxt.DOFade(0, delay);
        }
    }

    public void ChangeScript(string title, string content)
    {
        _titleTxt.text = title;
        _contentTxt.text = content;
    }
}
