using UnityEditor;
using UnityEngine;

namespace RicKit.Editor
{
    public static class DeleteMissingScripts
    {
        [MenuItem("RicKit/Delete Missing Scripts")]
        private static void CleanupMissingScript()
        {
            foreach (var o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                var gameObject = (GameObject)o;
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            }
        }
    }
}