using System;
using System.CodeDom;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace SF
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        TypeFilterAttribute typeFilter;
        string[] typeNames, typeFullNames;

        void Initialize()
        {
            if(typeFullNames != null) return;

            typeFilter = (TypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));

            var filteredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeFilter == null ? DefaultFilter(t) : typeFilter.Filter(t))
                .ToArray();

            typeNames = filteredTypes.Select(t => t.ReflectedType == null ? t.Name : $"{t.ReflectedType.Name} + {t.Name} ").ToArray();
            typeFullNames = filteredTypes.Select (t => t.AssemblyQualifiedName).ToArray();
        }

        static bool DefaultFilter(Type type)
        {
            return !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
        }

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
            Initialize();
            var typeIDProperty = property.FindPropertyRelative("_assemblyQualifiedName");

            if(string.IsNullOrEmpty(typeIDProperty.stringValue))
            {
                typeIDProperty.stringValue = typeFullNames.First();
                property.serializedObject.ApplyModifiedProperties();
            }

            var currentIndex = Array.IndexOf(typeFullNames, typeIDProperty.stringValue);
            var selectedIndex = EditorGUI.Popup(position,label.text, currentIndex, typeNames);

            if(selectedIndex >=0 && selectedIndex != currentIndex)
            {
                typeIDProperty.stringValue = typeFullNames[selectedIndex];
                property.serializedObject.ApplyModifiedProperties();
            }
		}
	}
}
