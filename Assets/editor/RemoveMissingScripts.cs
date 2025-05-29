
using UnityEditor;
using UnityEngine;

public class RemoveMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Cleanup/Remove Missing Scripts In All Scenes")]
    static void RemoveInAllScenes()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.scene.isLoaded)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            }
        }

        Debug.Log("✔️ Removed all missing scripts in open scenes!");
    }
}

