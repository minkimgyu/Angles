using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    Dictionary<BaseSkill.Name, SkillViewer> _viewers; // �����ؼ� ��ųʸ��� �־��ش�.
    Dictionary<BaseSkill.Name, Sprite> _skillIcons;

    [SerializeField] RectTransform _skillViewerParent;

    List<BaseSkill.Type> _showTypes;
    BaseFactory _viewerFactory;

    public void Initialize(List<BaseSkill.Type> showTypes, Dictionary<BaseSkill.Name, Sprite> skillIcons, BaseFactory viewerFactory)
    {
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.State.OnAddSkill, new AddSkillCommand(AddViewer));
        EventBusManager.Instance.ObserverEventBus.Register(ObserverEventBus.State.OnRemoveSkill, new RemoveSkillCommand(RemoveViewer));

        _viewers = new Dictionary<BaseSkill.Name, SkillViewer>();
        _showTypes = showTypes;
        _skillIcons = skillIcons;
        _viewerFactory = viewerFactory;
    }

    public void AddViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        bool isShowType = _showTypes.Contains(skill.SkillType);
        if (isShowType == false) return;

        SkillViewer viewer = (SkillViewer)_viewerFactory.Create(BaseViewer.Name.SkillViewer);
        Sprite skillIcon = _skillIcons[skillName];
        viewer.Initialize(skillIcon);

        skill.AddViewEvent(viewer.UpdateViewer);

        _viewers.Add(skillName, viewer);
        viewer.transform.SetParent(_skillViewerParent);
        viewer.transform.localScale = Vector3.one;
    }

    public void RemoveViewer(BaseSkill.Name skillName, BaseSkill skill)
    {
        SkillViewer viewer = _viewers[skillName];

        skill.RemoveViewEvent(viewer.UpdateViewer);
        _viewers.Remove(skillName);
        Destroy(viewer.gameObject);
    }
}
