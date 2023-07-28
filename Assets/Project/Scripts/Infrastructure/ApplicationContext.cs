using System.Threading;
using DG.Tweening;
using UnityEngine;

namespace Project.GameAssets
{
    public static class ApplicationContext
    {
        private static readonly CancellationTokenSource ApplicationCancellationTokenSource = new();

        public static CancellationToken Token => ApplicationCancellationTokenSource.Token;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            Application.wantsToQuit += OnApplicationWantsToQuit;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private static bool OnApplicationWantsToQuit()
        {
            Dispose();
            return true;
        }

        private static void Dispose()
        {
            DOTween.Clear(true);
            ApplicationCancellationTokenSource.Cancel();
        }
    }
}