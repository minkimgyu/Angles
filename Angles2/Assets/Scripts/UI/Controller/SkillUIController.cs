using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    Dictionary<BaseSkill.Name, BaseViewer> _viewers; // 생성해서 딕셔너리에 넣어준다.
    Dictionary<BaseSkill.Name, Sprite> _skillIcons;

    [SerializeField] RectTransform _skillViewerParent;

    List<BaseSkill.Type> _showTypes;
    System.Func<BaseViewer.Name, BaseViewer> SpawnViewer;


    public void Initialize(List<BaseSkill.Type> showTypes, Dictionary<BaseSkill.Name, Sprite> skillIcons, System.Func<BaseViewer.Name, BaseViewer> SpawnViewer)
    {
        _viewers = new Dictionary<BaseSkill.Name, BaseViewer>();
        _showTypes = showTypes;
        _skillIcons = skillIcons;
        this.SpawnViewer = SpawnViewer;
    }

    public void AddViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        bool isShowType = _showTypes.Contains(skill.SkillType);
        if (isShowType == false) return;

        BaseViewer viewer = SpawnViewer?.Invoke(BaseViewer.Name.SkillViewer);
        Sprite skillIcon = _skillIcons[skillName];
        viewer.Initialize(skillIcon);

        skill.ResetViewerValue += viewer.UpdateViewer;

        _viewers.Add(skillName, viewer);
        viewer.transform.SetParent(_skillViewerParent);
    }

    public void RemoveViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        BaseViewer viewer = _viewers[skillName];

        skill.ResetViewerValue -= viewer.UpdateViewer;
        _viewers.Remove(skillName);
        Destroy(viewer.gameObject);
    }
}
