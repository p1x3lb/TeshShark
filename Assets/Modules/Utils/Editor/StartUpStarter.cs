#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.Project.Scripts.Modules.Utils.Editor
{
    public static class StartUpStarter
    {
        private const string PlayFromFirstMenuStr = "Play/Start from StartUp";

        private static bool PlayFromFirstScene
        {
            get{return EditorPrefs.HasKey(PlayFromFirstMenuStr) && EditorPrefs.GetBool(PlayFromFirstMenuStr);}
            set{EditorPrefs.SetBool(PlayFromFirstMenuStr, value);}
        }
        
        [MenuItem(PlayFromFirstMenuStr, false, 150)]
        private static void PlayFromFirstSceneCheckMenu()
        {
            var newValue = !PlayFromFirstScene;
            PlayFromFirstScene = newValue;
            Menu.SetChecked(PlayFromFirstMenuStr, newValue);
 
            ShowNotifyOrLog(newValue ? "Play from StartUp" : "Play from current scene");
        }
 
        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(PlayFromFirstMenuStr, true)]
        private static bool PlayFromFirstSceneCheckMenuValidate()
        {
            Menu.SetChecked(PlayFromFirstMenuStr, PlayFromFirstScene);
            return true;
        }
 
        // This method is called before any Awake. It's the perfect callback for this feature
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
        private static void LoadFirstSceneAtGameBegins()
        {
            if(!PlayFromFirstScene)
                return;
            
            SceneManager.LoadScene("BootstrapScene");
        }
 
        private static void ShowNotifyOrLog(string msg)
        {
            if(Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            else
                Debug.Log(msg); // When there's no scene view opened, we just print a log
        }
    }
}

#endif