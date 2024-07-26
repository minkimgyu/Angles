using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerCreater : ObjCreater<BaseViewer>
{
    public override BaseViewer Create()
    {
        GameObject obj = Object.Instantiate(_prefab);
        BaseViewer viewer = obj.GetComponent<BaseViewer>();
        if (viewer == null) return null;
        return viewer;
    }
}

public class ViewerFactory : MonoBehaviour
{
    Dictionary<BaseViewer.Name, ViewerCreater> _viewerCreaters;
    private static ViewerFactory _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        _viewerCreaters = new Dictionary<BaseViewer.Name, ViewerCreater>();

        int viewerCount = System.Enum.GetValues(typeof(BaseViewer.Name)).Length;
        for (int i = 0; i < viewerCount; i++)
        {
            BaseViewer.Name key = (BaseViewer.Name)i;
            GameObject prefab = AddressableManager.Instance.PrefabAssetDictionary[key.ToString()];

            _viewerCreaters[key] = new ViewerCreater();
            _viewerCreaters[key].Initialize(prefab);
        }
    }

    public static BaseViewer Create(BaseViewer.Name name)
    {
        return _instance._viewerCreaters[name].Create();
    }
}
