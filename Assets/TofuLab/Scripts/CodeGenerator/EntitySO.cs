using System;
using System.Collections.Generic;
using UnityEngine;

namespace TofuLab.CodeGenerator {
    [CreateAssetMenu (
        menuName = "ScriptableObjects/EntitySO",
        fileName = "EntitySO"
    )]

    public class EntitySO : ScriptableObject {
        [Header ("Entity名"), SerializeField] public string Name;
        [Header ("イミュータブルかどうか"), SerializeField] public bool IsImmutable;
        [Header ("生成するプロパティ"), SerializeField] public List<PropertySettingsObject> PropertySettingsObjectList;
    }

    [Serializable]
    public class PropertySettingsObject {
        [Header ("プロパティのtype"), SerializeField] public DataType Type;
        [Header ("プロパティ名"), SerializeField] public string Name;
    }

    public enum DataType {
        Int = 0,
        Long = 1,
        Short = 2,
        Byte = 3,
        Float = 4,
        Double = 5,
        Bool = 6,
        Char = 7,
        String = 8,
        Object = 9,
    }

    public static class DataTypeExtension {
        public static string GetTypeName (this DataType type) {
            return type
            switch {
                DataType.Int => "int",
                    DataType.Long => "long",
                    DataType.Short => "short",
                    DataType.Byte => "byte",
                    DataType.Float => "float",
                    DataType.Double => "double",
                    DataType.Bool => "bool",
                    DataType.Char => "char",
                    DataType.String => "string",
                    DataType.Object => "object",
                    _ => string.Empty,
            };
        }
    }
}