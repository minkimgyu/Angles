using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameViewerFactory : BaseFactory
{
    Dictionary<BaseViewer.Name, ViewerCreater> _viewerCreaters;

    public InGameViewerFactory(Dictionary<BaseViewer.Name, BaseViewer> viewerPrefabs)
    {
        _viewerCreaters = new Dictionary<BaseViewer.Name, ViewerCreater>();

        _viewerCreaters[BaseViewer.Name.CardViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.CardViewer]);
        _viewerCreaters[BaseViewer.Name.CostCardViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.CostCardViewer]);
        _viewerCreaters[BaseViewer.Name.SkillViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.SkillViewer]);
        _viewerCreaters[BaseViewer.Name.HpViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.HpViewer]);
        _viewerCreaters[BaseViewer.Name.DashViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.DashViewer]);
        _viewerCreaters[BaseViewer.Name.DirectionViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.DirectionViewer]);
    }

    public override BaseViewer Create(BaseViewer.Name name)
    {
        return _viewerCreaters[name].Create();
    }
}
