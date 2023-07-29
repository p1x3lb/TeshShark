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
        private int _damage = 2;

        [SerializeField]
        private int _columns = 6;

        [SerializeField]
        private int _rows = 6;

        [SerializeField]
        private GameObject _mapPrefab;

        [SerializeReference]
        private List<IGoal> _goalConfigs = new();

        public int StepCount => _stepCount;
        public int Speed => _speed;
        public int Damage => _damage;

        public GameObject MapPrefab => _mapPrefab;
        public IReadOnlyList<IGoal> Goals => _goalConfigs;
        public int Rows => _rows;
        public int Columns => _columns;
    }
}