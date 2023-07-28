using Leopotam.Ecs;

namespace Project.Scripts.Infrastructure
{
    public interface IECSManager
    {
        EcsWorld World { get; }
    }
}