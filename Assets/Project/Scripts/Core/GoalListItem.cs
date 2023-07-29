using TMPro;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class GoalListItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private GameObject _point;

        private GoalModel _goal;

        public void SetData(GoalModel goal)
        {
            _goal = goal;
            _goal.Updated += OnGoalUpdated;
        }

        private void OnDestroy()
        {
            _goal.Updated -= OnGoalUpdated;
        }

        private void OnGoalUpdated(GoalModel goal)
        {
            _text.text = $"{goal.Current}/{goal.Target}";
            if (goal.IsCompleted)
            {
                _text.gameObject.SetActive(false);
                _point.gameObject.SetActive(true);
            }
        }
    }
}