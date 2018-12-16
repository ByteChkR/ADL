using UnityEditor;

namespace ADL.Unity
{
    /// <summary>
    /// Custom Editor Window  for UnityInspector
    /// </summary>
    [CustomEditor(typeof(DebugComponent))]
    public sealed class DebugEditorWindow : Editor
    {
        /// <summary>
        /// List of string containing the tags.
        /// </summary>
        SerializedProperty DebugLevel;
        
        void OnEnable()
        {
            DebugLevel = serializedObject.FindProperty("DebugLevel");
        }

        /// <summary>
        /// Draw & Get Data to create a custom bitmask
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            string[] names = new string[DebugLevel.arraySize];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = DebugLevel.GetArrayElementAtIndex(i).stringValue;
            }
            DebugComponent._DebugLevel = names;
        }
    }
}
