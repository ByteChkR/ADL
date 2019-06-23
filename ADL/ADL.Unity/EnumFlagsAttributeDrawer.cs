using UnityEditor;
using UnityEngine;

namespace ADL.Unity
{
    /// <summary>
    ///     Draws the Actual mask field.
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var debuglevels = property.serializedObject.FindProperty("DebugLevel");

            if (DebugComponent.DebugLevel.Length > 0)
                property.intValue =
                    EditorGUI.MaskField(position, label, property.intValue, DebugComponent.DebugLevel);
        }
    }
}