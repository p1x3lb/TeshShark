using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Core
{
    public class GoalListItem : MonoBehaviour
    {
        [Inject]
        private ContentListConfig ContentListConfig { get; }

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private GameObject _point;

        [SerializeField]
        private Image _icon;

        private GoalModel _goal;

        public void SetData(GoalModel goal)
        {
            _goal = goal;
            var contentConfig = ContentListConfig.GetContent(goal.ContentType);
            _icon.sprite = contentConfig?.Sprite;
            _icon.SetNativeSize();
            OnGoalUpdated(goal);
            _goal.Updated += OnGoalUpdated;
        }

        private void OnDestroy()
        {
            _goal.Updated -= OnGoalUpdated;
        }

        private void OnGoalUpdated(GoalModel goal)
        {
            _text.text = $"{goal.Target - goal.Current}";
            if (goal.IsCompleted)
            {
                _text.gameObject.SetActive(false);
                _point.gameObject.SetActive(true);
            }
        }
    }
}