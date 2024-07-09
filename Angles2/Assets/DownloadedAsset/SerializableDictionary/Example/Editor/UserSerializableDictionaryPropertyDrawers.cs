using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(WeaponInputDictionary))]
[CustomPropertyDrawer(typeof(ItemInputDictionary))]
[CustomPropertyDrawer(typeof(SkillInputDictionary))]
[CustomPropertyDrawer(typeof(EffectInputDictionary))]
[CustomPropertyDrawer(typeof(OutlineColorDictionary))]

[CustomPropertyDrawer(typeof(FlockBehaviorDictionary))]
[CustomPropertyDrawer(typeof(StringStringDictionary))]

[CustomPropertyDrawer(typeof(HandlerDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
