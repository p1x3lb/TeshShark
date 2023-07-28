using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using Project.Scripts.Infrastructure;
using Zenject;

namespace Project.Scripts.Core
{
    public abstract class EcsManager : ITickable, IDisposable, IInitializable, IECSManager
    {
        private List<EcsSystems> _systems;

        public EcsWorld World { get; private set; }

        public void Initialize()
        {
            World = new EcsWorld();

            foreach (var system in _systems)
            {
                system.ProcessInjects().Init();
            }


#if UNITY_EDITOR
            EcsWorldObserver.Create(World);
            foreach (var system in _systems)
            {
                EcsSystemsObserver.Create(system);
            }
#endif
        }

        protected EcsSystems CreateSystem()
        {
            var system = new EcsSystems(World);
            _systems.Add(system);
            return system;
        }

        public void Tick()
        {
            foreach (var system in _systems)
            {
                system.Run();
            }
        }

        public void Dispose()
        {
            foreach (var system in _systems)
            {
                system.Destroy();
            }

            _systems.Clear();
            World.Destroy();
            World = null;
        }

        protected abstract void Prepare();
    }
}