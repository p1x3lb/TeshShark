using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UI;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class ExitWindowModel : WindowModel
    {

    }

    public class ExitWindow : Window<ExitWindowModel>
    {
        [UsedImplicitly]
        public void OnCancel()
        {
            Hide().Forget();
        }

        [UsedImplicitly]
        public void OnExit()
        {
            Application.Quit();
        }
    }
}