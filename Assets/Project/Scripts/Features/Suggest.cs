using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core;
using TMPro;
using UI;
using UnityEngine;

namespace Project.Scripts.Features
{
    [Serializable]
    public class Suggest
    {



        public string Text { get; }
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

    public class SuggestionsWindow : Window<SuggestWindowModel>
    {
        [SerializeField]
        private TMP_Text _text;

        protected override UniTask OnShow()
        {
            _text.text = Model.Suggest.Text;
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