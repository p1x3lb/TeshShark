using System;

namespace Project.Scripts.Core
{
    public interface ISpawnableGoal
    {
        Type ContentType { get; }
    }
}