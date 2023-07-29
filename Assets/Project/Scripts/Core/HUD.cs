using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.Scripts.Infrastructure;
using TMPro;
using UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Core
{
    public class HUDModel : WindowModel
    {
    }

    public class HUD : Window<HUDModel>
    {
        [Inject]
        private WindowManager WindowManager { get; }

        [Inject]
        private CoreStateContext CoreStateContext { get; }

        [Inject]
        private InputManager InputManager { get; }

        [Inject]
        private PlayerModel PlayerModel { get; }

        [Inject]
        private GoalsManager GoalsManager { get; }

        [Inject]
        private IInstantiator Instantiator { get; }

        [SerializeField]
        private GoalListItem _goalListItem;

        [SerializeField]
        private Transform _goalsContainer;

        [SerializeField]
        private TextMeshProUGUI _speedText;

        [SerializeField]
        private TextMeshProUGUI _levelText;

        [SerializeField]
        private TextMeshProUGUI _damageText;

        private readonly List<GoalListItem> _goalList = new List<GoalListItem>();

        [UsedImplicitly]
        public void OnClose()
        {
            WindowManager.ShowWindow<ExitWindow>(new ExitWindowModel());
        }

        protected override UniTask OnShow()
        {
            _levelText.text = (PlayerModel.PlayerLevel + 1).ToString();
            _speedText.text = CoreStateContext.Speed.ToString();
            _damageText.text = CoreStateContext.Damage.ToString();

            CoreStateContext.TurnsChanged += OnTurnsChanged;

            foreach (var goal in GoalsManager.ActiveGoals)
            {
                var goalView = Instantiator.InstantiatePrefabForComponent<GoalListItem>(_goalListItem, _goalsContainer);
                _goalList.Add(goalView);
                goalView.SetData(goal);
            }

            return base.OnShow();
        }

        protected override UniTask OnHide()
        {
            foreach (var item in _goalList)
            {
                Destroy(item);
            }

            CoreStateContext.TurnsChanged -= OnTurnsChanged;
            return base.OnHide();
        }

        private void OnTurnsChanged(int count)
        {
            _damageText.text = count.ToString();
        }
    }
}