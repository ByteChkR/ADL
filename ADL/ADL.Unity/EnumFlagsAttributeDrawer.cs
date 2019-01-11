using UnityEditor;
using UnityEngine;

namespace ADL.Unity
{

    /// <summary>
    /// Draws the Actual mask field.
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public  class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            SerializedProperty debuglevels = _property.serializedObject.FindProperty("DebugLevel");

            if(DebugComponent._DebugLevel.Length > 0) _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, DebugComponent._DebugLevel);
        }
    }
}
