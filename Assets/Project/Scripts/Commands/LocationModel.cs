using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.GameAssets.Commands
{
    public interface ILocationModel
    {
        UniTask Initialize(CancellationToken cancellationToken);
    }

    public class LocationModel : ILocationModel
    {
        protected LocationModel(string path)
        {
        }

        public UniTask Initialize(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}