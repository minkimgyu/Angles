using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusManager : Singleton<EventBusManager>
{
    public enum Type
    {
        Main,
        Sub,
        Observer
    }

    public MainEventBus _mainEventBus = new MainEventBus();
    public MainEventBus MainEventBus { get { return _mainEventBus; } }

    public SubEventBus _subEventBus = new SubEventBus();
    public SubEventBus SubEventBus { get { return _subEventBus; } }

    public ObserverEventBus _observerEventBus = new ObserverEventBus();
    public ObserverEventBus ObserverEventBus { get { return _observerEventBus; } }
}
