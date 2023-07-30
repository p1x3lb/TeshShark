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
        public CoreStateContext CoreStateContext { get; }

        public HUDModel(CoreStateContext coreStateContext)
        {
            CoreStateContext = coreStateContext;
        }
    }

    public class HUD : Window<HUDModel>
    {
        [Inject]
        private WindowManager WindowManager { get; }

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

        [SerializeField]
        private TextMeshProUGUI _turnsText;

        [SerializeField]
        private Animator _animator;

        private readonly List<GoalListItem> _goalList = new List<GoalListItem>();

        [UsedImplicitly]
        public void OnClose()
        {
            WindowManager.ShowWindow<ExitWindow>(new ExitWindowModel());
        }

        protected override UniTask OnShow()
        {
            _levelText.text = $"Lvl. {(PlayerModel.PlayerLevel + 1).ToString()}";
            _speedText.text = Model.CoreStateContext.Speed.ToString();
            _damageText.text = Model.CoreStateContext.Damage.ToString();
            _turnsText.text = Model.CoreStateContext.StepsLeft.ToString();

            Model.CoreStateContext.TurnsChanged += OnTurnsChanged;

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

            Model.CoreStateContext.TurnsChanged -= OnTurnsChanged;
            return base.OnHide();
        }

        private void OnTurnsChanged(int count)
        {
            _turnsText.text = count.ToString();
            _animator.SetTrigger("Hit");
        }
    }
}