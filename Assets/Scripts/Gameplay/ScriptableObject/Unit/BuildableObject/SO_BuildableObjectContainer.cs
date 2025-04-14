using System;
using UnityEngine;

namespace Unit.BuildableObject
{
    [CreateAssetMenu(fileName = "SO_BuildableObjectContainer", menuName = "SO/Gameplay/Unit/Buildable/Container", order = 51)]
    public class SO_BuildableObjectContainer : ScriptableObject
    {
        [SerializeField] private SO_BuildableObject[] buildableObjects;

        public int GetBuildableObjectsCount => buildableObjects.Length;
        
        public SO_BuildableObject[] GetBuildableObjects() => buildableObjects;
        
        public SO_BuildableObject GetBuildableObjectConfig(BuildableObjectType buildableObjectType)
        {
            foreach (var VARIABLE in buildableObjects)
            {
                if (VARIABLE.BuildableObjectTypeID == buildableObjectType)
                    return VARIABLE;
            }
            throw new NullReferenceException();
        }
    }
}