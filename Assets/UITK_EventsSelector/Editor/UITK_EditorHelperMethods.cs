using UnityEditor;

namespace UIEvents
{
    public static class UITK_EditorHelperMethods
    {
        /// <summary>
        /// Finds first specified asset and gets path relative to it
        /// </summary>
        /// <param name="originToFind">Asset to look for. Use "t:type" to specify type and then name</param>
        /// <param name="subPath">Sub path in relation to found asset</param>
        /// <returns></returns>
       public static string GetPathRelativeTo(string originToFind, string subPath)
        {
            string scriptPath = AssetDatabase.FindAssets(originToFind)[0];
            string scriptFolder = System.IO.Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(scriptPath));
            return System.IO.Path.Combine(scriptFolder, subPath);
        }
    }
}
