using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Project.Scripts.Modules.Utils.Extensions
{
    public static class LinQExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action?.Invoke(item);
            }
        }

        public static IEnumerable<T> SelectComponent<T>(this IEnumerable<Component> components) where T : Component
        {
            foreach (var component in components)
            {
                if (component.TryGetComponent(out T requiredComponent))
                {
                    yield return requiredComponent;
                }
            }
        }
    }
}