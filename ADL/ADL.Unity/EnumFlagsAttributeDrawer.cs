using UnityEditor;
using UnityEngine;

namespace ADL.Unity
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public sealed class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            SerializedProperty debuglevels = _property.serializedObject.FindProperty("DebugLevel");

            if(DebugComponent._DebugLevel.Length > 0) _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, DebugComponent._DebugLevel);
        }
    }
}
