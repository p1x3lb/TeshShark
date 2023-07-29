using Cysharp.Threading.Tasks;
using GameStateMachine.Project.Scripts.Modules.GameStateMachine;
using JetBrains.Annotations;
using Project.Scripts.Core;
using Project.Scripts.Infrastructure;
using TMPro;
using UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Meta
{
    public class MetaWindowModel : WindowModel
    {

    }

    public class MetaWindow : Window<MetaWindowModel>
    {
        [Inject]
        private IGameStateMachine GameStateMachine { get; }

        [Inject]
        private PlayerModel PlayerModel { get; }

        [SerializeField]
        private TMP_Text _text;

        protected override UniTask OnShow()
        {
            _text.text = (PlayerModel.PlayerLevel + 1).ToString();
            return base.OnShow();
        }

        [UsedImplicitly]
        public void OnClick()
        {
            GameStateMachine.Enter<CoreState>();
        }
    }
}