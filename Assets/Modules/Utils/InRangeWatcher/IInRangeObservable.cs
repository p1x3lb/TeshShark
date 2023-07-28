using System;

namespace Utils.Project.Scripts.Modules.Utils.InRangeWatcher
{
    public interface IInRangeObservable
    {
        event Action<IInRangeObservable> Missed;
    }
}