using System;
using UnityEngine;

namespace Project.Scripts.Infrastructure
{
    [CreateAssetMenu(fileName = "LevelListConfig", menuName = "Configs/LevelListConfig")]
    public class LevelListConfig : ScriptableObject
    {
        [SerializeField]
        private LevelConfig[] _levels;

        public LevelConfig GetLevel(int index)
        {
            return _levels[index % _levels.Length];
        }
    }

    [Serializable]
    public class LevelConfig
    {
        [SerializeField]
        private int _stepCount;

        [SerializeField]
        private GameObject _mapPrefab;

        public int StepCount => _stepCount;

        public GameObject MapPrefab => _mapPrefab;
    }
}