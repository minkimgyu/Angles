using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerCreater
{
    protected BaseViewer _viewerPrefab;
    public ViewerCreater(BaseViewer viewerPrefab) { _viewerPrefab = viewerPrefab; }

    public BaseViewer Create()
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

        _viewerCreaters[BaseViewer.Name.SkinViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.SkinViewer]);
        _viewerCreaters[BaseViewer.Name.StatViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.StatViewer]);
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
        if (_viewerCreaters.ContainsKey(name) == false) return null;
        return _viewerCreaters[name].Create();
    }
}
