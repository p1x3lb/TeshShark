using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Core
{
    [CreateAssetMenu(fileName = "ContentListConfig", menuName = "Configs/ContentListConfig")]
    public class ContentListConfig : ScriptableObject
    {
        [SerializeField]
        private int _minRange = 3;

        [SerializeField]
        private int _maxRange = 6;

        [SerializeField]
        private List<ContentConfig> _contents;

        public int MinRange => _minRange;

        public int MaxRange => _maxRange;

        [UsedImplicitly]
        public static readonly List<string> CONTENT = new();

        public CellContent GetContent(Type spawnableGoalContentType)
        {
            return _contents.FirstOrDefault(x => x.Goal == spawnableGoalContentType.Name)?.CellContent;
        }

#if UNITY_EDITOR
        static ContentListConfig()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(x => !x.IsAbstract && typeof(CellContent).IsAssignableFrom(x)))
                {
                    CONTENT.Add(type.Name);
                }
            }
        }

#endif
    }

    [Serializable]
    public class ContentConfig
    {
        [SerializeField, HideLabel, ValueDropdown("@Project.Scripts.Core.ContentListConfig.CONTENT")]
        private string _goal;

        [SerializeField]
        private CellContent _cellContent;

        public string Goal => _goal;

        public CellContent CellContent => _cellContent;
    }
}