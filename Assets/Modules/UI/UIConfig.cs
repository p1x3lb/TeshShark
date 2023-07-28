using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "UI/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [SerializeField]
        private List<WindowConfig> _configs;

        [UsedImplicitly]
        public static readonly List<string> WINDOWS = new();

#if UNITY_EDITOR
        static UIConfig()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(x => !x.IsAbstract && typeof(Window<>).IsAssignableFrom(x)))
                {
                    WINDOWS.Add(type.Name);
                }
            }
        }
#endif
        public GameObject GetWindowPrefab<T>() where T : Window
        {
            return _configs.FirstOrDefault(config => config.Window == typeof(T).Name)?.Prefab;
        }
    }

    [Serializable]
    public class WindowConfig
    {
        [SerializeField, HideLabel, ValueDropdown("@UI.UIConfig.WINDOWS")]
        private string _window;

        [SerializeField]
        private GameObject _prefab;

        public string Window => _window;

        public GameObject Prefab => _prefab;
    }
}