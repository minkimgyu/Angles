﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
<<<<<<< Updated upstream
using System;

[CustomPropertyDrawer(typeof(StringBaseEntitySODictionary))]

[CustomPropertyDrawer(typeof(StringBaseBuffSODictionary))]
[CustomPropertyDrawer(typeof(StringBaseSkillSODictionary))]

[CustomPropertyDrawer(typeof(EffectConditionEffectDataDictionary))]
[CustomPropertyDrawer(typeof(EffectConditionSoundDataDictionary))]


[CustomPropertyDrawer(typeof(StringFollowEnemyStatDictionary))]
[CustomPropertyDrawer(typeof(StringReflectEnemyStatDictionary))]
=======

[CustomPropertyDrawer(typeof(StringStatSlotDictionary))]
[CustomPropertyDrawer(typeof(StringSkillSlotDictionary))]
[CustomPropertyDrawer(typeof(StringStatSlotDataDictionary))]
[CustomPropertyDrawer(typeof(StringSkillSlotDataDictionary))]
>>>>>>> Stashed changes

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
