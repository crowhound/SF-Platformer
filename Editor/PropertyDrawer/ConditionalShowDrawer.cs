using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;


namespace SF.PropertyDrawers.EditorModule
{
    [CustomPropertyDrawer(typeof(ConditionalShowAttribute))]
    public class ConditionalShowDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            ConditionalShowAttribute conditionalShow = attribute as ConditionalShowAttribute;
            bool conditionCheck = property.serializedObject.FindProperty(conditionalShow.PropertyName).boolValue;

            conditionalShow.ShowInInspector = conditionCheck;

            if (conditionalShow.ShowInInspector)
            {
                return new PropertyField(property);
            }
            else
                return new VisualElement();
        }
    }
}