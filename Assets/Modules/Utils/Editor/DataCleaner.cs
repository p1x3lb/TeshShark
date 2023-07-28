using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils.Project.Scripts.Modules.Utils.Editor
{
    public class DataCleaner
    {
        [MenuItem("DEV/Del Persistent")]
        public static void DeleteSaves()
        {
            DeletePersistentDirectory("");
            PlayerPrefs.DeleteAll();
        }
        
        [MenuItem("DEV/Open persistent folder")]
        public static void OpenPersistentFolder()
        {
            Application.OpenURL(Application.persistentDataPath);
        }

        private static void DeletePersistentDirectory(string dirPath)
        {
            var path = Path.Combine(Application.persistentDataPath, dirPath);
            var dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                dir.Delete(true);
            }
        }
    }
}