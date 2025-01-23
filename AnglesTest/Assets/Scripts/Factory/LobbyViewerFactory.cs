using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyViewerFactory : BaseFactory
{
    Dictionary<BaseViewer.Name, ViewerCreater> _viewerCreaters;

    public LobbyViewerFactory(Dictionary<BaseViewer.Name, BaseViewer> viewerPrefabs)
    {
        _viewerCreaters = new Dictionary<BaseViewer.Name, ViewerCreater>();

        _viewerCreaters[BaseViewer.Name.SkinViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.SkinViewer]);
        _viewerCreaters[BaseViewer.Name.StatViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.StatViewer]);
        _viewerCreaters[BaseViewer.Name.ChapterSelectViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.ChapterSelectViewer]);
    }

    public override BaseViewer Create(BaseViewer.Name name)
    {
        if (_viewerCreaters.ContainsKey(name) == false) return null;
        return _viewerCreaters[name].Create();
    }
}

public class MenuViewerFactory : BaseFactory
{
    Dictionary<BaseViewer.Name, ViewerCreater> _viewerCreaters;

    public MenuViewerFactory(Dictionary<BaseViewer.Name, BaseViewer> viewerPrefabs)
    {
        _viewerCreaters = new Dictionary<BaseViewer.Name, ViewerCreater>();
        _viewerCreaters[BaseViewer.Name.CloudSaveViewer] = new ViewerCreater(viewerPrefabs[BaseViewer.Name.CloudSaveViewer]);
    }

    public override BaseViewer Create(BaseViewer.Name name)
    {
        if (_viewerCreaters.ContainsKey(name) == false) return null;
        return _viewerCreaters[name].Create();
    }
}