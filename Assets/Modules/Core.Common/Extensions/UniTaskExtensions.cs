using System.Threading;

namespace Utils.Project.Scripts.Modules.Utils.Extensions
{
    public static class UniTaskExtensions
    {
        public static void Clear(this CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
            {
                return;
            }
            
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}