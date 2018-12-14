using UnityEditor;

namespace ADL.Unity
{
    [CustomEditor(typeof(DebugComponent))]
    public sealed class DebugEditorWindow : Editor
    {
        SerializedProperty DebugLevel;
        SerializedProperty StreamArray;
        void OnEnable()
        {
            StreamArray = serializedObject.FindProperty("Streams");
            DebugLevel = serializedObject.FindProperty("DebugLevel");
        }


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
