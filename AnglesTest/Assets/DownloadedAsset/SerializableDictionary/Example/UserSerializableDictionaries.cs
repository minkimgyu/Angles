using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//[Serializable]
//public class TrasformEnemyNameDictionary : SerializableDictionary<Transform, BaseEnemy.Name> { }

//[Serializable]
//public class StringStringDictionary : SerializableDictionary<string, string> {}

//[Serializable]
//public class CardInfoDictionary : SerializableDictionary<BaseSkill.Name, BaseCardInfo> { }

//[Serializable]
//public class EffectInputDictionary : SerializableDictionary<BaseEffect.Name, EffectCreaterInput> { }

//[Serializable]
//public class LifeInputDictionary : SerializableDictionary<BaseLife.Name, LifeCreaterInput> { }

//[Serializable]
//public class WeaponInputDictionary : SerializableDictionary<BaseWeapon.Name, WeaponCreaterInput> { }

//[Serializable]
//public class SkillInputDictionary : SerializableDictionary<BaseSkill.Name, SkillCreaterInput> { }

//[Serializable]
//public class InteractableObjectInputDictionary : SerializableDictionary<IInteractable.Name, InteractableObjectCreaterInput> { }

//[Serializable]
//public class FlockBehaviorDictionary : SerializableDictionary<FlockComponent.BehaviorType, float> { }

[Serializable]
public class HandlerDictionary : SerializableDictionary<InputController.Side, BaseInputHandler> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}