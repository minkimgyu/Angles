using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseViewerCreater
{
    protected BaseViewer _viewerPrefab;
    public BaseViewerCreater(BaseViewer viewerPrefab) { _viewerPrefab = viewerPrefab; }

    public virtual BaseViewer Create() { return default; }
}

public class ViewerCreater : BaseViewerCreater
{
    public ViewerCreater(BaseViewer viewerPrefab) : base(viewerPrefab) { }

    public override BaseViewer Create()
    {
        BaseViewer viewer = Object.Instantiate(_viewerPrefab);
        return viewer;
    }
}

public class ViewerFactory : BaseFactory
{
    Dictionary<BaseViewer.Name, ViewerCreater> _viewerCreaters;

    public ViewerFactory(Dictionary<BaseViewer.Name, BaseViewer> viewerPrefabs)
    {
        _viewerCreaters = new Dictionary<BaseViewer.Name, ViewerCreater>();

        _viewerCreaters[BaseViewer.Name.CardViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.CardViewer]);
        _viewerCreaters[BaseViewer.Name.CostCardViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.CostCardViewer]);
        _viewerCreaters[BaseViewer.Name.SkillViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.SkillViewer]);
        _viewerCreaters[BaseViewer.Name.HpViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.HpViewer]);
        _viewerCreaters[BaseViewer.Name.DashViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.DashViewer]);
        _viewerCreaters[BaseViewer.Name.DirectionViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.DirectionViewer]);
        _viewerCreaters[BaseViewer.Name.ChapterSelectViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.ChapterSelectViewer]);
    }

    public override BaseViewer Create(BaseViewer.Name name)
    {
        return _viewerCreaters[name].Create();
    }
}
