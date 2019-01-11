using UnityEditor;

namespace ADL.Unity
{
    /// <summary>
    /// Custom Editor Window  for UnityInspector
    /// </summary>
    [CustomEditor(typeof(DebugComponent))]
    public  class DebugEditorWindow : Editor
    {
        /// <summary>
        /// List of string containing the tags.
        /// </summary>
        private SerializedProperty _debugLevel;
        
        void OnEnable()
        {
            _debugLevel = serializedObject.FindProperty("Configuration").FindPropertyRelative("MaskPrefix").FindPropertyRelative("Values");
        }

        /// <summary>
        /// Draw & Get Data to create a custom bitmask
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            string[] names = new string[_debugLevel.arraySize];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = _debugLevel.GetArrayElementAtIndex(i).stringValue;
            }
            DebugComponent._DebugLevel = names;
        }
    }
}
