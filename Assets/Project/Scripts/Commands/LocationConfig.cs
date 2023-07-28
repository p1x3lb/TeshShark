using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.GameAssets.Commands
{
    [CreateAssetMenu(menuName = "Location/LocationConfig", fileName = "LocationConfig")]
    public class LocationConfig : ScriptableObject
    {
        [SerializeReference]
        private AssetReference _scene;

        public AssetReference Scene => _scene;
    }
}