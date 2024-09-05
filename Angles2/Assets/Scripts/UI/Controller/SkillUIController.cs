using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    Dictionary<BaseSkill.Name, BaseViewer> _viewers; // 생성해서 딕셔너리에 넣어준다.
    Dictionary<BaseSkill.Name, Sprite> _skillIcons;

    [SerializeField] RectTransform _skillViewerParent;

    List<BaseSkill.Type> _showTypes;
    BaseFactory _viewerFactory;

    public void Initialize(List<BaseSkill.Type> showTypes, Dictionary<BaseSkill.Name, Sprite> skillIcons, BaseFactory viewerFactory)
    {
        ObserverEventBus.Register(ObserverEventBus.State.OnAddSkill, new AddSkillCommand(AddViewer));
        ObserverEventBus.Register(ObserverEventBus.State.OnRemoveSkill, new RemoveSkillCommand(RemoveViewer));

        _viewers = new Dictionary<BaseSkill.Name, BaseViewer>();
        _showTypes = showTypes;
        _skillIcons = skillIcons;
        _viewerFactory = viewerFactory;
    }

    public void AddViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        bool isShowType = _showTypes.Contains(skill.SkillType);
        if (isShowType == false) return;

        BaseViewer viewer = _viewerFactory.Create(BaseViewer.Name.SkillViewer);
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
