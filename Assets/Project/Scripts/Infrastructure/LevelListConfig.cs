using System;
using System.Collections.Generic;
using Project.Scripts.Core;
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
            return _levels.Length == 0 ? null : _levels[index % _levels.Length];
        }
    }

    [Serializable]
    public class LevelConfig
    {
        [SerializeField]
        private int _stepCount = 5;

        [SerializeField]
        private int _speed = 5;

        [SerializeField]
        private GameObject _mapPrefab;

        [SerializeReference]
        private List<IGoal> _goalConfigs;

        public int StepCount => _stepCount;

        public GameObject MapPrefab => _mapPrefab;
        public int Speed => _speed;
        public IReadOnlyList<IGoal> Goals => _goalConfigs;
    }
}