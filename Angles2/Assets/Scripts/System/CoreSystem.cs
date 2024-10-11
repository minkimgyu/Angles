using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreSystem : MonoBehaviour
{
    public AddressableHandler AddressableHandler { get; private set; }
    public Database Database { get; private set; }
    public FactoryCollection FactoryCollection { get; private set; }

    public void Initialize(AddressableHandler addressableHandler, FactoryCollection factoryCollection, Database database)
    {
        AddressableHandler = addressableHandler;
        FactoryCollection = factoryCollection;
        Database = database;
    }
}
