using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using Utils.Project.Scripts.Modules.Utils.Editor;
using Zenject;

namespace Project.Scripts.Infrastructure
{
    public class Entity : MonoBehaviour
    {
        [SerializeReference]
        private List<IComponent> _components;

        [SerializeField, ReadOnly]
        private int _id;

        private IECSManager _ecsManager;

        [Inject]
        public void Initialize(IECSManager ecsManager)
        {
            _ecsManager = ecsManager;
            ProduceEntity(ecsManager.World);
        }

        private void ProduceEntity(EcsWorld world)
        {
            var entity = world.NewEntity();
            _id = entity.GetInternalId();
            foreach (var component in _components)
            {
                //entity.Replace<IComponent>(component);
            }
        }
    }
}