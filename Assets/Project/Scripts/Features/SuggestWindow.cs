using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Features
{
    [Serializable]
    public class Suggest
    {
        [SerializeField]
        private Sprite _sprite;

        public Sprite Sprite => _sprite;
    }

    public class SuggestWindowModel : WindowModel
    {
        public Suggest Suggest { get; }
        public CoreStateContext Context { get; }

        public SuggestWindowModel(Suggest suggest, CoreStateContext context)
        {
            Suggest = suggest;
            Context = context;
        }
    }

    public class SuggestWindow : Window<SuggestWindowModel>
    {
        [SerializeField]
        private Image _image;

        protected override UniTask OnShow()
        {
            _image.sprite = Model.Suggest.Sprite;
            Model.Context.TurnsChanged += OnChanged;
            return base.OnShow();
        }

        private void OnChanged(int obj)
        {
            Hide().Forget();
        }

        protected override UniTask OnHide()
        {
            Model.Context.TurnsChanged -= OnChanged;
            return base.OnHide();
        }
    }
}