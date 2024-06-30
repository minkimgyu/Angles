using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCountViewer : MonoBehaviour
{
    [SerializeField] Image _fillContent;

    public void FillViewer(float ratio)
    {
        _fillContent.fillAmount = ratio;
    }

    public void OnOffViewer(bool on)
    {
        gameObject.SetActive(on);
    }
}
