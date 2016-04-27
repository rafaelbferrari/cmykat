using UnityEngine;
using UnityEditor;
 
public class CMYKatEditor
{
    [MenuItem("CMYKat/Create/Level")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<LevelDescriptor>();
    }
}