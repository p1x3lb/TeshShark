using System.Collections.Generic;
using UnityEngine;

namespace Utils.Project.Scripts.Modules.Utils.Extensions
{
    public static class HitExtensions
    {
        public static IEnumerable<T> SelectComponent<T>(this IEnumerable<RaycastHit> components) where T : Component
        {
            foreach (var component in components)
            {
                if (component.transform.TryGetComponent(out T requiredComponent))
                {
                    yield return requiredComponent;
                }
            }
        }
    }
}