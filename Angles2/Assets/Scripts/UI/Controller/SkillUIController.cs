using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillUIController : MonoBehaviour
{
    Dictionary<BaseSkill.Name, BaseViewer> _viewers; // 생성해서 딕셔너리에 넣어준다.
    [SerializeField] RectTransform _skillViewerParent;
    const string _IconString = "Icon";

    List<BaseSkill.Type> _showTypes;

    public void Initialize(List<BaseSkill.Type> types)
    {
        _viewers = new Dictionary<BaseSkill.Name, BaseViewer>();
        _showTypes = types;
    }

    public void AddViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        bool isShowType = _showTypes.Contains(skill.SkillType);
        if (isShowType == false) return;

        BaseViewer viewer = ViewerFactory.Create(BaseViewer.Name.SkillViewer);
        Sprite skillIcon = AddressableManager.Instance.SpriteAssetDictionary[skillName.ToString() + _IconString];
        viewer.Initialize(skillIcon);

        skill.ResetViewerValue += viewer.UpdateViewer;

        _viewers.Add(skillName, viewer);
        viewer.transform.SetParent(_skillViewerParent);
    }

    public void RemoveViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        BaseViewer viewer = _viewers[skillName];

        skill.ResetViewerValue += viewer.UpdateViewer;
        _viewers.Remove(skillName);
        Destroy(viewer.gameObject);
    }
}
